using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class BinaryPhysVertex : FileEntry
  {
    protected static readonly int unkownFloat1Offset = 0;
    protected static readonly int unkownFloat2Offset = 4;
    protected static readonly int unkownFloat3Offset = 8;

    public float VertexX
    {
      get
      {
        return GetFloatFromFile(unkownFloat1Offset);
      }
      set
      {
        SetFloatInFile(value, unkownFloat1Offset);
      }
    }
    public float VertexY
    {
      get
      {
        return GetFloatFromFile(unkownFloat2Offset);
      }
      set
      {
        SetFloatInFile(value, unkownFloat2Offset);
      }
    }
    public float VertexZ
    {
      get
      {
        return GetFloatFromFile(unkownFloat3Offset);
      }
      set
      {
        SetFloatInFile(value, unkownFloat3Offset);
      }
    }
    public override int Size
    {
      get
      {
        return 12;
      }
      set
      {
        throw new Exception("SecondChunkData size should always be 12");
      }
    }

    public BinaryPhysVertex(FileCacheHolder file, int offset, bool cache = false)
    : base(file, offset, cache)
    {

    }
  }
}
