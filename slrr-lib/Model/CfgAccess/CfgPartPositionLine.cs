using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace SlrrLib.Model
{
  public class CfgPartPositionLine
  {
    public CfgLine PosLine
    {
      get;
      protected set;
    }

    public virtual string LineName
    {
      get
      {
        if (PosLine.Tokens.Count > 0)
          return PosLine.Tokens[0].Value;
        return "";
      }
      set
      {
        PosLine.FixCountFromFormatDescriptor(1);
        PosLine.Tokens[0].Value = value;
      }
    }
    public virtual float LineX
    {
      get
      {
        if (PosLine.Tokens.Count > 1)
          return PosLine.Tokens[1].ValueAsFloat;
        return 0.0f;
      }
      set
      {
        PosLine.FixCountFromFormatDescriptor(2);
        PosLine.Tokens[1].ValueAsFloat = value;
      }
    }
    public virtual float LineY
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
    public virtual float LineZ
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

    public CfgPartPositionLine(CfgLine line)
    {
      PosLine = line;
    }
    protected CfgPartPositionLine()
    {
      PosLine = null;
    }

    public override string ToString()
    {
      return PosLine.ToString();
    }
  }
}
