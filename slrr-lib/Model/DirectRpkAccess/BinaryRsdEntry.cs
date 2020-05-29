using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class BinaryRsdEntry : FileEntry
  {
    private int size;
    private IEnumerable<BinaryInnerRsdEntry> innerEntries = null;

    public byte[] RSDData
    {
      get
      {
        byte[] ret = new byte[size];
        Array.Copy(Cache.GetFileData(), Offset, ret, 0, size);
        return ret;
      }
      set
      {
        LengthChangingReplace(value, 0, value.Length, 0, size);
        size = value.Length;
      }
    }
    public string RSDDataString
    {
      get
      {
        return ASCIIEncoding.ASCII.GetString(Cache.GetFileData(), Offset, size);
      }
      set
      {
        string toConvert = value;
        var bytes = ASCIIEncoding.ASCII.GetBytes(toConvert);
        LengthChangingReplace(bytes, 0, bytes.Length, 0, size);
        size = bytes.Length;
      }
    }
    public IEnumerable<BinaryInnerRsdEntry> InnerEntries
    {
      get
      {
        if (innerEntries == null)
          innerEntries = ReLoadInnerEntries();
        return innerEntries;
      }
      set
      {
        innerEntries = value;
      }
    }
    public override int Size
    {
      get
      {
        return size;
      }
      set
      {
        size = value;
      }
    }

    public BinaryRsdEntry(int Size, FileCacheHolder file, int offset, bool cache = false)
    :base(file,offset,cache)
    {
      this.size = Size;
    }

    public IEnumerable<BinaryInnerRsdEntry> ReLoadInnerEntries()
    {
      List<BinaryInnerRsdEntry> ret = new List<BinaryInnerRsdEntry>();
      int currOffset = 0;
      while (currOffset < Size)
      {
        var signature = GetFixLengthString(currOffset, 4);
        int signatureInt = GetIntFromFile(currOffset);
        if(signature == "RSD\0")
        {
          ret.Add(new BinaryRSDInnerEntry(Cache, Offset + currOffset));
        }
        else if(signature == "ICFG")
        {
          ret.Add(new BinaryICFGInnerEntry(Cache, Offset + currOffset));
        }
        else if (signature == "XCFG")
        {
          ret.Add(new BinaryXCFGInnerEntry(Cache, Offset + currOffset));
        }
        else if (signature == "EXTP")
        {
          ret.Add(new BinaryEXTPInnerEntry(Cache, Offset + currOffset));
        }
        else if (signatureInt == 0)
        {
          var datArr = new BinarySpatialNode(Cache, Offset + currOffset);
          ret.Add(datArr);
        }
        else
        {
          ret.Add(new BinaryStringInnerEntry(Size - currOffset, Cache, Offset + currOffset));
        }
        currOffset += ret.Last().Size;
      }
      if(currOffset != Size)
      {
        throw new Exception("Bad read of inner RSDs");
      }
      return ret;
    }
  }
}
