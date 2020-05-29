using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public enum BinaryScxV4VertexDataFlag
  {
    Position = 0x1,
    BoneWeightNumIs0 = 0x2,
    BoneWeightNumIs1 = 0x4,
    BoneWeightNumIs2 = 0x8,
    BoneWeightNumIs3 = 0x10,
    BoneIndRef = 0x20,
    Normal = 0x40,
    VertexIllumination = 0x80,
    VertexColor = 0x100,
    UV1 = 0x200,
    UV2 = 0x400,
    UV3 = 0x800,
    BumpMapNormal = 0x40000,
    Zero = 0
  }

  public class BinaryVertexV4 : FileEntry
  {
    private static readonly int typeOffset = 0; //this will be 4
    private static readonly int sizeOffset = 4;
    private static readonly int numberOfVerticesOffset = 8;//will be indexed with shorts in facedef so it cant be > max_short+1
    private static readonly int vertexTypeOffset = 12;
    private static readonly int vertexListOffset = 16;

    private IEnumerable<BasicXYZ> bumpNormals_cache = null;
    private IEnumerable<BasicUV> uv3s_cache = null;
    private IEnumerable<BasicUV> uv2s_cache = null;
    private IEnumerable<BasicUV> uv1s_cache = null;
    private IEnumerable<BasicRGBA> vertexColors_cache = null;
    private IEnumerable<BasicRGBA> vertexIlluminations_cache = null;
    private IEnumerable<BasicXYZ> normals_cache = null;
    private IEnumerable<BinaryBoneIndRefV4> boneIndRefs_cache = null;
    private IEnumerable<BinaryBoneWeightsV4> boneWeightLists_cache = null;
    private IEnumerable<BasicXYZ> positions_cache = null;
    private IEnumerable<BinaryCompleteVertexDataV4> fullVertexDataList_cache = null;

    public override int Size
    {
      get
      {
        return GetIntFromFile(sizeOffset, true);
      }
      set
      {
        SetIntInFile(value, sizeOffset);
      }
    }
    public int NumberOfVertices
    {
      get
      {
        return GetIntFromFile(numberOfVerticesOffset);
      }
      set
      {
        SetIntInFile(value, numberOfVerticesOffset);
      }
    }
    public int Type
    {
      get
      {
        return GetIntFromFile(typeOffset);
      }
      set
      {
        SetIntInFile(value, typeOffset);
      }
    }
    public int VertexType
    {
      get
      {
        return GetIntFromFile(vertexTypeOffset);
      }
      set
      {
        SetIntInFile(value, vertexTypeOffset);
      }
    }
    public bool PotentialOverFlow
    {
      get
      {
        return NumberOfVertices > ushort.MaxValue;
      }
    }
    public int OneVertexSize
    {
      get
      {
        return (Size - 16) / NumberOfVertices;
      }
    }
    public bool IsPositionDefined
    {
      get
      {
        return vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.Position) != -1;
      }
    }
    public bool IsBoneWeightNumIs0Defined
    {
      get
      {
        return vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.BoneWeightNumIs0) != -1;
      }
    }
    public bool IsBoneWeightNumIs1Defined
    {
      get
      {
        return vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.BoneWeightNumIs1) != -1;
      }
    }
    public bool IsBoneWeightNumIs2Defined
    {
      get
      {
        return vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.BoneWeightNumIs2) != -1;
      }
    }
    public bool IsBoneWeightNumIs3Defined
    {
      get
      {
        return vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.BoneWeightNumIs3) != -1;
      }
    }
    public bool IsBoneIndRefDefined
    {
      get
      {
        return vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.BoneIndRef) != -1;
      }
    }
    public bool IsNormalDefined
    {
      get
      {
        return vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.Normal) != -1;
      }
    }
    public bool IsVertexIlluminationDefined
    {
      get
      {
        return vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.VertexIllumination) != -1;
      }
    }
    public bool IsVertexColorDefined
    {
      get
      {
        return vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.VertexColor) != -1;
      }
    }
    public bool IsUV1Defined
    {
      get
      {
        return vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.UV1) != -1;
      }
    }
    public bool IsUV2Defined
    {
      get
      {
        return vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.UV2) != -1;
      }
    }
    public bool IsUV3Defined
    {
      get
      {
        return vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.UV3) != -1;
      }
    }
    public bool IsBumpMapNormalDefined
    {
      get
      {
        return vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.BumpMapNormal) != -1;
      }
    }
    public IEnumerable<BasicXYZ> BumpNormals
    {
      get
      {
        if (bumpNormals_cache == null)
          bumpNormals_cache = LazyLoadBumpNormals();
        return bumpNormals_cache;
      }
    }
    public IEnumerable<BasicUV> UV3s
    {
      get
      {
        if(uv3s_cache == null)
          uv3s_cache = LazyLoadUV3s();
        return uv3s_cache;
      }
    }
    public IEnumerable<BasicUV> UV2s
    {
      get
      {
        if (uv2s_cache == null)
          uv2s_cache = LazyLoadUV2s();
        return uv2s_cache;
      }
    }
    public IEnumerable<BasicUV> UV1s
    {
      get
      {
        if (uv1s_cache == null)
          uv1s_cache = LazyLoadUV1s();
        return uv1s_cache;
      }
    }
    public IEnumerable<BasicRGBA> VertexColors
    {
      get
      {
        if (vertexColors_cache == null)
          vertexColors_cache = LazyLoadVertexColors();
        return vertexColors_cache;
      }
    }
    public IEnumerable<BasicRGBA> VertexIlluminations
    {
      get
      {
        if (vertexIlluminations_cache == null)
          vertexIlluminations_cache = LazyLoadVertexIlluminations();
        return vertexIlluminations_cache;
      }
    }
    public IEnumerable<BasicXYZ> Normals
    {
      get
      {
        if (normals_cache == null)
          normals_cache = LazyLoadNormals();
        return normals_cache;
      }
    }
    public IEnumerable<BinaryBoneIndRefV4> BoneIndRefs
    {
      get
      {
        if (boneIndRefs_cache == null)
          boneIndRefs_cache = LazyLoadBoneIndRefs();
        return boneIndRefs_cache;
      }
    }
    public IEnumerable<BinaryBoneWeightsV4> BoneWeightLists
    {
      get
      {
        if (boneWeightLists_cache == null)
          boneWeightLists_cache = LazyLoadBoneWeightLists();
        return boneWeightLists_cache;
      }
    }
    public IEnumerable<BasicXYZ> Positions
    {
      get
      {
        if (positions_cache == null)
          positions_cache = LazyLoadPositions();
        return positions_cache;
      }
    }
    public IEnumerable<BinaryCompleteVertexDataV4> FullVertexDataList
    {
      get
      {
        if (fullVertexDataList_cache == null)
        {
          List<int> ret = new List<int>();
          for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
          {
            ret.Add(def_i);
          }
          fullVertexDataList_cache = ret.Select(x => new BinaryCompleteVertexDataV4
          {
            BoneIndRefs = GetNthBoneIndRef(x),
            BoneWeightList = GetNthBoneWeightList(x),
            BumpNormal = GetNthBumpNormal(x),
            Normal = GetNthNormal(x),
            Position = GetNthPosition(x),
            UV1 = GetNthUV1(x),
            UV2 = GetNthUV2(x),
            UV3 = GetNthUV3(x),
            VertexColor = GetNthVertexColor(x),
            VertexIllumination = GetNthVertexIllumination(x)
          });
        }
        return fullVertexDataList_cache;
      }
    }

    public BinaryVertexV4(int offset, FileCacheHolder fileCache, bool cache = false)
    : base(fileCache,offset,cache)
    {
    }

    public BasicXYZ GetNthBumpNormal(int ind)
    {
      int vertexRealtiveOffset = vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.BumpMapNormal);
      if (vertexListOffset == -1)
        return new BasicXYZ();
      int vertexOffset = nthVertexDefOffset(ind);
      if (vertexOffset == -1)
        return new BasicXYZ();
      int fileRelativeOffset = vertexRealtiveOffset + vertexOffset;
      float x = GetFloatFromFile(fileRelativeOffset);
      float y = GetFloatFromFile(fileRelativeOffset + 4);
      float z = GetFloatFromFile(fileRelativeOffset + 8);
      return new BasicXYZ { X = x, Y = y, Z = z };
    }
    public BasicUV GetNthUV3(int ind)
    {
      int vertexRealtiveOffset = vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.UV3);
      if (vertexListOffset == -1)
        return new BasicUV();
      int vertexOffset = nthVertexDefOffset(ind);
      if (vertexOffset == -1)
        return new BasicUV();
      int fileRelativeOffset = vertexRealtiveOffset + vertexOffset;
      float u = GetFloatFromFile(fileRelativeOffset);
      float v = GetFloatFromFile(fileRelativeOffset + 4);
      return new BasicUV { U = u, V = v };
    }
    public BasicUV GetNthUV2(int ind)
    {
      int vertexRealtiveOffset = vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.UV2);
      if (vertexListOffset == -1)
        return new BasicUV();
      int vertexOffset = nthVertexDefOffset(ind);
      if (vertexOffset == -1)
        return new BasicUV();
      int fileRelativeOffset = vertexRealtiveOffset + vertexOffset;
      float u = GetFloatFromFile(fileRelativeOffset);
      float v = GetFloatFromFile(fileRelativeOffset + 4);
      return new BasicUV { U = u, V = v };
    }
    public BasicUV GetNthUV1(int ind)
    {
      int vertexRealtiveOffset = vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.UV1);
      if (vertexListOffset == -1)
        return new BasicUV();
      int vertexOffset = nthVertexDefOffset(ind);
      if (vertexOffset == -1)
        return new BasicUV();
      int fileRelativeOffset = vertexRealtiveOffset + vertexOffset;
      float u = GetFloatFromFile(fileRelativeOffset);
      float v = GetFloatFromFile(fileRelativeOffset + 4);
      return new BasicUV { U = u, V = v };
    }
    public BasicRGBA GetNthVertexColor(int ind)
    {
      int vertexRealtiveOffset = vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.VertexColor);
      if (vertexListOffset == -1)
        return new BasicRGBA();
      int vertexOffset = nthVertexDefOffset(ind);
      if (vertexOffset == -1)
        return new BasicRGBA();
      int fileRelativeOffset = vertexRealtiveOffset + vertexOffset;
      byte ind1 = GetByteFromFile(fileRelativeOffset);
      byte ind2 = GetByteFromFile(fileRelativeOffset + 1);
      byte ind3 = GetByteFromFile(fileRelativeOffset + 2);
      byte ind4 = GetByteFromFile(fileRelativeOffset + 3);
      return new BasicRGBA { R = ind1, G = ind2, B = ind3, A = ind4 };
    }
    public BasicRGBA GetNthVertexIllumination(int ind)
    {
      int vertexRealtiveOffset = vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.VertexIllumination);
      if (vertexListOffset == -1)
        return new BasicRGBA();
      int vertexOffset = nthVertexDefOffset(ind);
      if (vertexOffset == -1)
        return new BasicRGBA();
      int fileRelativeOffset = vertexRealtiveOffset + vertexOffset;
      byte ind1 = GetByteFromFile(fileRelativeOffset);
      byte ind2 = GetByteFromFile(fileRelativeOffset + 1);
      byte ind3 = GetByteFromFile(fileRelativeOffset + 2);
      byte ind4 = GetByteFromFile(fileRelativeOffset + 3);
      return new BasicRGBA { R = ind1, G = ind2, B = ind3, A = ind4 };
    }
    public BasicXYZ GetNthNormal(int ind)
    {
      int vertexRealtiveOffset = vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.Normal);
      if (vertexListOffset == -1)
        return new BasicXYZ();
      int vertexOffset = nthVertexDefOffset(ind);
      if (vertexOffset == -1)
        return new BasicXYZ();
      int fileRelativeOffset = vertexRealtiveOffset + vertexOffset;
      float x = GetFloatFromFile(fileRelativeOffset);
      float y = GetFloatFromFile(fileRelativeOffset + 4);
      float z = GetFloatFromFile(fileRelativeOffset + 8);
      return new BasicXYZ { X = x, Y = y, Z = z };
    }
    public BinaryBoneIndRefV4 GetNthBoneIndRef(int ind)
    {
      int vertexRealtiveOffset = vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.BoneIndRef);
      if (vertexListOffset == -1)
        return new BinaryBoneIndRefV4();
      int vertexOffset = nthVertexDefOffset(ind);
      if (vertexOffset == -1)
        return new BinaryBoneIndRefV4();
      int fileRelativeOffset = vertexRealtiveOffset + vertexOffset;
      byte ind1 = GetByteFromFile(fileRelativeOffset);
      byte ind2 = GetByteFromFile(fileRelativeOffset + 1);
      byte ind3 = GetByteFromFile(fileRelativeOffset + 2);
      byte ind4 = GetByteFromFile(fileRelativeOffset + 3);
      return new BinaryBoneIndRefV4 { First = ind1, Second = ind2, Third = ind3, Fourth = ind4 };
    }
    public BinaryBoneWeightsV4 GetNthBoneWeightList(int ind)
    {
      int vertexRealtiveOffset = vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.BoneWeightNumIs0);
      int numExplicitWeights = 0;
      vertexRealtiveOffset = vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.BoneWeightNumIs1);
      if (vertexRealtiveOffset != -1)
      {
        numExplicitWeights = 1;
      }
      if (vertexRealtiveOffset == -1)
      {
        vertexRealtiveOffset = vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.BoneWeightNumIs2);
        if (vertexRealtiveOffset != -1)
        {
          numExplicitWeights = 2;
        }
      }
      if (vertexRealtiveOffset == -1)
      {
        vertexRealtiveOffset = vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.BoneWeightNumIs3);
        if (vertexRealtiveOffset != -1)
        {
          numExplicitWeights = 3;
        }
      }
      if (vertexRealtiveOffset == -1)
        return new BinaryBoneWeightsV4();
      int vertexOffset = nthVertexDefOffset(ind);
      if (vertexOffset == -1)
        return new BinaryBoneWeightsV4();
      int fileRelativeOffset = vertexRealtiveOffset + vertexOffset;
      float w1 = 0;
      float w2 = 0;
      float w3 = 0;
      if(numExplicitWeights > 0)
        w1 = GetFloatFromFile(fileRelativeOffset);
      if (numExplicitWeights > 1)
        w2 = GetFloatFromFile(fileRelativeOffset + 4);
      if (numExplicitWeights > 2)
        w3 = GetFloatFromFile(fileRelativeOffset + 8);
      return new BinaryBoneWeightsV4 { First = w1, Second = w2, Third = w3 };
    }
    public BasicXYZ GetNthPosition(int ind)
    {
      int vertexRealtiveOffset = vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag.Position);
      if (vertexListOffset == -1)
        return new BasicXYZ();
      int vertexOffset = nthVertexDefOffset(ind);
      if (vertexOffset == -1)
        return new BasicXYZ();
      int fileRelativeOffset = vertexRealtiveOffset + vertexOffset;
      float x = GetFloatFromFile(fileRelativeOffset);
      float y = GetFloatFromFile(fileRelativeOffset + 4);
      float z = GetFloatFromFile(fileRelativeOffset + 8);
      return new BasicXYZ { X = x, Y = y, Z = z };
    }
    public IEnumerable<BasicXYZ> LoadBumpNormals()
    {
      List<BasicXYZ> ret = new List<BasicXYZ>();
      if (!IsBumpMapNormalDefined)
        return ret;
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(GetNthBumpNormal(def_i));
      }
      return ret;
    }
    public IEnumerable<BasicUV> LoadUV3s()
    {
      List<BasicUV> ret = new List<BasicUV>();
      if (!IsUV3Defined)
        return ret;
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(GetNthUV3(def_i));
      }
      return ret;
    }
    public IEnumerable<BasicUV> LoadUV2s()
    {
      List<BasicUV> ret = new List<BasicUV>();
      if (!IsUV2Defined)
        return ret;
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(GetNthUV2(def_i));
      }
      return ret;
    }
    public IEnumerable<BasicUV> LoadUV1s()
    {
      List<BasicUV> ret = new List<BasicUV>();
      if (!IsUV1Defined)
        return ret;
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(GetNthUV1(def_i));
      }
      return ret;
    }
    public IEnumerable<BasicRGBA> LoadVertexColors()
    {
      List<BasicRGBA> ret = new List<BasicRGBA>();
      if (!IsVertexColorDefined)
        return ret;
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(GetNthVertexColor(def_i));
      }
      return ret;
    }
    public IEnumerable<BasicRGBA> LoadVertexIlluminations()
    {
      List<BasicRGBA> ret = new List<BasicRGBA>();
      if (!IsVertexIlluminationDefined)
        return ret;
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(GetNthVertexIllumination(def_i));
      }
      return ret;
    }
    public IEnumerable<BasicXYZ> LoadNormals()
    {
      List<BasicXYZ> ret = new List<BasicXYZ>();
      if (!IsNormalDefined)
        return ret;
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(GetNthNormal(def_i));
      }
      return ret;
    }
    public IEnumerable<BinaryBoneIndRefV4> LoadBoneIndRefs()
    {
      List<BinaryBoneIndRefV4> ret = new List<BinaryBoneIndRefV4>();
      if (!IsBoneIndRefDefined)
        return ret;
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(GetNthBoneIndRef(def_i));
      }
      return ret;
    }
    public IEnumerable<BinaryBoneWeightsV4> LoadBoneWeightLists()
    {
      List<BinaryBoneWeightsV4> ret = new List<BinaryBoneWeightsV4>();
      if (!IsBoneWeightNumIs1Defined && !IsBoneWeightNumIs2Defined && !IsBoneWeightNumIs3Defined)
        return ret;
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(GetNthBoneWeightList(def_i));
      }
      return ret;
    }
    public IEnumerable<BasicXYZ> LoadPositions()
    {
      List<BasicXYZ> ret = new List<BasicXYZ>();
      if (!IsPositionDefined)
        return ret;
      for(int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(GetNthPosition(def_i));
      }
      return ret;
    }
    public IEnumerable<BasicXYZ> LazyLoadBumpNormals()
    {
      List<int> ret = new List<int>();
      if (!IsBumpMapNormalDefined)
        return Enumerable.Empty<BasicXYZ>();
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(def_i);
      }
      return ret.Select(x => GetNthBumpNormal(x));
    }
    public IEnumerable<BasicUV> LazyLoadUV3s()
    {
      List<int> ret = new List<int>();
      if (!IsUV3Defined)
        return Enumerable.Empty<BasicUV>();
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(def_i);
      }
      return ret.Select(x => GetNthUV3(x));
    }
    public IEnumerable<BasicUV> LazyLoadUV2s()
    {
      List<int> ret = new List<int>();
      if (!IsUV2Defined)
        return Enumerable.Empty<BasicUV>();
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(def_i);
      }
      return ret.Select(x => GetNthUV2(x));
    }
    public IEnumerable<BasicUV> LazyLoadUV1s()
    {
      List<int> ret = new List<int>();
      if (!IsUV1Defined)
        return Enumerable.Empty<BasicUV>();
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(def_i);
      }
      return ret.Select(x => GetNthUV1(x));
    }
    public IEnumerable<BasicRGBA> LazyLoadVertexColors()
    {
      List<int> ret = new List<int>();
      if (!IsVertexColorDefined)
        return Enumerable.Empty<BasicRGBA>();
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(def_i);
      }
      return ret.Select(x => GetNthVertexColor(x));
    }
    public IEnumerable<BasicRGBA> LazyLoadVertexIlluminations()
    {
      List<int> ret = new List<int>();
      if (!IsVertexIlluminationDefined)
        return Enumerable.Empty<BasicRGBA>();
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(def_i);
      }
      return ret.Select(x => GetNthVertexIllumination(x));
    }
    public IEnumerable<BasicXYZ> LazyLoadNormals()
    {
      List<int> ret = new List<int>();
      if (!IsNormalDefined)
        return Enumerable.Empty<BasicXYZ>();
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(def_i);
      }
      return ret.Select(x => GetNthNormal(x));
    }
    public IEnumerable<BinaryBoneIndRefV4> LazyLoadBoneIndRefs()
    {
      List<int> ret = new List<int>();
      if (!IsBoneIndRefDefined)
        return Enumerable.Empty<BinaryBoneIndRefV4>();
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(def_i);
      }
      return ret.Select(x => GetNthBoneIndRef(x));
    }
    public IEnumerable<BinaryBoneWeightsV4> LazyLoadBoneWeightLists()
    {
      List<int> ret = new List<int>();
      if (!IsBoneWeightNumIs1Defined && !IsBoneWeightNumIs2Defined && !IsBoneWeightNumIs3Defined)
        return Enumerable.Empty<BinaryBoneWeightsV4>();
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(def_i);
      }
      return ret.Select(x => GetNthBoneWeightList(x));
    }
    public IEnumerable<BasicXYZ> LazyLoadPositions()
    {
      List<int> ret = new List<int>();
      if (!IsPositionDefined)
        return Enumerable.Empty<BasicXYZ>();
      for (int def_i = 0; def_i != NumberOfVertices; ++def_i)
      {
        ret.Add(def_i);
      }
      return ret.Select(x => GetNthPosition(x));
    }
    public void RereadVertexData()
    {
      bumpNormals_cache = null;
      uv3s_cache = null;
      uv2s_cache = null;
      uv1s_cache = null;
      vertexColors_cache = null;
      vertexIlluminations_cache = null;
      normals_cache = null;
      boneIndRefs_cache = null;
      boneWeightLists_cache = null;
      positions_cache = null;
    }
    public bool IsFalgDefined(BinaryScxV4VertexDataFlag feature)
    {
      return vertexDataRelativeOffsetOfFeature(feature) != -1;
    }

    private int vertexDataRelativeOffsetOfFeature(BinaryScxV4VertexDataFlag feature)
    {
      if ((VertexType & (int)feature) == 0)
        return -1;//not defed in vertex defintion
      int currentOffset = 0;
      foreach(var possFeature in FlagToSize)
      {
        if (possFeature.Key == feature)
          return currentOffset;
        if ((VertexType & (int)possFeature.Key) != 0)
          currentOffset += possFeature.Value;
      }
      return -1;//(unkown feature, should be impossible)
    }
    private int nthVertexDefOffset(int ind)
    {
      if (ind < 0 || ind >= NumberOfVertices)
        return -1;
      return vertexListOffset + (OneVertexSize * ind);
    }

    public static Dictionary<BinaryScxV4VertexDataFlag, int> FlagToSize = new Dictionary<BinaryScxV4VertexDataFlag, int>
    {
      {BinaryScxV4VertexDataFlag.Position,4*3},//XYZ floats
      {BinaryScxV4VertexDataFlag.BoneWeightNumIs0,0},//0 weight-float written (one is implicit 1-Sum(WrittenWeights))
      {BinaryScxV4VertexDataFlag.BoneWeightNumIs1,4*1},//1 weight-float written (one is implicit 1-Sum(WrittenWeights))
      {BinaryScxV4VertexDataFlag.BoneWeightNumIs2,4*2},//2 weight-floats written (one is implicit 1-Sum(WrittenWeights))
      {BinaryScxV4VertexDataFlag.BoneWeightNumIs3,4*3},//3 weight-floats written (one is implicit 1-Sum(WrittenWeights))
      {BinaryScxV4VertexDataFlag.BoneIndRef,4},//byte[4] (four indices to the BoneIndexList defined before the current FileEntry)
      {BinaryScxV4VertexDataFlag.Normal,4*3},//XYZ floats
      {BinaryScxV4VertexDataFlag.VertexIllumination,4},//RGBA
      {BinaryScxV4VertexDataFlag.VertexColor,4},//RGBA
      {BinaryScxV4VertexDataFlag.UV1,4*2},//UV
      {BinaryScxV4VertexDataFlag.UV2,4*2},//UV
      {BinaryScxV4VertexDataFlag.UV3,4*2},//UV
      {BinaryScxV4VertexDataFlag.BumpMapNormal,4*3},//XYZ
      {BinaryScxV4VertexDataFlag.Zero,0},//XYZ
    };
  }
}
