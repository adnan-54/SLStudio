using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class ScriptClass
  {
    private ScriptClassCONSChunk cls = null;
    private ScriptJavaClassToken java = null;

    public string ShortClassName
    {
      get
      {
        if (java != null)
          return java.ClassName.Token;
        return cls.ShortClassName;
      }
      set
      {
        if (java != null)
          java.ClassName.Token = value;
        if (cls != null)
          cls.ShortClassName = value;
      }
    }
    public string ShortSuperClassName
    {
      get
      {
        if (java != null)
          return java.ClassExtendsName.Token;
        return cls.ShortSuperClassName;
      }
    }

    public ScriptClass(ScriptClassCONSChunk cls,ScriptJavaClassToken java)
    {
      this.cls = cls;
      this.java = java;
    }

    public void ReplaceRpkName(string slrrRelativeRpkFnamFrom,string slrrRelativeRpkFnamTo)
    {
      if(java != null)
      {
        foreach(var token in java.RPKRefs)
        {
          if(token.RPKasSlrrRootRelativeFnam.ToLower() == slrrRelativeRpkFnamFrom.ToLower())
          {
            token.RPKasSlrrRootRelativeFnam = slrrRelativeRpkFnamTo;
          }
        }
      }
      if(cls != null)
      {
        foreach(var rpkRef in cls.RpkRefs)
        {
          if(rpkRef.RPKNameString.ToLower() == slrrRelativeRpkFnamFrom.ToLower())
          {
            rpkRef.RPKNameString = slrrRelativeRpkFnamTo;
            break;
          }
        }
      }
    }
    public bool ReplaceRpkReference(string slrrRelativeRpkFnamFrom, int typeIDFrom,string slrrRelativeRpkFnamTo, int typeIDTo)
    {
      bool ret = false;
      if (java != null)
      {
        foreach (var token in java.RPKRefs)
        {
          if (token.RPKasSlrrRootRelativeFnam.ToLower() == slrrRelativeRpkFnamFrom.ToLower() &&
              token.ResID == typeIDFrom)
          {
            token.RPKasSlrrRootRelativeFnam = slrrRelativeRpkFnamTo;
            token.ResID = typeIDTo;
            ret = true;
          }
        }
      }
      if (cls != null)
      {
        int indOfRpkName = cls.IndexOfStringEntryWithStr(slrrRelativeRpkFnamTo);
        foreach (var rpkRef in cls.RpkRefs)
        {
          if (rpkRef.RPKNameString.ToLower() == slrrRelativeRpkFnamFrom.ToLower() &&
              rpkRef.TypeIdInRPK == typeIDFrom)
          {
            rpkRef.TypeIdInRPK = typeIDTo;
            if (indOfRpkName == -1)
            {
              cls.CreateAndAddNameEntry(slrrRelativeRpkFnamTo);
              indOfRpkName = cls.IndexOfStringEntryWithStr(slrrRelativeRpkFnamTo);
            }
            rpkRef.RPKnameIndexInConstantTable = indOfRpkName;
            ret = true;
            break;
          }
        }
      }
      return ret;
    }
    public List<ScriptClassRpkRef> CollectRpkRefs()
    {
      List<ScriptClassRpkRef> ret = new List<ScriptClassRpkRef>();
      if(cls != null)
      {
        foreach(var rpkRef in cls.RpkRefs)
        {
          ret.Add(new ScriptClassRpkRef (rpkRef.RPKNameString, rpkRef.TypeIdInRPK));
        }
        return ret;
      }
      if(java != null)
      {
        foreach(var rpkRef in java.RPKRefs)
        {
          ret.Add(new ScriptClassRpkRef (rpkRef.RPKasSlrrRootRelativeFnam,rpkRef.ResID));
        }
        return ret.Distinct().ToList();
      }
      return ret;
    }
  }
}
