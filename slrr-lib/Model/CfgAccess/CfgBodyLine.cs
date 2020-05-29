using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace SlrrLib.Model
{
  public class CfgBodyLine : CfgPartPosRotLine
  {
    public float BodyWeight
    {
      get
      {
        if (PosLine.Tokens.Count > 7)
          return PosLine.Tokens[7].ValueAsFloat;
        return 0.0f;
      }
      set
      {
        PosLine.FixCountFromFormatDescriptor(8);
        PosLine.Tokens[7].ValueAsFloat = value;
      }
    }
    public string BodyModel
    {
      get
      {
        if (PosLine.Tokens.Count > 8)
          return PosLine.Tokens[8].Value;
        return "";
      }
      set
      {
        PosLine.FixCountFromFormatDescriptor(9);
        PosLine.Tokens[8].Value = value;
      }
    }

    public CfgBodyLine(CfgLine line)
    :base(line)
    {

    }

    public static CfgBodyLine GetZeroBodyLine()
    {
      return new CfgBodyLine(new CfgLine("body 0.0 0.0 0.0 0.000 0.000 0.000 0.0 none"));
    }
  }
}
