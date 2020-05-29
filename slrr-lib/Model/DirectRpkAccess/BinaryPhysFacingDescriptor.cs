using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class BinaryPhysFacingDescriptor : FileEntry
  {
    protected static readonly int unkownInt1Offset = 0;
    protected static readonly int unkownInt2Offset = 4;
    protected static readonly int unkownInt3Offset = 8;
    protected static readonly int unkownInt4Offset = 12;
    protected static readonly int unkownFloat1Offset = 16;
    protected static readonly int unkownFloat2Offset = 20;
    protected static readonly int unkownFloat3Offset = 24;

    public int UnkownInt1
    {
      get
      {
        return GetIntFromFile(unkownInt1Offset);
      }
      set
      {
        SetIntInFile(value, unkownInt1Offset);
      }
    }
    public int TriIndex0//triangle index0
    {
      get
      {
        return GetIntFromFile(unkownInt2Offset);
      }
      set
      {
        SetIntInFile(value, unkownInt2Offset);
      }
    }
    public int TriIndex1//triangle index1
    {
      get
      {
        return GetIntFromFile(unkownInt3Offset);
      }
      set
      {
        SetIntInFile(value, unkownInt3Offset);
      }
    }
    public float NormalX//triangle normalX
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
    public float NormalY//triangle normalY
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
    public float NormalZ//triangle normalZ
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
    public int TriIndex2//triangle index2
    {
      get
      {
        return GetIntFromFile(unkownInt4Offset);
      }
      set
      {
        SetIntInFile(value, unkownInt4Offset);
      }
    }
    public override int Size
    {
      get
      {
        return 28;
      }
      set
      {
        throw new Exception("FirstChunkData size should always be 28");
      }
    }

    public BinaryPhysFacingDescriptor(FileCacheHolder file, int offset, bool cache = false)
    : base(file, offset, cache)
    {

    }
  }
}
