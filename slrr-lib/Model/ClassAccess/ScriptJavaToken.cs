using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class ScriptJavaToken
  {
    public int StartInd;
    public int EndInd;
    public string Token;

    public override string ToString()
    {
      return Token;
    }
  }
}
