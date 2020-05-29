using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class BinaryHardSurfaceDefV4 : FileEntry
  {
    private static readonly int typeDeferOffset = 0;
    private static readonly int sizeOffset = 4;
    private static readonly int unkownNullOffset = 8;

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

    public BinaryHardSurfaceDefV4(FileCacheHolder fileCache, int offset, bool cache = false)
    :base(fileCache,offset,cache)
    {

    }
  }
}
