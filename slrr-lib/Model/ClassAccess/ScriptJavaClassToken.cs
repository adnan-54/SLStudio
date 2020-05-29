using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class ScriptJavaClassToken : ScriptJavaToken
  {
    public ScriptJavaClassNameToken ClassName;
    public ScriptJavaToken ClassExtendsName = null;
    public int Depth;
    public List<ScriptJavaRpkRefToken> RPKRefs = new List<ScriptJavaRpkRefToken>();

    public ScriptJavaClassToken(ScriptJavaToken from)
    {
      StartInd = from.StartInd;
      EndInd = from.EndInd;
      Token = from.Token;
    }

    public void ReplaceRpkName(string slrrRelativeRpkNameFrom,string slrrRelativeRpkNameTo)
    {
      foreach(var rpkRef in RPKRefs)
      {
        if (rpkRef.RPKasSlrrRootRelativeFnam.ToLower() == slrrRelativeRpkNameFrom.ToLower())
          rpkRef.RPKasSlrrRootRelativeFnam = slrrRelativeRpkNameTo;
      }
    }
    public override string ToString()
    {
      if (ClassExtendsName != null)
        return ClassName.Token + ":" + ClassExtendsName.Token;
      return ClassName.Token;
    }
  }
}
