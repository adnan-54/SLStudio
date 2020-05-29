using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class ScriptClassRpkRef
  {
    public string SlrrRelativeRpkName
    {
      get;
      private set;
    }
    public int TypeIdInRpk
    {
      get;
      private set;
    }

    public ScriptClassRpkRef(string rpkName,int typeID)
    {
      SlrrRelativeRpkName = rpkName;
      TypeIdInRpk = typeID;
    }
  }
}
