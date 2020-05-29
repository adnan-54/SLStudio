using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Media.Media3D;

namespace SlrrLib.Model.HighLevel
{
  public class GameFileManager
  {
    public static string[] BlackListWordsForFiles = new string[]
    {
      "BAK",
      "slrrHelp",
      "RpkReverse",
      "null.rpk",
      "ForModdersTrackPart",
      "abstract_textures",
      "SourcesOriginal"
    };

    private Dictionary<string, RpkManager> rpkDict = new Dictionary<string, RpkManager>();
    private Stack<CompatibleStackData> compatibleLookupStack = new Stack<CompatibleStackData>();

    public IEnumerable<RpkManager> Rpks
    {
      get
      {
        return rpkDict.Values;
      }
    }
    public string SlrrRoot
    {
      get;
      private set;
    }

    public GameFileManager(string slrrRoot)
    {
      SlrrRoot = slrrRoot;
    }

    public void ManualAddRPK(RpkManager rpk, string fnam)
    {
      rpkDict[fnam] = rpk;
    }
    public string GetSlrrRoot()
    {
      return SlrrRoot;
    }
    public void BuildRpkDict()
    {
      MessageLog.AddMessage("Building Rpk dictionary BlackList is: " + BlackListWordsForFiles.Aggregate((x, y) => x + ", " + y), "", 0);
      var allRpks = Directory.EnumerateFiles(SlrrRoot, "*.rpk", SearchOption.AllDirectories).ToList();
      var blackList = allRpks.Where(y => BlackListWordsForFiles.Any(x => y.Contains(x)));
      if(blackList.Any())
        MessageLog.AddMessage("Skipping because of blacklist (" + blackList.Aggregate((x, y) => x + ", " + y) + ")", "", 0);
      allRpks = allRpks.Except(blackList).ToList();
      for(int rpk_i = 0; rpk_i != allRpks.Count; ++rpk_i)
      {
        string fullPath = Path.GetFullPath(allRpks[rpk_i]).ToLower().Replace("\\\\","\\");
        MessageLog.AddMessage("Loading rpk(" + allRpks[rpk_i] + ") " + (rpk_i + 1).ToString() + "\\" + allRpks.Count.ToString(), "", 0);
        rpkDict[fullPath] = new RpkManager(fullPath, SlrrRoot);
      }
      MessageLog.AddMessage("Done Building Rpk dictionary", "", 0);
    }
    public BinaryResEntry LookUpResFromScriptReference(string scriptRef)
    {
      var spl = scriptRef.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
      if (spl.Length != 2)
        return null;
      int localTypeID = RpkManager.GetTypeIDFromString(spl[1].TrimEnd('r'));
      var rpk = GetRPKFromScriptTypeIDRef(scriptRef);
      return rpk.GetResEntry(localTypeID);
    }
    public RpkManager GetRPKOfRefedTypeIDIfTypeIDISValid(RpkManager refingRpk,int typeID)
    {
      var extRefStr = refingRpk.GetRpkExternalRefIndAsRPKRefString(RpkManager.TypeIDExternalRefPart(typeID));
      if (extRefStr == "")
        return null;
      var rpkRefed = GetRpkFromRefString(extRefStr);
      if (rpkRefed.GetResEntry(RpkManager.TypeIDExternalIDPart(typeID)) != null)
        return rpkRefed;
      return null;
    }
    public RpkManager GetRpkFromRefString(string refString)
    {
      return GetRpkFromWeakFileName(SlrrRoot + "\\" + refString);
    }
    public RpkManager GetRPKFromScriptTypeIDRef(string scriptTypeIDRef)
    {
      string refstr = scriptTypeIDRef.Remove(scriptTypeIDRef.IndexOf(':')).Replace(".","\\")+".rpk";
      return GetRpkFromRefString(refstr);
    }
    public RpkManager GetRpkFromWeakFileName(string weakName)
    {
      string fullPath = Path.GetFullPath(weakName).Replace("\\\\","\\").ToLower();
      RpkManager ret = null;
      if(rpkDict.TryGetValue(fullPath, out ret))
        return ret;
      if (rpkDict.TryGetValue(fullPath.Trim('\\')+".rpk", out ret))
        return ret;
      return null;
    }
    public void CheckSlotRefrenceValidityForRpkFnam(string rpkFnam)
    {
      CheckSlotReferenceVAliditiForRpk(GetRpkFromWeakFileName(rpkFnam));
    }
    public void CheckSlotReferenceVAliditiForRpk(RpkManager rpk)
    {
      if(rpk == null)
      {
        MessageLog.AddError("Check Validity for RPK called with null");
        return;
      }
      MessageLog.AddMessage("Checking SlotRefrences in " + rpk.RpkFileName, rpk.RpkFileName);
      foreach (var typeID in rpk.typeIDTOSlotToSlotCacheEntry.Values)
      {
        foreach (var slot in typeID.Values)
        {
          foreach (var attach in slot.AttachLines.Union(slot.CompatibleLines))
          {
            RpkManager refedRpk = null;
            int externalPart = RpkManager.TypeIDExternalRefPart(attach.TypeID);
            if (externalPart == 0)
              refedRpk = rpk;
            else
            {
              if ((externalPart - 1) >= rpk.Rpk.ExternalReferences.Count())
              {
                MessageLog.AddError("External typeID reference(0x" + attach.TypeID.ToString("X8") + ") referencing external rpk index not defined in rpk("
                                    + rpk.RpkFileName + ") referenced by cfg: " + rpk.GetCfgFromTypeID(slot.TypeIDOfSlotOwner).CfgFileName
                                    + " identified by TypeID: 0x" + slot.TypeIDOfSlotOwner.ToString("X8") + " at SlotID: " + slot.SlotID.ToString(), rpk.RpkFileName);
              }
              else
              {
                string refedrpkFullPath = (SlrrRoot + "\\" + rpk.Rpk.GetNthExternalReference(externalPart - 1).ReferenceString).Replace("\\\\", "\\").ToLower();
                if (!rpkDict.ContainsKey(refedrpkFullPath))
                {
                  MessageLog.AddError("Rpk path(" + refedrpkFullPath + ") not found but referenced in rpk(" + rpk.RpkFileName + ") as externaly referenced rpk with ind: " + externalPart.ToString(),
                                      rpk.RpkFileName);
                }
                else
                {
                  refedRpk = rpkDict[refedrpkFullPath];
                }
              }
            }
            if (refedRpk != null)
            {
              var referencedSlot = refedRpk.GetSlotFromTypeIDAndSlotID(RpkManager.TypeIDExternalIDPart(attach.TypeID), attach.SlotID, false);
              if (referencedSlot == null)
              {
                if (slot.AttachLines.Any(x => x.SlotID == attach.SlotID && x.TypeID == attach.TypeID))
                {
                  //MessageLog.AddMessage("No flat attach found checking indirect compatibles to undefined slots...");
                  bool foundMatchingCompatibleToUndefinedSlotID = false;
                  foreach (var innerRpk in Rpks)
                  {
                    int extrefInd = RpkManager.TypeIDExternalRefPart(attach.TypeID) - 1;
                    string extRefStr = "";
                    if (extrefInd == -1)
                    {
                      extRefStr = rpk.GetRpkAsRPKRefString();
                    }
                    else
                    {
                      extRefStr = rpk.Rpk.GetNthExternalReference(extrefInd).ReferenceString.ToLower();
                    }
                    int indexOfREfInCurrentRPK = innerRpk.GetIndexOfExternalRPKRefFromStringVal(extRefStr);
                    if (indexOfREfInCurrentRPK == -1)
                    {
                      if (innerRpk.GetRpkAsRPKRefString() != extRefStr)
                        continue;
                    }
                    indexOfREfInCurrentRPK++;
                    var toFind = new CfgSlotReference(RpkManager.TypeIDExternalIDPart(attach.TypeID) + (indexOfREfInCurrentRPK << 16), attach.SlotID);
                    //if (innerRpk.compatibleReferences.Keys.Any(x => x.slotID == toFind.slotID && x.typeID == toFind.typeID))
                    if (innerRpk.compatibleReferences.ContainsKey(toFind))
                    {
                      foundMatchingCompatibleToUndefinedSlotID = true;
                      /*MessageLog.AddMessage("Could resolve typeID-slotID pair: 0x" + attach.typeID.ToString("X8") +
                                            "-" + attach.slotID.ToString() + " in referencedRpk: " + refedRpk.rpkFileName
                                            + " referenced by cfg: " + rpk.GetCfgFromTypeID(slot.typeIDOfSlotOwner).CfgFileName
                                            + " identified by TypeID: 0x" + slot.typeIDOfSlotOwner.ToString("X8") + " at SlotID: " + slot.slotID.ToString()
                                            + " in rpk: " + rpk.rpkFileName + " with a compatible to an undefined SlotID or TypeID from rpk: " + innerRpk.rpkFileName
                                            + " from TypeID: 0x" + innerRpk.compatibleReferences[toFind].typeID.ToString("X8")
                                            + " from SlotID: " + innerRpk.compatibleReferences[toFind].slotID.ToString(), rpk.rpkFileName);*/
                      break;
                    }
                  }
                  if (foundMatchingCompatibleToUndefinedSlotID)
                    continue;
                }
                else
                {
                  /*refedRpk.GetSlotFromTypeIDAndSlotID(RpkManager.typeIDExternalIDPart(attach.typeID), attach.slotID,  true);
                  MessageLog.AddError("Compatible reference to Invalid typeID-slotID pair: 0x" + attach.typeID.ToString("X8") +
                                      "-" + attach.slotID.ToString() + " in referencedRpk: " + refedRpk.rpkFileName
                                      + " referenced by cfg: " + rpk.GetCfgFromTypeID(slot.typeIDOfSlotOwner).CfgFileName
                                      + " identified by TypeID: 0x" + slot.typeIDOfSlotOwner.ToString("X8") + " at SlotID: " + slot.slotID.ToString()
                                      + " in rpk: " + rpk.rpkFileName, rpk.rpkFileName);*/
                  continue;
                }
                refedRpk.GetSlotFromTypeIDAndSlotID(RpkManager.TypeIDExternalIDPart(attach.TypeID), attach.SlotID, true);
                MessageLog.AddError("Could not resolve typeID-slotID pair: 0x" + attach.TypeID.ToString("X8") +
                                    "-" + attach.SlotID.ToString() + " in referencedRpk: " + refedRpk.RpkFileName
                                    + " referenced by cfg: " + rpk.GetCfgFromTypeID(slot.TypeIDOfSlotOwner).CfgFileName
                                    + " identified by TypeID: 0x" + slot.TypeIDOfSlotOwner.ToString("X8") + " at SlotID: " + slot.SlotID.ToString()
                                    + " in rpk: " + rpk.RpkFileName, rpk.RpkFileName);
              }
            }
          }
        }
      }
    }
    public void CheckSlotReferenceValidity()
    {
      foreach(var rpk in Rpks)
      {
        CheckSlotReferenceVAliditiForRpk(rpk);
      }
    }
    public HashSet<RpkSlotReference> GetAttachableReferencesForSlot(RpkManager rpk, int typeID, int slotID)
    {
      return getAllAttachableReferencesForSlot(rpk, typeID, slotID, true);
    }
    public HashSet<RpkSlotReference> GetLocalAttachableReferencesForSlot(RpkManager rpk, int typeID, int slotID)
    {
      return getAllLocalAttachableReferencesForSlot(rpk, typeID, slotID, true);
    }
    public RpkSCXFile GetFullSCXPathFromTypeByteResEntryWithLineKeySCXLine(BinaryResEntry res, RpkManager extRpk,byte typeByte,string lineKey)
    {
      if (res.TypeOfEntry == typeByte)//direct scx ref in rsd
      {
        if (res.RSD.InnerEntries.Count() != 1)
        {
          MessageLog.AddError("Type "+typeByte.ToString()+" Res Entry in rpk: " + extRpk.RpkFileName + " non standard RSD entry: not 1 inner RSD entry defined");
          return null;
        }
        if (!(res.RSD.InnerEntries.First() is BinaryStringInnerEntry))
        {
          MessageLog.AddError("Type " + typeByte.ToString() + " Res Entry in rpk: " + extRpk.RpkFileName + " non standard RSD entry: the 1 defined Inner Entry is not a StringEntry");
          return null;
        }
        var strEntry = (res.RSD.InnerEntries.First() as BinaryStringInnerEntry);
        var spl = strEntry.StringData.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        bool found = false;
        foreach (var ln in spl)
        {
          var lnspl = ln.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
          if (lnspl.Length < 2)
          {
            continue;
          }
          if (lnspl[0].ToLower() == lineKey.ToLower())
          {
            string fullPath = SlrrRoot + "\\" + lnspl[1];
            if (!File.Exists(fullPath))
            {
              MessageLog.AddError("Type " + typeByte.ToString() + " Res Entry in rpk: " + extRpk.RpkFileName + " defined StringInnerEntry does not define a existing " + lineKey + " SCX file: " +
                                  fullPath);
              return null;
            }
            else
            {
              if (!extRpk.IsFullFnamProbableForLocalPath(fullPath))
              {
                MessageLog.AddError("Type " + typeByte.ToString() + " Res Entry in rpk: " + extRpk.RpkFileName + " defined StringInnerEntry does not define a probable " + lineKey + " SCX file: " + fullPath
                                    +" but it does exist");
              }
              return new RpkSCXFile(extRpk,fullPath);
            }
          }
        }
        if (!found)
        {
          MessageLog.AddError("Type " + typeByte.ToString() + " Res Entry in rpk: " + extRpk.RpkFileName + " defined StringInnerEntry does not define a "+lineKey+" line: \n" + strEntry.StringData);
          return null;
        }
      }
      return null;
    }
    public HashSet<RpkSCXFile> GetAllFullFnamOfUsedSCXsInCfg(RpkManager rpk, int typeID)
    {
      HashSet<RpkSCXFile> ret = new HashSet<RpkSCXFile>();
      var cfg = rpk.GetCfgFromTypeID(typeID);
      if (cfg != null)
      {
        //from refed TypeIDs
        foreach (var typeIDref in cfg.ReferencedRpkTypeIDs)//return distinct
        {
          var extRpk = GetRpkFromRefString(rpk.GetRpkExternalRefIndAsRPKRefString(RpkManager.TypeIDExternalRefPart(typeIDref)));
          if(extRpk != null)
          {
            var res = extRpk.GetResEntry(RpkManager.TypeIDExternalIDPart(typeIDref));
            if(res != null)
            {
              if(res.TypeOfEntry == 9)//direct scx ref in rsd
              {
                var toad = GetFullSCXPathFromTypeByteResEntryWithLineKeySCXLine(res, extRpk, 9, "shape");
                if(toad != null)
                {
                  ret.Add(toad);
                }
              }
              else if (res.TypeOfEntry == 5)//direct scx ref in rsd
              {
                var toad = GetFullSCXPathFromTypeByteResEntryWithLineKeySCXLine(res, extRpk, 5, "sourcefile");
                if (toad != null)
                {
                  ret.Add(toad);
                }
              }
              else if(res.TypeOfEntry == 14)//mesh descriptor
              {
                var strEntry = stringEntryFromResEntry(res);
                if(strEntry == null)
                {
                  MessageLog.AddError("Type 14 RES entry with typeID: 0x" + res.TypeID.ToString("X8") + " in rpk: " + extRpk.RpkFileName + " does not define a StringInnerRSD entry");
                  continue;
                }
                var meshTypeID = extractValueWithKeyFromStringRSDEntry(strEntry, "mesh").ToLower();
                int convertedTypeID = -1;
                if (!(meshTypeID.StartsWith("0x") && meshTypeID.Length > 2 && int.TryParse(meshTypeID.Substring(2),
                      System.Globalization.NumberStyles.HexNumber,
                      System.Globalization.CultureInfo.InvariantCulture,
                      out convertedTypeID)))
                {
                  MessageLog.AddError("Type 14 RES entry with typeID: 0x" + res.TypeID.ToString("X8") + " in rpk: " + extRpk.RpkFileName + " defines invalid TypeID as mesh reference: "+meshTypeID);
                  continue;
                }
                var extExtRpk = GetRpkFromRefString(extRpk.GetRpkExternalRefIndAsRPKRefString(RpkManager.TypeIDExternalRefPart(convertedTypeID)));
                if(extExtRpk != null)
                {
                  var extRes = extExtRpk.GetResEntry(RpkManager.TypeIDExternalIDPart(convertedTypeID));
                  if(extRes != null)
                  {
                    if(extRes.TypeOfEntry != 5)
                    {
                      MessageLog.AddError("Type 14 RES entry with typeID: 0x" + res.TypeID.ToString("X8") + " in rpk: " + extRpk.RpkFileName + " defines TypeID as mesh reference: " + meshTypeID
                                          + " that is not a Type 5 RES enrty in rpk: " + extExtRpk.RpkFileName);
                      continue;
                    }
                    var toad = GetFullSCXPathFromTypeByteResEntryWithLineKeySCXLine(extRes, extExtRpk, 5, "sourcefile");
                    if (toad != null)
                    {
                      ret.Add(toad);
                    }
                  }
                }
              }
            }
          }
        }
        //from body lines
        foreach(var bodyLn in cfg.BodyLines)
        {
          if(bodyLn.BodyModel.ToLower().EndsWith(".scx"))
          {
            string fullName = SlrrRoot + "\\" + bodyLn.BodyModel;
            if(!File.Exists(fullName))
            {
              MessageLog.AddError("Body line(" + bodyLn.ToString() + ") of cfg: " + cfg.CfgFileName + " from rpk: "
                                  + rpk.RpkFileName + " defined at TypeID: 0x" + typeID.ToString("X8") +
                                  "does not define existing SCX file: " + fullName + " with SlrrRoot: " + SlrrRoot);
              continue;
            }
            if (!rpk.IsFullFnamProbableForLocalPath(fullName))
            {
              MessageLog.AddError("Body line(" + bodyLn.ToString() + ") of cfg: " + cfg.CfgFileName + " from rpk: "
                                  + rpk.RpkFileName + " defined at TypeID: 0x" + typeID.ToString("X8") +
                                  "does not define a probable SCX file: " + fullName + " with SlrrRoot: " + SlrrRoot +" but it does exist");
            }
            ret.Add(new RpkSCXFile(rpk,fullName));
          }
        }
      }
      return ret;
    }
    public RpkSCXFile GetFullSXCFnamOfTypeID(RpkManager rpk, int typeIDref)
    {
      var extRpk = GetRpkFromRefString(rpk.GetRpkExternalRefIndAsRPKRefString(RpkManager.TypeIDExternalRefPart(typeIDref)));
      if (extRpk != null)
      {
        var res = extRpk.GetResEntry(RpkManager.TypeIDExternalIDPart(typeIDref));
        if (res != null)
        {
          if (res.TypeOfEntry == 9)//direct scx ref in rsd
          {
            var toad = GetFullSCXPathFromTypeByteResEntryWithLineKeySCXLine(res, extRpk, 9, "shape");
            if (toad != null)
            {
              return (toad);
            }
          }
          else if (res.TypeOfEntry == 5)//direct scx ref in rsd
          {
            var toad = GetFullSCXPathFromTypeByteResEntryWithLineKeySCXLine(res, extRpk, 5, "sourcefile");
            if (toad != null)
            {
              return (toad);
            }
          }
          else if (res.TypeOfEntry == 14)//mesh descriptor
          {
            var strEntry = stringEntryFromResEntry(res);
            if (strEntry == null)
            {
              MessageLog.AddError("Type 14 RES entry with typeID: 0x" + res.TypeID.ToString("X8") + " in rpk: " + extRpk.RpkFileName + " does not define a StringInnerRSD entry");
              return null;
            }
            var meshTypeID = extractValueWithKeyFromStringRSDEntry(strEntry, "mesh").ToLower();
            int convertedTypeID = -1;
            if (!(meshTypeID.StartsWith("0x") && meshTypeID.Length > 2 && int.TryParse(meshTypeID.Substring(2),
                  System.Globalization.NumberStyles.HexNumber,
                  System.Globalization.CultureInfo.InvariantCulture,
                  out convertedTypeID)))
            {
              MessageLog.AddError("Type 14 RES entry with typeID: 0x" + res.TypeID.ToString("X8") + " in rpk: " + extRpk.RpkFileName + " defines invalid TypeID as mesh reference: " + meshTypeID);
              return null;
            }
            var extExtRpk = GetRpkFromRefString(extRpk.GetRpkExternalRefIndAsRPKRefString(RpkManager.TypeIDExternalRefPart(convertedTypeID)));
            if (extExtRpk != null)
            {
              var extRes = extExtRpk.GetResEntry(RpkManager.TypeIDExternalIDPart(convertedTypeID));
              if (extRes != null)
              {
                if (extRes.TypeOfEntry != 5)
                {
                  MessageLog.AddError("Type 14 RES entry with typeID: 0x" + res.TypeID.ToString("X8") + " in rpk: " + extRpk.RpkFileName + " defines TypeID as mesh reference: " + meshTypeID
                                      + " that is not a Type 5 RES enrty in rpk: " + extExtRpk.RpkFileName);
                  return null;
                }
                var toad = GetFullSCXPathFromTypeByteResEntryWithLineKeySCXLine(extRes, extExtRpk, 5, "sourcefile");
                if (toad != null)
                {
                  return (toad);
                }
              }
            }
          }
        }
      }
      return null;
    }

