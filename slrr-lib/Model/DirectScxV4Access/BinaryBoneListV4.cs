using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class BinaryBoneListV4 : FileEntry
  {
    private static readonly int typeDeferOffset = 0;
    private static readonly int sizeOffset = 4;
    private static readonly int unkownNullOffset = 8;

    public int NumIndices
    {
      get
      {
        return ((Size - (3 * 4)) / 4);
      }
    }
    public IEnumerable<int> Indices
    {
      get
      {
        List<int> ret = new List<int>();
        for(int i = 0; i != NumIndices; ++i)
        {
          ret.Add(GetIntFromFile((3 * 4) + i * 4));
        }
        return ret;
      }
    }
    public int Type
    {
      get
      {
        return GetIntFromFile(typeDeferOffset);
      }
      set
      {
        SetIntInFile(value, typeDeferOffset);
      }
    }
    public int UnkownNull
    {
      get
      {
        return GetIntFromFile(unkownNullOffset);
      }
      set
      {
        SetIntInFile(value, unkownNullOffset);
      }
    }
    public override int Size
    {
      get
      {
        return GetIntFromFile(sizeOffset,true);
      }
      set
      {
        SetIntInFile(value, sizeOffset);
      }
    }

    public BinaryBoneListV4(FileCacheHolder fileCache, int offset, bool cache = false)
    :base(fileCache,offset,cache)
    {

    }
  }
}
