using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class DynamicStringInnerEntry : DynamicRSDInnerEntryBase
  {
    public string StringData
    {
      get;
      set;
    }

    public DynamicStringInnerEntry()
    {

    }
    public DynamicStringInnerEntry(BinaryStringInnerEntry from = null)
    {
      if (from == null)
        return;
      StringData = from.StringData;
    }
    public DynamicStringInnerEntry(string data)
    {
      StringData = data;
    }

    public override int GetSize()
    {
      return StringData.Length;
    }
    public override void Save(BinaryWriter bw)
    {
      bw.Write(ASCIIEncoding.ASCII.GetBytes(StringData));
    }
  }
}
