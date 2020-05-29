using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model.HighLevel
{
  public class RpkManager
  {
    private CfgCache cfgCahce;
    private ClassJavaPairCache classCache;
    private string rpkFileNameField;
    private string slrrRootDirField;
    private string rpkNameAsRefString;

    public string RpkFileName
    {
      get
      {
        return rpkFileNameField;
      }
      protected set
      {
        rpkFileNameField = value;
        rpkNameAsRefString = GetExternalRefStringFromRpkFnam(rpkFileNameField, slrrRootDirField);
      }
    }
    public string SlrrRootDir
    {
      get
      {
        return slrrRootDirField;
      }
      protected set
      {
        slrrRootDirField = value;
        rpkNameAsRefString = GetExternalRefStringFromRpkFnam(rpkFileNameField, slrrRootDirField);
      }
    }
    public BinaryRpk Rpk
    {
      get;
      private set;
    }

    public static string GetValueFromStringWithKey(string key,string entry)
    {
      if (entry == null)
        return "";
      var spl = entry.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
      foreach(var ln in spl)
      {
        if (ln.StartsWith(key))
          return ln.Substring(key.Length).Trim(' ', '\t');
      }
      return "";
    }
    public static string GetValueFromStringInnerEntryWithKey(string key,BinaryStringInnerEntry entry)
    {
      return GetValueFromStringWithKey(key, entry.StringData);
    }
    public static int GetTypeIDFromString(string str)
    {
      try
      {
        return int.Parse(str.Substring(2), System.Globalization.NumberStyles.HexNumber);
      }
      catch(Exception)
      {

      }
      return -1;
    }
    public static int TypeIDExternalRefPart(int TypeID)
    {
      return TypeID >> 16;

    }
    public static int TypeIDExternalIDPart(int TypeID)
    {
      return TypeID & 0x0000FFFF;
    }
    public static int ExtRefIndToExtRefPart(int extrefInd)
    {
      return extrefInd << 16;
    }
    public static string GetExternalRefStringFromRpkFnam(string rpkFnam,string slrrRoot)
    {
      if (rpkFnam == null || slrrRoot == null)
        return "";
      var fullPath = Path.GetFullPath(rpkFnam).ToLower();
      return fullPath.Replace(Path.GetFullPath(slrrRoot).ToLower(), "").Replace(".rpk", "\\");
    }
    public static string GetRpkFnamFromExternalRefString(string refString, string slrrRoot)
    {
      if (refString == null || slrrRoot == null)
        return "";
      return Path.GetFullPath(slrrRoot + "\\" + refString).ToLower();
    }

    public RpkManager(string rpkfnam, string slrrRoot,bool reportErrors = true)
    {
      RpkFileName = rpkfnam;
      SlrrRootDir = slrrRoot;
      Rpk = new BinaryRpk(rpkfnam, true);
      cfgCahce = new CfgCache(slrrRoot);
      classCache = new ClassJavaPairCache(slrrRoot);

      foreach(var res in Rpk.RESEntries)
      {
        if(resKeyToResEntry.ContainsKey(res.TypeID) && reportErrors)
        {
          MessageLog.AddError("TypeID collision in " + RpkFileName + " TypeID: 0x" + res.TypeID.ToString("X8"), rpkfnam, 0);
          resKeyCollisionResolving.Add(resKeyToResEntry[res.TypeID]);
        }
        resKeyToResEntry[res.TypeID] = res;
      }
    }

    public Dictionary<int, BinaryResEntry> resKeyToResEntry = new Dictionary<int, BinaryResEntry>();
    public List<BinaryResEntry> resKeyCollisionResolving = new List<BinaryResEntry>();
    public Dictionary<int, Dictionary<int, CfgSlotCacheEntry>> typeIDTOSlotToSlotCacheEntry = new Dictionary<int, Dictionary<int, CfgSlotCacheEntry>>();
    public CfgSlotCacheEntry SlotCacheFromTypeIDSlotID(int typeID,int slotID,bool reportErrors = false)
    {
      Dictionary<int, CfgSlotCacheEntry> tempDict;
      if(typeIDTOSlotToSlotCacheEntry.TryGetValue(typeID, out tempDict))
      {
        CfgSlotCacheEntry ret;
        if(tempDict.TryGetValue(slotID,out ret))
        {
          return ret;
        }
        else
        {
          if(reportErrors)
            MessageLog.AddError("TypeID (0x" + typeID.ToString("X8") + ") defines cfg: " + GetCfgFnamOrEmptyStringFromTypeID(typeID)
                                + "in rpk: "+RpkFileName+" but does not define nonempty SlotID: " + slotID.ToString());
        }
      }
      else
      {
        if (reportErrors)
          MessageLog.AddError("TypeID (0x" + typeID.ToString("X8") + ") does not define a cfg in rpk: "+RpkFileName);
      }
      return null;
    }
    public List<CfgSlotReference> GetFlatAttachSourcesForAttachingLocalRefSpaceTypeIDSlotID(int typeID, int slotID)
    {
      var ret = new List<CfgSlotReference>();
      foreach (var dict in typeIDTOSlotToSlotCacheEntry.Values)
      {
        foreach (var slotCache in dict.Values)
        {
          if (slotCache.AttachLines.Any(x => x.SlotID == slotID && x.TypeID == typeID))
          {
            ret.Add(new CfgSlotReference(slotCache.TypeIDOfSlotOwner, slotCache.SlotID));
            continue;
          }
        }
      }
      return ret;
    }
    public List<CfgSlotReference> GetFlatCompatibleSourcesForCompatibleToLocalRefSpaceTypeIDSlotID(int typeID, int slotID)
    {
      var ret = new List<CfgSlotReference>();
      foreach (var dict in typeIDTOSlotToSlotCacheEntry.Values)
      {
        foreach (var slotCache in dict.Values)
        {
          if (slotCache.CompatibleLines.Any(x => x.SlotID == slotID && x.TypeID == typeID))
          {
            ret.Add(new CfgSlotReference(slotCache.TypeIDOfSlotOwner, slotCache.SlotID));
            continue;
          }
        }
      }
      return ret;
    }
    public Dictionary<CfgSlotReference, CfgSlotReference> compatibleReferences = new Dictionary<CfgSlotReference, CfgSlotReference>();
    public BinaryResEntry GetResEntry(int typeid,bool reportErrors = true)
    {
      if (resKeyToResEntry.ContainsKey(typeid))
      {
        if (resKeyCollisionResolving.Any(x => x.TypeID == typeid))
        {
          MessageLog.AddError("Multiply defined TypeID(0x" + typeid.ToString("X8") + ") refed", RpkFileName, 0);
        }
        return resKeyToResEntry[typeid];
      }
      MessageLog.AddError("NOT defined TypeID(0x" + typeid.ToString("X8") + ") refed", RpkFileName, 0);
      return null;
    }
    public BinaryStringInnerEntry GetStrEntryFromTypeID(int typeid,bool reportErrors = true)
    {
      var res = GetResEntry(typeid,reportErrors);
      if(res != null)
      {
        if(res.RSD.InnerEntries.Count() == 1 && res.RSD.InnerEntries.First() is BinaryStringInnerEntry)
          return res.RSD.InnerEntries.First() as BinaryStringInnerEntry;
        else if(reportErrors)
        {
          MessageLog.AddError("Defined TypeID(0x" + typeid.ToString("X8") + ") does not (only) have a StringInnerEntry", RpkFileName, 0);
        }
      }
      return null;
    }
    public string GetHeuristicScxFnamFromLocalPartTypeID(int localPartTypeID, bool reportErrors = true)
    {
      try
      {
        var meshRes = GetHeuristicMeshResFromLocalPartTypeID(localPartTypeID, reportErrors);
        if (meshRes == null)
          return null;
        return getValFromRSDkeyValue((meshRes.RSD.InnerEntries.First() as BinaryStringInnerEntry).StringData, "sourcefile");
      }
      catch(Exception)
      {
        if(reportErrors)
        {
          MessageLog.AddError("Failed to retrive heuristic Scx for part: 0x" + localPartTypeID.ToString("X8") + " from RPK: " + RpkFileName);
        }
      }
      return null;
    }
    public BinaryResEntry GetHeuristicMeshResFromLocalPartTypeID(int localPartTypeID, bool reportErrors = true)
    {
      try
      {
        var cfg = GetCfgFromTypeID(localPartTypeID, reportErrors);
        var keyOfLines = "render";
        if (cfg.FirstIndexOfNamedLine("chassis") != -1)
        {
          keyOfLines = "deformable";
        }
        BinaryResEntry ret = null;
        long maxSize = -1;
        var slrrRoot = GameFileManager.GetSLRRRoot(RpkFileName);
        foreach (var renderLine in cfg.LinesWithName(keyOfLines))
        {
          int renderID = 0;
          if (renderLine.Tokens.Count > 0 && renderLine.Tokens[1].IsValueHexInt)
          {
            renderID = renderLine.Tokens[1].ValueAsHexInt;
          }
          if (renderID == 0)
            continue;
          var render = Rpk.RESEntries.FirstOrDefault(x => x.TypeID == renderID && x.TypeOfEntry == 14);
          if (render == null)
            continue;
          if (!render.RSD.InnerEntries.Any())
            continue;
          if (!(render.RSD.InnerEntries.First() is BinaryStringInnerEntry))
            continue;
          var renderRSD = (render.RSD.InnerEntries.First() as BinaryStringInnerEntry).StringData;
          var meshIDList = getValTypeIDParis(renderRSD, "mesh");
          if (!meshIDList.Any())
            continue;
          var meshID = meshIDList.First();
          var mesh = Rpk.RESEntries.FirstOrDefault(x => x.TypeID == meshID && x.TypeOfEntry == 5);
          if (mesh == null)
            continue;
          if (!mesh.RSD.InnerEntries.Any())
            continue;
          if (!(mesh.RSD.InnerEntries.First() is BinaryStringInnerEntry))
            continue;
          var meshRSD = (mesh.RSD.InnerEntries.First() as BinaryStringInnerEntry).StringData;
          string scxName = getValFromRSDkeyValue(meshRSD, "sourcefile");
          FileInfo finf = new FileInfo(slrrRoot + "\\" + scxName);
          if (finf.Length > maxSize)
          {
            maxSize = finf.Length;
            ret = mesh;
          }
        }
        return ret;
      }
      catch (Exception)
      {
        if (reportErrors)
        {
          MessageLog.AddError("Failed to retrive heuristic Scx for part: 0x" + localPartTypeID.ToString("X8") + " from RPK: " + RpkFileName);
        }
      }
      return null;
    }
    public BinaryResEntry GetHeuristicRenderResFromLocalPartTypeID(int localPartTypeID, bool reportErrors = true)
    {
      try
      {
        var cfg = GetCfgFromTypeID(localPartTypeID, reportErrors);
        var keyOfLines = "render";
        if (cfg.FirstIndexOfNamedLine("chassis") != -1)
        {
          keyOfLines = "deformable";
        }
        BinaryResEntry ret = null;
        long maxSize = -1;
        var slrrRoot = GameFileManager.GetSLRRRoot(RpkFileName);
        foreach (var renderLine in cfg.LinesWithName(keyOfLines))
        {
          int renderID = 0;
          if (renderLine.Tokens.Count > 0 && renderLine.Tokens[1].IsValueHexInt)
          {
            renderID = renderLine.Tokens[1].ValueAsHexInt;
          }
          if (renderID == 0)
            continue;
          var render = Rpk.RESEntries.FirstOrDefault(x => x.TypeID == renderID && x.TypeOfEntry == 14);
          if (render == null)
            continue;
          if (!render.RSD.InnerEntries.Any())
            continue;
          if (!(render.RSD.InnerEntries.First() is BinaryStringInnerEntry))
            continue;
          var renderRSD = (render.RSD.InnerEntries.First() as BinaryStringInnerEntry).StringData;
          var meshIDList = getValTypeIDParis(renderRSD, "mesh");
          if (!meshIDList.Any())
            continue;
          var meshID = meshIDList.First();
          var mesh = Rpk.RESEntries.FirstOrDefault(x => x.TypeID == meshID && x.TypeOfEntry == 5);
          if (mesh == null)
            continue;
          if (!mesh.RSD.InnerEntries.Any())
            continue;
          if (!(mesh.RSD.InnerEntries.First() is BinaryStringInnerEntry))
            continue;
          var meshRSD = (mesh.RSD.InnerEntries.First() as BinaryStringInnerEntry).StringData;
          string scxName = getValFromRSDkeyValue(meshRSD, "sourcefile");
          FileInfo finf = new FileInfo(slrrRoot + "\\" + scxName);
          if (finf.Length > maxSize)
          {
            maxSize = finf.Length;
            ret = render;
          }
        }
        return ret;
      }
      catch (Exception)
      {
        if (reportErrors)
        {
          MessageLog.AddError("Failed to retrive heuristic Scx for part: 0x" + localPartTypeID.ToString("X8") + " from RPK: " + RpkFileName);
        }
      }
      return null;
    }
    public int GetPartScxPaintableTextureIndex(int localPartTypeID,bool reportErrors = true)
    {
      try
      {
        var cfg = GetCfgFromTypeID(localPartTypeID, reportErrors);
        var textureID = cfg.LastLineWithName("texture").Tokens[1].ValueAsHexInt;
        var render = GetHeuristicRenderResFromLocalPartTypeID(localPartTypeID, reportErrors);
        var renderRSD = (render.RSD.InnerEntries.First() as BinaryStringInnerEntry).StringData;
        var textures = getValTypeIDParis(renderRSD, "texture");
        return textures.IndexOf(textureID);
      }
      catch(Exception)
      {
        if (reportErrors)
        {
          MessageLog.AddError("Failed to retrive paintable UV for part: 0x" + localPartTypeID.ToString("X8") + " from RPK: " + RpkFileName);
        }
      }
      return -1;
    }
    public string GetRpkAsRPKRefString()
    {
      return rpkNameAsRefString.TrimStart('\\','/');
    }
    public string GetRpkAsScriptRPKRefString()
    {
      return rpkNameAsRefString.Replace('\\','.').Trim('.');
    }
    public string GetRpkExternalRefIndAsRPKRefString(int externalRefInd)
    {
      if (externalRefInd == 0)
        return GetRpkAsRPKRefString();
      externalRefInd--;
      if(externalRefInd >= 0 && Rpk.ExternalReferences.Count() > externalRefInd)
      {
        return Rpk.GetNthExternalReference(externalRefInd).ReferenceString;
      }
      return "";
    }
    public Cfg GetCfgFromTypeID(int typeID,bool reportErrors = true)
    {
      List<CfgLine> ret = new List<CfgLine>();
      if (resKeyToResEntry.ContainsKey(typeID))
      {
        if (reportErrors && resKeyCollisionResolving.Any(x => x.TypeID == typeID))
        {
          MessageLog.AddError("Trying to get Cfg of TypeID(0x" + typeID.ToString("X8") + ") that is multiply defined in rpk: " + RpkFileName, RpkFileName, 0);
        }
        var res = resKeyToResEntry[typeID];
        if (res.RSD.InnerEntries.Count() == 1 && res.RSD.InnerEntries.First() is BinaryStringInnerEntry)
        {
          var strInnerEntry = res.RSD.InnerEntries.First() as BinaryStringInnerEntry;
          var tokensWithCfg = strInnerEntry.StringData.Split(new string[] { "\r", "\n", " ", "\t" }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.ToLower().EndsWith(".cfg")).ToList();
          if (tokensWithCfg.Count > 1)
          {
            if (reportErrors)
              MessageLog.AddError("Trying to get Cfg of TypeID(0x" + typeID.ToString("X8") + ") that defines more than one cfg rpk: " + RpkFileName + "\n\t" + tokensWithCfg.Aggregate((x,
                                  y) => x + ", " + y), RpkFileName, 0);
          }
          if (tokensWithCfg.Count == 0)
          {
            if (reportErrors)
              MessageLog.AddError("Trying to get Cfg of TypeID(0x" + typeID.ToString("X8") + ") that does not define a cfg rpk: " + RpkFileName, RpkFileName, 0);
          }
          else
          {
            return cfgCahce.GetCFGFromFileName(tokensWithCfg.First());
          }
        }
        else if(reportErrors)
        {
          MessageLog.AddError("Trying to get Cfg of TypeID(0x" + typeID.ToString("X8") + ") that does not define a cfg rpk: " + RpkFileName, RpkFileName, 0);
        }
      }
      else if(reportErrors)
      {
        //MessageLog.AddError("Tried to get Cfg of TypeID(0x" + typeID.ToString("X8") + ") not defined in rpk: " + rpkFileName, rpkFileName, 0);
      }
      if (reportErrors && resKeyCollisionResolving.Any(x => x.TypeID == typeID))
      {
        return getCfgFromMultiplyDefinedTypeID(typeID, reportErrors);
      }
      return null;
    }
    public CfgSlotLine GetSlotFromTypeIDAndSlotID(int typeID, int slotID,bool reportErrors = false)
    {
      var cfg = GetCfgFromTypeID(typeID, false);
      if (cfg == null)
      {
        if (reportErrors)
          MessageLog.AddError("TypeID(0x"+typeID.ToString("X8")+") does not define a Cfg in rpk: "+RpkFileName, RpkFileName, 0);
        return null;
      }
      var slotWithID = cfg.Slots.Where(x => x.SlotID == slotID).ToList();
      if (slotWithID.Count > 1)
      {
        if (reportErrors)
          MessageLog.AddError("Multiply defined SlotID(" + slotID.ToString() + " in cfg: " + cfg.CfgFileName +
                              " identified by typeID: 0x" + typeID.ToString("X8") + ") referenced ", cfg.CfgFileName,0);
      }
      if (slotWithID.Count == 0)
      {
        if (reportErrors)
          MessageLog.AddError("Undefined SlotID( " + slotID.ToString() + " in cfg: " + cfg.CfgFileName +
                              " identified by typeID: 0x"+typeID.ToString("X8")+") referenced", cfg.CfgFileName, 0);
      }
      else
      {
        return slotWithID.First();
      }
      return null;
    }
    public List<CfgPartPositionLine> GetPositionLinesFromTypeID(int typeID)
    {
      var cfg = GetCfgFromTypeID(typeID);
      if(cfg != null)
      {
        return new List<CfgPartPositionLine>(cfg.LinesWithPositionData);
      }
      return new List<CfgPartPositionLine>();
    }
    public string GetCfgFnamOrEmptyStringFromTypeID(int typeID)
    {
      var cfg = GetCfgFromTypeID(typeID);
      if(cfg == null)
      {
        return "";
      }
      return cfg.CfgFileName;
    }
    public bool IsFullFnamProbableForLocalPath(string fullFnam)
    {
      string rpkPath = RpkFileName;
      rpkPath = rpkPath.Replace("/", "\\").Replace("\\\\", "\\").ToLower();
      string rpkPathWithoutExtension = rpkPath.Substring(0, rpkPath.Length - 4);
      string fixedFullFnam = fullFnam.Replace("/", "\\").Replace("\\\\", "\\").ToLower();
      int lastp = fixedFullFnam.LastIndexOf('.');
      if (lastp != -1)
        fixedFullFnam = fixedFullFnam.Substring(0, lastp);
      return (fixedFullFnam.StartsWith(rpkPathWithoutExtension));
    }
    public ClassJavaPair GetClassPairFromTypeID(int typeID, bool reportErrors = true,bool tryRedefinitionsIfMainResEntryFails = false)
    {
      List<CfgLine> ret = new List<CfgLine>();
      if (resKeyToResEntry.ContainsKey(typeID))
      {
        //if (reportErrors && resKeyCollisionResolving.Any(x => x.TypeID == typeID))
        //{
        //  MessageLog.AddError("Trying to get script of TypeID(0x" + typeID.ToString("X8") + ") that is multiply defined in rpk: " + rpkFileName, rpkFileName, 0);
        //}
        var res = resKeyToResEntry[typeID];
        if (res.RSD.InnerEntries.Count() == 1 && res.RSD.InnerEntries.First() is BinaryStringInnerEntry)
        {
          var strInnerEntry = res.RSD.InnerEntries.First() as BinaryStringInnerEntry;
          var tokensWithClass = strInnerEntry.StringData.Split(new string[] { "\r", "\n", " ", "\t" }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.ToLower().EndsWith(".class")).ToList();
          if (tokensWithClass.Count > 1)
          {
            if (reportErrors)
              MessageLog.AddError("Trying to get script of TypeID(0x" + typeID.ToString("X8") + ") that defines more than one scripts rpk: " + RpkFileName + "\n\t"
                                  + tokensWithClass.Aggregate((x,y) => x + ", " + y), RpkFileName, 0);
          }
          if (tokensWithClass.Count == 0)
          {
            if (reportErrors)
              MessageLog.AddError("Trying to get script of TypeID(0x" + typeID.ToString("X8") + ") that does not define a script rpk: " + RpkFileName, RpkFileName, 0);
          }
          else
          {
            if (reportErrors && resKeyCollisionResolving.Any(x => x.TypeID == typeID))
            {
              MessageLog.AddError("Returning script of TypeID(0x" + typeID.ToString("X8") + ") that is multiply defined in rpk: " + RpkFileName, RpkFileName, 0);
            }
            return classCache.GetPairFromClassFileName(tokensWithClass.First());
          }
        }
        else if (reportErrors)
        {
          MessageLog.AddError("Trying to get script of TypeID(0x" + typeID.ToString("X8") + ") that does not define a script rpk: " + RpkFileName, RpkFileName, 0);
        }
      }
      else if (reportErrors)
      {
        //MessageLog.AddError("Tried to get Cfg of TypeID(0x" + typeID.ToString("X8") + ") not defined in rpk: " + rpkFileName, rpkFileName, 0);
      }
      if (resKeyCollisionResolving.Any(x => x.TypeID == typeID) && tryRedefinitionsIfMainResEntryFails)
      {
        if(reportErrors)
          MessageLog.AddError("Falling back to redefinitions of script of TypeID(0x" + typeID.ToString("X8") + ") that is multiply defined in rpk: " + RpkFileName, RpkFileName, 0);
        return getClassPairFromMultiplyDefinedTypeID(typeID, reportErrors);
      }
      return null;
    }
    public int GetIndexOfExternalRPKRefFromStringVal(string extrRefToLookUp)
    {
      var extrRef = extrRefToLookUp;
      if (extrRef.EndsWith("\\") || extrRef.EndsWith("/"))
        extrRef = extrRef.Substring(0, extrRef.Length - 1) + ".rpk";
      extrRef = extrRef.Replace("/", "\\").Trim('\\');
      int extRefCount = Rpk.ExternalReferences.Count();
      var extRefList = Rpk.ExternalReferences.ToList();
      for (int extRef_i = 0; extRef_i != extRefCount; ++extRef_i )
      {
        if (extRefList[extRef_i].ReferenceString.ToLower().Trim('\\','/') == extrRef.ToLower())
          return extRef_i;
      }
      return -1;
    }
    public int GetTypeIDInLocalExternalRefSpace(string externalRefStringCurrentlyRefed, int typeID)
    {
      int extrefInd = GetIndexOfExternalRPKRefFromStringVal(externalRefStringCurrentlyRefed);
      if (extrefInd == -1 && externalRefStringCurrentlyRefed != GetRpkAsRPKRefString())
        return -1;
      extrefInd++;
      return ExtRefIndToExtRefPart(extrefInd) + TypeIDExternalIDPart(typeID);
    }
    public IEnumerable<CfgSlotReference> GetAttachLinesFromTypeIDAndSlotID(int typeID, int slotID)
    {
      List<CfgSlotReference> ret = new List<CfgSlotReference>();
      var slot = GetSlotFromTypeIDAndSlotID(typeID, slotID);
      if(slot != null)
      {
        foreach (var attachLine in slot.AttachLines)
        {
          if (!CfgSlotReference.IsValidLine(attachLine))
          {
            MessageLog.AddError("Bad Attach line:" + attachLine.ToString() + "\n\t in rpk: " + RpkFileName + "\n\tcfgFnam: " + GetCfgFromTypeID(typeID).CfgFileName, RpkFileName, 0);
          }
          else
          {
            ret.Add(new CfgSlotReference(attachLine));
          }
        }
      }
      return ret;
    }
    public IEnumerable<CfgSlotReference> GetCompatibleLinesFromTypeIDAndSlotID(int typeID, int slotID)
    {
      List<CfgSlotReference> ret = new List<CfgSlotReference>();
      var slot = GetSlotFromTypeIDAndSlotID(typeID, slotID);
      if (slot != null)
      {
        foreach (var compatibleLine in slot.CompatibleLines)
        {
          if (!CfgSlotReference.IsValidLine(compatibleLine))
          {
            MessageLog.AddError("Bad Compatible line:"+ compatibleLine.ToString()+ "\n\t in rpk: " + RpkFileName + "\n\tcfgFnam: " + GetCfgFromTypeID(typeID).CfgFileName, RpkFileName, 0);
          }
          else
          {
            ret.Add(new CfgSlotReference(compatibleLine));
          }
        }
      }
      return ret;
    }
    public void BuildSlotCache()
    {
      foreach(var resEntry in resKeyToResEntry.Values)
      {
        var cfg = GetCfgFromTypeID(resEntry.TypeID,false);
        if(cfg != null)
        {
          foreach(var slot in cfg.Slots)
          {
            //if ((slot.AttachLines.Any() || slot.CompatibleLines.Any())) // there are empty slots ... man this cfg system is fucked ....
            //{
            if(!typeIDTOSlotToSlotCacheEntry.ContainsKey(resEntry.TypeID))
              typeIDTOSlotToSlotCacheEntry[resEntry.TypeID] = new Dictionary<int, CfgSlotCacheEntry>();
            if (!typeIDTOSlotToSlotCacheEntry[resEntry.TypeID].ContainsKey(slot.SlotID))
              typeIDTOSlotToSlotCacheEntry[resEntry.TypeID][slot.SlotID] = new CfgSlotCacheEntry(resEntry.TypeID, slot.SlotID);
            foreach(var attach in slot.AttachLines)
            {
              typeIDTOSlotToSlotCacheEntry[resEntry.TypeID][slot.SlotID].AttachLines.Add(new CfgSlotReference(attach));
            }
            foreach(var compatible in slot.CompatibleLines)
            {
              var compRef = new CfgSlotReference(compatible);
              typeIDTOSlotToSlotCacheEntry[resEntry.TypeID][slot.SlotID].CompatibleLines.Add(compRef);
              if(!compatibleReferences.ContainsKey(compRef))
              {
                compatibleReferences[compRef] = new CfgSlotReference(resEntry.TypeID, slot.SlotID);
              }
            }
            //}
          }
        }
      }
    }
    public void ReplaceInAllBodyLineModelEntries(string from, string to)
    {
      foreach(var id in Rpk.RESEntries)
      {
        var cfg = GetCfgFromTypeID(id.TypeID,false);
        if (cfg == null)
          continue;
        bool wasMatch = false;
        foreach(var bodyLine in cfg.BodyLines)
        {
          if(bodyLine.BodyModel.Contains(from))
          {
            wasMatch = true;
            MessageLog.AddMessage("BodyLine: " + bodyLine.ToString() + " in cfg: " + cfg.CfgFileName + " has match for: " + from);
            bodyLine.BodyModel = bodyLine.BodyModel.Replace(from, to);
            MessageLog.AddMessage("Updated BodyLine: " + bodyLine.ToString() + " in cfg: " + cfg.CfgFileName);
          }
        }
        if(wasMatch)
        {
          MessageLog.AddMessage("There was match for bodyline model replace from: " + from + " to: " + to + " in cfg: " + cfg.CfgFileName+" will save now.");
          cfg.Save();
        }
      }
    }
    public override string ToString()
    {
      return RpkFileName;
    }

    //these may be too specific...
    public bool IsTypeIDPartDefinition(int typeID)
    {
      if(!resKeyToResEntry.ContainsKey(typeID))
      {
        return false;
      }
      var innerEntries = resKeyToResEntry[typeID].RSD.InnerEntries.ToList();
      if(innerEntries.Count == 1)
      {
        if(innerEntries.First() is BinaryStringInnerEntry)
        {
          var strEntry = innerEntries.First() as BinaryStringInnerEntry;
          if(strEntry.StringData.Split(new string[] {"\r","\n"},StringSplitOptions.RemoveEmptyEntries).Any(x => x.ToLower().StartsWith("native part")))
          {
            return true;
          }
        }
      }
      return false;
    }
    public bool IsTypeIDCarDefinition(int typeID)
    {
      if (!resKeyToResEntry.ContainsKey(typeID))
      {
        return false;
      }
      var innerEntries = resKeyToResEntry[typeID].RSD.InnerEntries.ToList();
      if (innerEntries.Count == 1)
      {
        if (innerEntries.First() is BinaryStringInnerEntry)
        {
          var strEntry = innerEntries.First() as BinaryStringInnerEntry;
          if (strEntry.StringData.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).Any(x => x.ToLower().StartsWith("native car")))
          {
            return true;
          }
        }
      }
      return false;
    }
    public IEnumerable<int> PartDefTypeIDs()
    {
      return resKeyToResEntry.Keys.Where(x => IsTypeIDPartDefinition(x));
    }
    public IEnumerable<int> CarDefTypeIDs()
    {
      return resKeyToResEntry.Keys.Where(x => IsTypeIDCarDefinition(x));
    }

    private static string removeLineComments(string line)
    {
      string ret = line;
      var blackList = new string[] { ";", "//", "#" };
      foreach(var black in blackList)
      {
        int indexof = line.IndexOf(black);
        if(indexof != -1)
        {
          ret = ret.Remove(indexof);
        }
      }
      return ret;
    }
    private static List<int> getValTypeIDParis(string rsd, string val)
    {
      List<int> ret = new List<int>();
      var spl = rsd.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
      foreach (var lineComment in spl)
      {
        var line = removeLineComments(lineComment);
        var lineSpl = line.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
        if (lineSpl.Any() && lineSpl[0] == val)
        {
          int typeIDVal = -1;
          if (int.TryParse(lineSpl[1].Substring(2), System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out typeIDVal))
          {
            ret.Add(typeIDVal);
          }
        }
      }
      return ret;
    }
    private static string getValFromRSDkeyValue(string rsd, string key, int valInd = 1)
    {
      var spl = rsd.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
      foreach (var lineComment in spl)
      {
        var line = removeLineComments(lineComment);
        var lineSpl = line.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
        if (lineSpl.Any() && lineSpl[0] == key && lineSpl.Length > valInd)
        {
          return lineSpl[valInd];
        }
      }
      return "";
    }

    private BinaryResEntry getResEntryAddErrorIfMultipleDefined(int typeid)
    {
      if(resKeyToResEntry.ContainsKey(typeid))
      {
        if(resKeyCollisionResolving.Any(x => x.TypeID == typeid))
        {
          MessageLog.AddError("Multiply defined TypeID(0x" + typeid.ToString("X8") + ") refed", RpkFileName, 0);
        }
        return resKeyToResEntry[typeid];
      }
      return null;
    }
    private Cfg getCfgFromMultiplyDefinedTypeID(int typeID,bool reportErrors = true)
    {
      foreach (var res in resKeyCollisionResolving.Where(x => x.TypeID == typeID))
      {
        if (res.RSD.InnerEntries.Count() == 1 && res.RSD.InnerEntries.First() is BinaryStringInnerEntry)
        {
          var strInnerEntry = res.RSD.InnerEntries.First() as BinaryStringInnerEntry;
          var tokensWithCfg = strInnerEntry.StringData.Split(new string[] { "\r", "\n", " ", "\t" }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.ToLower().EndsWith(".cfg")).ToList();
          if (tokensWithCfg.Count > 1)
          {
            if (reportErrors)
              MessageLog.AddError("Trying to get Cfg of TypeID(0x" + typeID.ToString("X8") + ") that defines more than one cfg rpk: " + RpkFileName + "\n\t" + tokensWithCfg.Aggregate((x,
                                  y) => x + ", " + y), RpkFileName, 0);
          }
          if (tokensWithCfg.Count == 0)
          {
            if (reportErrors)
              MessageLog.AddError("Trying to get Cfg of TypeID(0x" + typeID.ToString("X8") + ") that does not define a cfg rpk: " + RpkFileName, RpkFileName, 0);
          }
          else
          {
            return cfgCahce.GetCFGFromFileName(tokensWithCfg.First());
          }
        }
        else if (reportErrors)
        {
          MessageLog.AddError("Trying to get Cfg of TypeID(0x" + typeID.ToString("X8") + ") that does not define a cfg rpk: " + RpkFileName, RpkFileName, 0);
        }
      }
      return null;
    }
    private ClassJavaPair getClassPairFromMultiplyDefinedTypeID(int typeID, bool reportErrors = true)
    {
      foreach (var res in resKeyCollisionResolving.Where(x => x.TypeID == typeID))
      {
        if (res.RSD.InnerEntries.Count() == 1 && res.RSD.InnerEntries.First() is BinaryStringInnerEntry)
        {
          var strInnerEntry = res.RSD.InnerEntries.First() as BinaryStringInnerEntry;
          var tokensWithClass = strInnerEntry.StringData.Split(new string[] { "\r", "\n", " ", "\t" }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.ToLower().EndsWith(".class")).ToList();
          if (tokensWithClass.Count > 1)
          {
            if (reportErrors)
              MessageLog.AddError("Trying to get Script of TypeID(0x" + typeID.ToString("X8") + ") that defines more than one class rpk: " + RpkFileName + "\n\t" + tokensWithClass.Aggregate((x,
                                  y) => x + ", " + y), RpkFileName, 0);
          }
          if (tokensWithClass.Count == 0)
          {
            if (reportErrors)
              MessageLog.AddError("Trying to get Script of TypeID(0x" + typeID.ToString("X8") + ") that does not define a class rpk: " + RpkFileName, RpkFileName, 0);
          }
          else
          {
            MessageLog.AddMessage("Returning class from fall back for TypeID(0x" + typeID.ToString("X8") + ") in rpk: " + RpkFileName, RpkFileName, 0);
            return classCache.GetPairFromClassFileName(tokensWithClass.First());
          }
        }
        else if (reportErrors)
        {
          MessageLog.AddError("Trying to get Script of TypeID(0x" + typeID.ToString("X8") + ") that does not define a class rpk: " + RpkFileName, RpkFileName, 0);
        }
      }
      return null;
    }
  }
}
