using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class BinarySpatialNode : BinaryInnerRsdEntry
  {
    protected static readonly int signatureOffset = 0;
    protected static readonly int countOfNamedDataOffset = 4;
    protected static readonly int dataOffset = 8;

    private IEnumerable<BinaryNamedSpatialData> dataArray = null;

    public int Signature
    {
      get
      {
        return GetIntFromFile(signatureOffset);
      }
      set
      {
        SetIntInFile(value, signatureOffset);
      }
    }
    public int CountOfNamedData
    {
      get
      {
        return GetIntFromFile(countOfNamedDataOffset,true);
      }
      set
      {
        SetIntInFile(value, countOfNamedDataOffset);
      }
    }
    public IEnumerable<BinaryNamedSpatialData> DataArray
    {
      get
      {
        if (dataArray == null)
          dataArray = ReLoadDataArray();
        return dataArray;
      }
      set
      {
        dataArray = value;
      }
    }
    public override int Size
    {
      get
      {
        return (DataArray.Sum(x => x.Size)) + 8;
      }
      set
      {
        if (value != Size)
          throw new Exception("(DataArray.Sum(x.Size)) + 8");
      }
    }

    public BinarySpatialNode(FileCacheHolder file, int offset, bool cache = false)
    : base(0,file, offset, cache)
    {

    }

    public IEnumerable<BinaryNamedSpatialData> ReLoadDataArray()
    {
      List<BinaryNamedSpatialData> ret = new List<BinaryNamedSpatialData>();
      int currentOffset = dataOffset;
      for(int data_i = 0; data_i != CountOfNamedData; ++data_i)
      {
        var toad = new BinaryNamedSpatialData(Cache, Offset+currentOffset, Cache.IsDataCached);
        ret.Add(toad);
        currentOffset += toad.Size;
      }
      return ret;
    }
  }
}
