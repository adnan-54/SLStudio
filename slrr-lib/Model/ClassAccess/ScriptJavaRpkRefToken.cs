using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class ScriptJavaRpkRefToken : ScriptJavaToken
  {
    public ScriptJavaToken IDToken;
    public ScriptJavaToken RPKToken;

    public int ResID
    {
      get
      {
        return int.Parse(IDToken.Token.ToLower().Replace("0x", "").Replace("r", ""), System.Globalization.NumberStyles.HexNumber);
      }
      set
      {
        IDToken.Token = "0x" + value.ToString("X") + "r";
      }
    }
    public string RPKasSlrrRootRelativeFnam
    {
      get
      {
        return RPKToken.Token.Replace(".", "\\").Trim('\\','/') + ".rpk";
      }
      set
      {
        RPKToken.Token = value.Substring(0, value.Length - 4).Replace("\\", ".");
      }
    }

    public void GetBoundsAndTokenValueFromComponents()
    {
      Token = RPKToken.Token + ":" + IDToken.Token;
      StartInd = RPKToken.StartInd;
      EndInd = IDToken.EndInd;
    }
    public override string ToString()
    {
      return RPKToken.Token + ":" + IDToken.Token;
    }
  }
}
