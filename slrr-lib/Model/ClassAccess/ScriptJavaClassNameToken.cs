using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class ScriptJavaClassNameToken : ScriptJavaToken
  {
    public List<ScriptJavaToken> othersWithSameStringValue = new List<ScriptJavaToken>();
    public ScriptJavaClassNameToken(ScriptJavaToken cpy)
    {
      Token = cpy.Token;
      StartInd = cpy.StartInd;
      EndInd = cpy.EndInd;
    }
    public void PropagateClassNameChange()
    {
      foreach (var other in othersWithSameStringValue)
        other.Token = Token;
    }
  }
}
