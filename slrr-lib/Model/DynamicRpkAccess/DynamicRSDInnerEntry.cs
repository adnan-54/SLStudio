using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class DynamicRSDInnerEntry : DynamicRSDInnerEntryBase
  {
    public int OriginalOffset
    {
      get;
      private set;
    }
    public string StringData
    {
      get;
      set;
    }
    public string Signature
    {
      get;
      set;
    }

    public DynamicRSDInnerEntry()
    :this(null)
    {
      Signature = "RSD\0";
    }
    public DynamicRSDInnerEntry(BinaryRSDInnerEntry from = null)
    {
      if (from == null)
        return;
      Signature = from.Signature;
      StringData = from.StringData;
      OriginalOffset = from.Offset;
    }

    public List<DynamicLineNameValue> GetStringDataAsDict()
    {
      var spl = StringData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
      List<DynamicLineNameValue> ret = new List<DynamicLineNameValue>();
      foreach (var ln in spl)
      {
        string localLn = ln.Trim();
        int firstWhiteSpace = localLn.IndexOfAny(new char[] { ' ', '\t' });
        if(firstWhiteSpace != -1)
          ret.Add(new DynamicLineNameValue(localLn.Remove(firstWhiteSpace).Trim(), localLn.Substring(firstWhiteSpace).Trim()));
      }
      return ret;
    }
    public void SetStringDataAsDict(List<DynamicLineNameValue> dict)
    {
      StringBuilder sb = new StringBuilder();
      foreach (var kv in dict)
      {
        sb.AppendLine(kv.Key + " \t " + kv.Value);
      }
      StringData = sb.ToString();
    }
    public override int GetSize()
    {
      return 4 + 4 + StringData.TrimEnd('\r', '\n', '\0').Length + 3;
    }
    public override void Save(BinaryWriter bw)
    {
      string relSign = Signature;
      if (relSign.Length > 4)
        relSign = relSign.Substring(0, 4);
      while (relSign.Length < 4)
        relSign += " ";
      bw.Write(ASCIIEncoding.ASCII.GetBytes(relSign));
      bw.Write(GetSize() - 8);
      bw.Write(ASCIIEncoding.ASCII.GetBytes(StringData.TrimEnd('\r', '\n', '\0') + "\r\n\0"));
    }
  }
}
