using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class DynamicMeshV3
  {
    public DynamicMaterialV3 CorrespondingMaterial
    {
      get;
      set;
    }
    public List<DynamicVertexV3> VertexDatas
    {
      get;
      set;
    } = new List<DynamicVertexV3>();
    public List<int> VertexIndices
    {
      get;
      set;
    } = new List<int>();

    public DynamicMeshV3(DynamicMaterialV3 correspondingMaterial, BinaryMeshV3 constructFrom = null)
    {
      CorrespondingMaterial = correspondingMaterial;
      if (constructFrom == null)
        return;
      constructFrom.Cache.CacheData();
      foreach(var vert in constructFrom.ReLoadVertexDatas())
      {
        VertexDatas.Add(new DynamicVertexV3(this,vert));
      }
      foreach(var index in constructFrom.ReLoadVertexIndices())
      {
        VertexIndices.Add(index);
      }
    }

    public void MakeSameDefinedFixedDefVertices()
    {
      foreach (var vert in VertexDatas)
      {
        vert.FixIsDefines();
      }
      bool shouldDef_VertexCoordX = VertexDatas.Any(x => x.IsVertexCoordXDefined);
      bool shouldDef_VertexCoordY = VertexDatas.Any(x => x.IsVertexCoordYDefined);
      bool shouldDef_VertexCoordZ = VertexDatas.Any(x => x.IsVertexCoordZDefined);
      bool shouldDef_VertexNormalX = VertexDatas.Any(x => x.IsVertexNormalXDefined);
      bool shouldDef_VertexNormalY = VertexDatas.Any(x => x.IsVertexNormalYDefined);
      bool shouldDef_VertexNormalZ = VertexDatas.Any(x => x.IsVertexNormalZDefined);
      bool shouldDef_UVChannel1X = VertexDatas.Any(x => x.IsUVChannel1XDefined);
      bool shouldDef_UVChannel1Y = VertexDatas.Any(x => x.IsUVChannel1YDefined);
      bool shouldDef_UVChannel2X = VertexDatas.Any(x => x.IsUVChannel2XDefined);
      bool shouldDef_UVChannel2Y = VertexDatas.Any(x => x.IsUVChannel2YDefined);
      bool shouldDef_VertexColorB = VertexDatas.Any(x => x.IsVertexColorBDefined);
      bool shouldDef_VertexColorG = VertexDatas.Any(x => x.IsVertexColorGDefined);
      bool shouldDef_VertexColorR = VertexDatas.Any(x => x.IsVertexColorRDefined);
      bool shouldDef_VertexColorA = VertexDatas.Any(x => x.IsVertexColorADefined);
      bool shouldDef_Unkown1 = VertexDatas.Any(x => x.IsUnkown1Defined);
      bool shouldDef_Unkown2 = VertexDatas.Any(x => x.IsUnkown2Defined);
      bool shouldDef_UVChannel3X = VertexDatas.Any(x => x.IsUVChannel3XDefined);
      bool shouldDef_UVChannel3Y = VertexDatas.Any(x => x.IsUVChannel3YDefined);
      bool shouldDef_Unkown3 = VertexDatas.Any(x => x.IsUnkown1Defined);
      foreach(var vert in VertexDatas)
      {
        vert.IsVertexCoordXDefined = shouldDef_VertexCoordX;
        vert.IsVertexCoordYDefined = shouldDef_VertexCoordY;
        vert.IsVertexCoordZDefined = shouldDef_VertexCoordZ;
        vert.IsVertexNormalXDefined = shouldDef_VertexNormalX;
        vert.IsVertexNormalYDefined = shouldDef_VertexNormalY;
        vert.IsVertexNormalZDefined = shouldDef_VertexNormalZ;
        vert.IsUVChannel1XDefined = shouldDef_UVChannel1X;
        vert.IsUVChannel1YDefined = shouldDef_UVChannel1Y;
        vert.IsUVChannel2XDefined = shouldDef_UVChannel2X;
        vert.IsUVChannel2YDefined = shouldDef_UVChannel2Y;
        vert.IsVertexColorBDefined = shouldDef_VertexColorB;
        vert.IsVertexColorGDefined = shouldDef_VertexColorG;
        vert.IsVertexColorRDefined = shouldDef_VertexColorR;
        vert.IsVertexColorADefined = shouldDef_VertexColorA;
        vert.IsUnkown1Defined = shouldDef_Unkown1;
        vert.IsUnkown2Defined = shouldDef_Unkown2;
        vert.IsUVChannel3XDefined = shouldDef_UVChannel3X;
        vert.IsUVChannel3YDefined = shouldDef_UVChannel3Y;
        vert.IsUnkown1Defined = shouldDef_Unkown3;
      }
    }
    public int GetOneVertexSize()
    {
      if (VertexDatas.Any())
        return VertexDatas.First().GetVertexSize();
      return 0;
    }
    public void FixNumberOfIndices()
    {
      while (VertexIndices.Count % 3 != 0)
        VertexIndices.RemoveAt(VertexIndices.Count - 1);
    }
    public void VerticesSizeBound(int maxCount = ushort.MaxValue)
    {
      if(VertexDatas.Count > maxCount)
      {
        FixNumberOfIndices();
        VertexDatas.RemoveRange(maxCount, VertexDatas.Count - maxCount);
        for(int tri_i = 0; tri_i+2 < VertexIndices.Count; tri_i+=3)
        {
          if(VertexIndices[tri_i] > maxCount ||
              VertexIndices[tri_i+1] > maxCount ||
              VertexIndices[tri_i+2] > maxCount)
          {
            VertexIndices.RemoveAt(tri_i);
            VertexIndices.RemoveAt(tri_i);
            VertexIndices.RemoveAt(tri_i);
            tri_i-=3;
          }
        }
      }
    }
    public void RemoveOverflownTriangles()
    {
      int maxCount = VertexDatas.Count;
      {
        FixNumberOfIndices();
        for (int tri_i = 0; tri_i + 2 < VertexIndices.Count; tri_i += 3)
        {
          if (VertexIndices[tri_i] >= maxCount ||
              VertexIndices[tri_i + 1] >= maxCount ||
              VertexIndices[tri_i + 2] >= maxCount)
          {
            VertexIndices.RemoveAt(tri_i);
            VertexIndices.RemoveAt(tri_i);
            VertexIndices.RemoveAt(tri_i);
            tri_i-=3;
          }
        }
      }
    }
    public void Save(BinaryWriter bw)
    {

      bw.Write((int)VertexDatas.Count);
      foreach(var vert in VertexDatas)
      {
        vert.Save(bw);
      }
      bw.Write(((int)VertexIndices.Count)/3);
      foreach(var ind in VertexIndices)
      {
        bw.Write(ind);
      }
    }
  }
}
