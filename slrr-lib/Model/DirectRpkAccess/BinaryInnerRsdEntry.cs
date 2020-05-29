using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class BinaryInnerRsdEntry : FileEntry
  {
    private int size;

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

    protected byte[] innerRSDData
    {
      get
      {
        byte[] ret = new byte[Size];
        Array.Copy(Cache.GetFileData(), Offset, ret, 0, Size);
        return ret;
      }
      set
      {
        LengthChangingReplace(value, 0, value.Length, 0, Size);
        Size = value.Length;
      }
    }
    protected string innerRSDDataString
    {
      get
      {
        return ASCIIEncoding.ASCII.GetString(Cache.GetFileData(), Offset, Size);
      }
      set
      {
        string toConvert = value;
        var bytes = ASCIIEncoding.ASCII.GetBytes(toConvert);
        LengthChangingReplace(bytes, 0, bytes.Length, 0, Size);
        Size = bytes.Length;
      }
    }

    public BinaryInnerRsdEntry(int Size, FileCacheHolder file, int offset, bool cache = false)
    :base(file,offset,cache)
    {
      size = Size;
    }
  }
}