    public static bool IsPathSLRRRoot(string path)
    {
      try
      {
        return (File.Exists(path + "\\cars.rpk") &&
                File.Exists(path + "\\maps.rpk") &&
                File.Exists(path + "\\frontend.rpk"));
      }
      catch (Exception)
      { }
      return false;
    }
    public static string GetSLRRRoot(string path)
    {
      string ret = path;
      while (!IsPathSLRRRoot(ret))
      {
        if (!ret.Contains('\\'))
          return "";
        ret = ret.Remove(ret.LastIndexOf('\\'));
      }
      return ret;
    }
    public static string GetPathAsSlrrRootRelative(string dirInSlrrStructure, string toConvert)
    {
      string fullSlrr = Path.GetFullPath(GetSLRRRoot(dirInSlrrStructure)).ToLower();
      string fullConv = Path.GetFullPath(toConvert).ToLower();
      return fullConv.Remove(0, fullSlrr.Length);
    }

    private BinaryStringInnerEntry stringEntryFromResEntry(BinaryResEntry res)
    {
      return (res.RSD.InnerEntries.FirstOrDefault() as BinaryStringInnerEntry);
    }
    private string extractValueWithKeyFromStringRSDEntry(BinaryStringInnerEntry strEntry,string lineKey)
    {
      var spl = strEntry.StringData.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
      foreach (var ln in spl)
      {
        var lnspl = ln.Split(new string[] { " ", "\t" }, StringSplitOptions.RemoveEmptyEntries);
        if (lnspl.Length < 2)
        {
          continue;
        }
        if (lnspl[0].ToLower() == lineKey.ToLower())
        {
          return lnspl[1];
        }
      }
      return "";
    }
    private HashSet<RpkSlotReference> getAllAttachableReferencesForSlot(RpkManager rpk, int typeID, int slotID,bool clearStack = false)
    {
      if (clearStack)
        compatibleLookupStack = new Stack<CompatibleStackData>();
      var ret = new HashSet<RpkSlotReference>();
      if(compatibleLookupStack.Any(x => x.rpk == rpk && x.slotID == slotID && x.typeID == typeID))
      {
        MessageLog.AddError("Flat-compatible loop detected chain: " + compatibleLookupStack.Select(x => Path.GetFileNameWithoutExtension(x.rpk.RpkFileName)
                            + "|0x" + x.typeID.ToString("X8") + ":" + x.slotID.ToString()).Aggregate((x, y) => x + "-" + y)+"-"
                            + Path.GetFileNameWithoutExtension(rpk.RpkFileName)
                            + "|0x" + typeID.ToString("X8") + ":" + slotID.ToString());
        return ret;
      }
      compatibleLookupStack.Push(new CompatibleStackData { rpk = rpk, slotID = slotID, typeID = typeID });
      var slotCache = rpk.SlotCacheFromTypeIDSlotID(typeID, slotID);
      //flat attaches
      if (slotCache != null)
      {
        ret.UnionWith(slotCache.AttachLines.Select(x => new RpkSlotReference(rpk,x.TypeID,x.SlotID)));
      }
      string asReferenceString = rpk.GetRpkExternalRefIndAsRPKRefString(RpkManager.TypeIDExternalRefPart(typeID));
      if (asReferenceString != "")
      {
        //indirect attaches
        foreach(var innerRpk in Rpks)
        {
          int asLocalTypeID = innerRpk.GetTypeIDInLocalExternalRefSpace(asReferenceString, typeID);
          if (asLocalTypeID != -1)
          {
            ret.UnionWith(innerRpk.GetFlatAttachSourcesForAttachingLocalRefSpaceTypeIDSlotID(asLocalTypeID, slotID)
                          .Select(x => new RpkSlotReference(innerRpk, x.TypeID, x.SlotID)));
          }
        }
        //any compatibles to current attahces
        var toCheckCompatiblityWith = new HashSet<RpkSlotReference>(ret);
        bool first = true;
        while (toCheckCompatiblityWith.Any())
        {
          var toCheckCompatiblityWithNext = new HashSet<RpkSlotReference>();
          foreach (var innerRpk in Rpks)
          {
            foreach (var innerSlotReference in toCheckCompatiblityWith)
            {
              string innerAsReferenceString = innerSlotReference.Rpk.GetRpkExternalRefIndAsRPKRefString(
                                                RpkManager.TypeIDExternalRefPart(innerSlotReference.SlotRef.TypeID));
              int asLocalTypeID = innerRpk.GetTypeIDInLocalExternalRefSpace(innerAsReferenceString, innerSlotReference.SlotRef.TypeID);
              if (asLocalTypeID != -1)
              {
                toCheckCompatiblityWithNext.UnionWith(innerRpk.GetFlatCompatibleSourcesForCompatibleToLocalRefSpaceTypeIDSlotID(asLocalTypeID, innerSlotReference.SlotRef.SlotID)
                                                      .Select(x => new RpkSlotReference(innerRpk, x.TypeID, x.SlotID)));
              }
            }
          }
          if (!first)
            ret.UnionWith(toCheckCompatiblityWith);
          first = false;
          if (ret.Overlaps(toCheckCompatiblityWithNext))
          {
            toCheckCompatiblityWithNext.ExceptWith(ret);
          }
          toCheckCompatiblityWith = toCheckCompatiblityWithNext;
        }
      }
      //flat compatibles
      if (slotCache != null)
      {
        foreach(var compatible in slotCache.CompatibleLines)
        {
          if (RpkManager.TypeIDExternalRefPart(compatible.TypeID) == 0)
          {
            ret.UnionWith(getAllAttachableReferencesForSlot(rpk, compatible.TypeID, compatible.SlotID));
          }
          else
          {
            string compatibleReferenceString = rpk.GetRpkExternalRefIndAsRPKRefString(RpkManager.TypeIDExternalRefPart(compatible.TypeID));
            if(compatibleReferenceString == "")
            {
              MessageLog.AddError("External ref ind of compatible reference to TypeID:0x" + compatible.TypeID.ToString("X8")
                                  + " is not defined as an external rpk reference in rpk: " + rpk.RpkFileName + " but used in cfg: "
                                  + rpk.GetCfgFnamOrEmptyStringFromTypeID(slotCache.TypeIDOfSlotOwner) + " defined by typeID: 0x"
                                  + slotCache.TypeIDOfSlotOwner.ToString("X8") + " compatible is at slotID: " + slotCache.SlotID.ToString());
            }
            else
            {
              var extRepk = GetRpkFromRefString(compatibleReferenceString);
              if(extRepk == null)
              {
                MessageLog.AddError("External ref ind of compatible reference to TypeID:0x" + compatible.TypeID.ToString("X8")
                                    + " is defined as an external rpk with RefString: "+compatibleReferenceString+" reference in rpk: " + rpk.RpkFileName + " used in cfg: "
                                    + rpk.GetCfgFnamOrEmptyStringFromTypeID(slotCache.TypeIDOfSlotOwner) + " defined by typeID: 0x"
                                    + slotCache.TypeIDOfSlotOwner.ToString("X8") + " compatible is at slotID: " + slotCache.SlotID.ToString()+
                                    " but file is not found with slrrRoot: "+SlrrRoot);
              }
              else
              {
                ret.UnionWith(getAllAttachableReferencesForSlot(extRepk, RpkManager.TypeIDExternalIDPart(compatible.TypeID), compatible.SlotID));
              }
            }
          }
        }
      }
      compatibleLookupStack.Pop();
      return ret;
    }
    private HashSet<RpkSlotReference> getAllLocalAttachableReferencesForSlot(RpkManager rpk, int typeID, int slotID, bool clearStack = false)
    {
      if (clearStack)
        compatibleLookupStack = new Stack<CompatibleStackData>();
      var ret = new HashSet<RpkSlotReference>();
      if (compatibleLookupStack.Any(x => x.rpk == rpk && x.slotID == slotID && x.typeID == typeID))
      {
        MessageLog.AddError("Flat-compatible loop detected chain: " + compatibleLookupStack.Select(x => Path.GetFileNameWithoutExtension(x.rpk.RpkFileName)
                            + "|0x" + x.typeID.ToString("X8") + ":" + x.slotID.ToString()).Aggregate((x, y) => x + "-" + y) + "-"
                            + Path.GetFileNameWithoutExtension(rpk.RpkFileName)
                            + "|0x" + typeID.ToString("X8") + ":" + slotID.ToString());
        return ret;
      }
      compatibleLookupStack.Push(new CompatibleStackData { rpk = rpk, slotID = slotID, typeID = typeID });
      var slotCache = rpk.SlotCacheFromTypeIDSlotID(typeID, slotID);
      //flat attaches
      if (slotCache != null)
      {
        ret.UnionWith(slotCache.AttachLines.Select(x => new RpkSlotReference(rpk, x.TypeID, x.SlotID)));
      }
      else if(clearStack)
      {
        MessageLog.AddError("Slot cahce is null with base entry 0x" + typeID.ToString("X8") + " in rpk: " + rpk.RpkFileName);
      }
      //indirect attaches
      if(RpkManager.TypeIDExternalRefPart(typeID) == 0)
      {
        ret.UnionWith(rpk.GetFlatAttachSourcesForAttachingLocalRefSpaceTypeIDSlotID(typeID, slotID)
                      .Select(x => new RpkSlotReference(rpk, x.TypeID, x.SlotID)));
        var toCheckCompatiblityWith = new HashSet<RpkSlotReference>(ret);
        bool first = true;
        while (toCheckCompatiblityWith.Any())
        {
          var toCheckCompatiblityWithNext = new HashSet<RpkSlotReference>();
          foreach (var innerSlotReference in toCheckCompatiblityWith)
          {
            toCheckCompatiblityWithNext.UnionWith(rpk.GetFlatCompatibleSourcesForCompatibleToLocalRefSpaceTypeIDSlotID(innerSlotReference.SlotRef.TypeID, innerSlotReference.SlotRef.SlotID)
                                                  .Select(x => new RpkSlotReference(rpk, x.TypeID, x.SlotID)));
          }
          if (!first)
            ret.UnionWith(toCheckCompatiblityWith);
          first = false;
          if (ret.Overlaps(toCheckCompatiblityWithNext))
          {
            toCheckCompatiblityWithNext.ExceptWith(ret);
          }
          toCheckCompatiblityWith = toCheckCompatiblityWithNext;
        }
      }
      //flat compatibles
      if (slotCache != null)
      {
        foreach (var compatible in slotCache.CompatibleLines)
        {
          if (RpkManager.TypeIDExternalRefPart(compatible.TypeID) == 0)
          {
            ret.UnionWith(getAllLocalAttachableReferencesForSlot(rpk, compatible.TypeID, compatible.SlotID));
          }
        }
      }
      compatibleLookupStack.Pop();
      return ret;
    }

    internal class CompatibleStackData
    {
      public RpkManager rpk;
      public int typeID;
      public int slotID;
    }
  }
}
