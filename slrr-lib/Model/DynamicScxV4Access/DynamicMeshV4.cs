using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class DynamicMeshV4
  {
    public DynamicMaterialV4 Material
    {
      get;
      set;
    } = null;
    public DynamicHardSurfaceDefV4 HardSurfaceDef
    {
      get;
      set;
    } = null;
    public DynamicBoneListV4 BoneList
    {
      get;
      set;
    } = null;//either hardSurface or bonelist
    public DynamicVertexV4 VertexData
    {
      get;
      set;
    } = null;
    public DynamicFaceDefV4 FaceDef
    {
      get;
      set;
    } = null;
    public int Size
    {
      get
      {
        int ret = 0;
        if (Material != null)
          ret += Material.Size;
        if (FaceDef != null)
          ret += FaceDef.Size;
        if (VertexData != null)
          ret += VertexData.Size;
        if (HardSurfaceDef != null)
          ret += HardSurfaceDef.Size;
        else if (BoneList != null)
          ret += BoneList.Size;
        return ret;
      }
    }
    public int CountDefed
    {
      get
      {
        int ret = 0;
        if (Material != null)
          ret++;
        if (FaceDef != null)
          ret++;
        if (VertexData != null)
          ret++;
        if (HardSurfaceDef != null)
          ret++;
        if (BoneList != null)
          ret++;
        return ret;
      }
    }

    public void Save(BinaryWriter bw)
    {
      if (Material != null)
        Material.Save(bw);
      if (HardSurfaceDef != null)
        HardSurfaceDef.Save(bw);
      else if (BoneList != null)
        BoneList.Save(bw);
      if (VertexData != null)
        VertexData.Save(bw);
      if (FaceDef != null)
        FaceDef.Save(bw);
    }
  }
}
