using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace SlrrLib.Model
{
  public class CfgRenderLine : CfgPartPositionLine
  {
    public override float LineX
    {
      get
      {
        if (PosLine.Tokens.Count > 2)
          return PosLine.Tokens[2].ValueAsFloat;
        return 0.0f;
      }
      set
      {
        PosLine.FixCountFromFormatDescriptor(3);
        PosLine.Tokens[2].ValueAsFloat = value;
      }
    }
    public override float LineY
    {
      get
      {
        if (PosLine.Tokens.Count > 3)
          return PosLine.Tokens[3].ValueAsFloat;
        return 0.0f;
      }
      set
      {
        PosLine.FixCountFromFormatDescriptor(4);
        PosLine.Tokens[3].ValueAsFloat = value;
      }
    }
    public override float LineZ
    {
      get
      {
        if (PosLine.Tokens.Count > 4)
          return PosLine.Tokens[4].ValueAsFloat;
        return 0.0f;
      }
      set
      {
        PosLine.FixCountFromFormatDescriptor(5);
        PosLine.Tokens[4].ValueAsFloat = value;
      }
    }

    public CfgRenderLine(CfgLine line)
    :base(line)
    {

    }
    protected CfgRenderLine()
    :base()
    {

    }
  }
}
