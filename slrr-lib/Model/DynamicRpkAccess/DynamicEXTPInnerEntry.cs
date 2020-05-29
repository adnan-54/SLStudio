using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class DynamicEXTPInnerEntry : DynamicRSDInnerEntryBase
  {
    public readonly static string Signature = "EXTP";

    public int EXTPint
    {
      get;
      set;
    }

    public DynamicEXTPInnerEntry()
    :this(null)
    {

    }
    public DynamicEXTPInnerEntry(BinaryEXTPInnerEntry from = null)
    {
      if (from == null)
        return;
      EXTPint = from.EXTPint;
    }

    public override int GetSize()
    {
      return 4 + 4 + 4;
    }
    public override void Save(BinaryWriter bw)
    {
      bw.Write(ASCIIEncoding.ASCII.GetBytes(Signature));
      bw.Write(GetSize() - 8);
      bw.Write(EXTPint);
    }
  }
}
