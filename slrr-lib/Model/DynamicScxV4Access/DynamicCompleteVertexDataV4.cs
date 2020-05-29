using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class DynamicCompleteVertexDataV4
  {
    private BasicXYZ position;
    private DynamicBoneWeightsV4 boneWeightList;
    private BasicXYZ normal;
    private BasicRGBA vertexIllumination;
    private BasicRGBA vertexColor;
    private BasicUV uv1;
    private BasicUV uv2;
    private BasicUV uv3;
    private BasicXYZ bumpNormal;

    public int VertexType
    {
      get;
      set;
    }
    public BasicXYZ Position
    {
      get
      {
        return position;
      }
      set
      {
        position = value;
        IsPositionDefined = true;
      }
    }
    public DynamicBoneWeightsV4 BoneWeightList
    {
      get
      {
        return boneWeightList;
      }
      set
      {
        boneWeightList = value;
        SetBoneWeightDefinedsFromBoneWeights();
      }
    }
    public BasicXYZ Normal
    {
      get
      {
        return normal;
      }
      set
      {
        normal = value;
        IsNormalDefined = true;
      }
    }
    public BasicRGBA VertexIllumination
    {
      get
      {
        return vertexIllumination;
      }
      set
      {
        vertexIllumination = value;
        IsVertexIlluminationDefined = true;
      }
    }
    public BasicRGBA VertexColor
    {
      get
      {
        return vertexColor;
      }
      set
      {
        vertexColor = value;
        IsVertexColorDefined = true;
      }
    }
    public BasicUV Uv1
    {
      get
      {
        return uv1;
      }
      set
      {
        uv1 = value;
        IsUV1Defined = true;
      }
    }
    public BasicUV Uv2
    {
      get
      {
        return uv2;
      }
      set
      {
        uv2 = value;
        IsUV2Defined = true;
      }
    }
    public BasicUV Uv3
    {
      get
      {
        return uv3;
      }
      set
      {
        uv3 = value;
        IsUV3Defined = true;
      }
    }
    public BasicXYZ BumpNormal
    {
      get
      {
        return bumpNormal;
      }
      set
      {
        bumpNormal = value;
        IsBumpMapNormalDefined = true;
      }
    }
    public bool IsPositionDefined
    {
      get
      {
        return (VertexType & (int)BinaryScxV4VertexDataFlag.Position) != 0;
      }
      set
      {
        if (value)
        {
          VertexType |= (int)BinaryScxV4VertexDataFlag.Position;
        }
        else
        {
          VertexType &= ~(int)BinaryScxV4VertexDataFlag.Position;
        }
      }
    }
    public bool IsBoneWeightNumIs0Defined
    {
      get
      {
        return (VertexType & (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs0) != 0;
      }
      set
      {
        if (value)
        {
          IsBoneWeightNumIs1Defined = false;
          IsBoneWeightNumIs2Defined = false;
          IsBoneWeightNumIs3Defined = false;
          VertexType |= (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs0;
        }
        else
        {
          VertexType &= ~(int)BinaryScxV4VertexDataFlag.BoneWeightNumIs0;
        }
      }
    }
    public bool IsBoneWeightNumIs1Defined
    {
      get
      {
        return (VertexType & (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs1) != 0;
      }
      set
      {
        if (value)
        {
          IsBoneWeightNumIs0Defined = false;
          IsBoneWeightNumIs2Defined = false;
          IsBoneWeightNumIs3Defined = false;
          VertexType |= (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs1;
        }
        else
        {
          VertexType &= ~(int)BinaryScxV4VertexDataFlag.BoneWeightNumIs1;
        }
      }
    }
    public bool IsBoneWeightNumIs2Defined
    {
      get
      {
        return (VertexType & (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs2) != 0;
      }
      set
      {
        if (value)
        {
          IsBoneWeightNumIs0Defined = false;
          IsBoneWeightNumIs1Defined = false;
          IsBoneWeightNumIs3Defined = false;
          VertexType |= (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs2;
        }
        else
        {
          VertexType &= ~(int)BinaryScxV4VertexDataFlag.BoneWeightNumIs2;
        }
      }
    }
    public bool IsBoneWeightNumIs3Defined
    {
      get
      {
        return (VertexType & (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs3) != 0;
      }
      set
      {
        if (value)
        {
          IsBoneWeightNumIs0Defined = false;
          IsBoneWeightNumIs1Defined = false;
          IsBoneWeightNumIs2Defined = false;
          VertexType |= (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs3;
        }
        else
        {
          VertexType &= ~(int)BinaryScxV4VertexDataFlag.BoneWeightNumIs3;
        }
      }
    }
    public bool IsBoneIndRefDefined
    {
      get
      {
        return (VertexType & (int)BinaryScxV4VertexDataFlag.BoneIndRef) != 0;
      }
      set
      {
        if (value)
        {
          VertexType |= (int)BinaryScxV4VertexDataFlag.BoneIndRef;
        }
        else
        {
          VertexType &= ~(int)BinaryScxV4VertexDataFlag.BoneIndRef;
        }
      }
    }// if the bone list is a sparsebonelist(type of 2) this have been seen to be false it is used in suspesnions shocks and other springlike models
    public bool IsNormalDefined
    {
      get
      {
        return (VertexType & (int)BinaryScxV4VertexDataFlag.Normal) != 0;
      }
      set
      {
        if (value)
        {
          VertexType |= (int)BinaryScxV4VertexDataFlag.Normal;
        }
        else
        {
          VertexType &= ~(int)BinaryScxV4VertexDataFlag.Normal;
        }
      }
    }
    public bool IsVertexIlluminationDefined
    {
      get
      {
        return (VertexType & (int)BinaryScxV4VertexDataFlag.VertexIllumination) != 0;
      }
      set
      {
        if (value)
        {
          VertexType |= (int)BinaryScxV4VertexDataFlag.VertexIllumination;
        }
        else
        {
          VertexType &= ~(int)BinaryScxV4VertexDataFlag.VertexIllumination;
        }
      }
    }
    public bool IsVertexColorDefined
    {
      get
      {
        return (VertexType & (int)BinaryScxV4VertexDataFlag.VertexColor) != 0;
      }
      set
      {
        if (value)
        {
          VertexType |= (int)BinaryScxV4VertexDataFlag.VertexColor;
        }
        else
        {
          VertexType &= ~(int)BinaryScxV4VertexDataFlag.VertexColor;
        }
      }
    }
    public bool IsUV1Defined
    {
      get
      {
        return (VertexType & (int)BinaryScxV4VertexDataFlag.UV1) != 0;
      }
      set
      {
        if (value)
        {
          VertexType |= (int)BinaryScxV4VertexDataFlag.UV1;
        }
        else
        {
          VertexType &= ~(int)BinaryScxV4VertexDataFlag.UV1;
        }
      }
    }
    public bool IsUV2Defined
    {
      get
      {
        return (VertexType & (int)BinaryScxV4VertexDataFlag.UV2) != 0;
      }
      set
      {
        if (value)
        {
          VertexType |= (int)BinaryScxV4VertexDataFlag.UV2;
        }
        else
        {
          VertexType &= ~(int)BinaryScxV4VertexDataFlag.UV2;
        }
      }
    }
    public bool IsUV3Defined
    {
      get
      {
        return (VertexType & (int)BinaryScxV4VertexDataFlag.UV3) != 0;
      }
      set
      {
        if (value)
        {
          VertexType |= (int)BinaryScxV4VertexDataFlag.UV3;
        }
        else
        {
          VertexType &= ~(int)BinaryScxV4VertexDataFlag.UV3;
        }
      }
    }
    public bool IsBumpMapNormalDefined
    {
      get
      {
        return (VertexType & (int)BinaryScxV4VertexDataFlag.BumpMapNormal) != 0;
      }
      set
      {
        if (value)
        {
          VertexType |= (int)BinaryScxV4VertexDataFlag.BumpMapNormal;
        }
        else
        {
          VertexType &= ~(int)BinaryScxV4VertexDataFlag.BumpMapNormal;
        }
      }
    }

    public DynamicCompleteVertexDataV4(BinaryCompleteVertexDataV4 constructFrom = null,BinaryVertexV4 parentOfConstructFrom = null)
    {
      if (constructFrom == null || parentOfConstructFrom == null)
        return;
      if (parentOfConstructFrom.IsPositionDefined)
        Position = constructFrom.Position;
      if(parentOfConstructFrom.IsBoneIndRefDefined ||
          parentOfConstructFrom.IsBoneWeightNumIs3Defined||
          parentOfConstructFrom.IsBoneWeightNumIs2Defined||
          parentOfConstructFrom.IsBoneWeightNumIs1Defined||
          parentOfConstructFrom.IsBoneWeightNumIs0Defined)
      {
        BoneWeightList = new DynamicBoneWeightsV4();
        if(parentOfConstructFrom.IsBoneIndRefDefined)
        {
          BoneWeightList.IsBoneIndRefDefined = true;
          if (parentOfConstructFrom.IsBoneWeightNumIs0Defined)
            BoneWeightList.Ind1 = constructFrom.BoneIndRefs.First;
          else if(parentOfConstructFrom.IsBoneWeightNumIs1Defined)
          {
            BoneWeightList.Ind1 = constructFrom.BoneIndRefs.First;
            BoneWeightList.Ind2 = constructFrom.BoneIndRefs.Second;
          }
          else if(parentOfConstructFrom.IsBoneWeightNumIs2Defined)
          {
            BoneWeightList.Ind1 = constructFrom.BoneIndRefs.First;
            BoneWeightList.Ind2 = constructFrom.BoneIndRefs.Second;
            BoneWeightList.Ind3 = constructFrom.BoneIndRefs.Third;
          }
          else if(parentOfConstructFrom.IsBoneWeightNumIs3Defined)
          {
            BoneWeightList.Ind1 = constructFrom.BoneIndRefs.First;
            BoneWeightList.Ind2 = constructFrom.BoneIndRefs.Second;
            BoneWeightList.Ind3 = constructFrom.BoneIndRefs.Third;
            BoneWeightList.Ind4 = constructFrom.BoneIndRefs.Fourth;
          }
        }
        if (parentOfConstructFrom.IsBoneWeightNumIs0Defined)
        {
          BoneWeightList.W1 = 1.0f;
        }
        else if (parentOfConstructFrom.IsBoneWeightNumIs1Defined)
        {
          BoneWeightList.W1 = constructFrom.BoneWeightList.First;
          BoneWeightList.W2 = constructFrom.BoneWeightList.Implicit;
        }
        else if (parentOfConstructFrom.IsBoneWeightNumIs2Defined)
        {
          BoneWeightList.W1 = constructFrom.BoneWeightList.First;
          BoneWeightList.W2 = constructFrom.BoneWeightList.Second;
          BoneWeightList.W3 = constructFrom.BoneWeightList.Implicit;
        }
        else if (parentOfConstructFrom.IsBoneWeightNumIs3Defined)
        {
          BoneWeightList.W1 = constructFrom.BoneWeightList.First;
          BoneWeightList.W2 = constructFrom.BoneWeightList.Second;
          BoneWeightList.W3 = constructFrom.BoneWeightList.Third;
          BoneWeightList.W4 = constructFrom.BoneWeightList.Implicit;
        }
      }
      if (parentOfConstructFrom.IsNormalDefined)
        Normal = constructFrom.Normal;
      if (parentOfConstructFrom.IsVertexIlluminationDefined)
        VertexIllumination = constructFrom.VertexIllumination;
      if (parentOfConstructFrom.IsVertexColorDefined)
        VertexColor = constructFrom.VertexColor;
      if (parentOfConstructFrom.IsUV1Defined)
        Uv1 = constructFrom.UV1;
      if (parentOfConstructFrom.IsUV2Defined)
        Uv2 = constructFrom.UV2;
      if (parentOfConstructFrom.IsUV3Defined)
        Uv3 = constructFrom.UV3;
      if (parentOfConstructFrom.IsBumpMapNormalDefined)
        BumpNormal = constructFrom.BumpNormal;
    }

    public void SetBoneWeightDefinedsFromBoneWeights()
    {
      if (BoneWeightList == null)
        return;
      if (BoneWeightList.IsW1Defined)
        IsBoneWeightNumIs0Defined = true;//the one defined weight must be 1.0
      if (BoneWeightList.IsW2Defined)
        IsBoneWeightNumIs1Defined = true;
      if (BoneWeightList.IsW3Defined)
        IsBoneWeightNumIs2Defined = true;
      if (BoneWeightList.IsW4Defined)
        IsBoneWeightNumIs3Defined = true;
      IsBoneIndRefDefined = BoneWeightList.IsBoneIndRefDefined;
    }
    public void Save(BinaryWriter bw)
    {
      if(IsPositionDefined)
      {
        bw.Write(Position.X);
        bw.Write(Position.Y);
        bw.Write(Position.Z);
      }
      if (IsBoneWeightNumIs0Defined)
      {
        //SKIP
      }
      if (IsBoneWeightNumIs1Defined)
      {
        bw.Write(BoneWeightList.W1);
      }
      if(IsBoneWeightNumIs2Defined)
      {
        bw.Write(BoneWeightList.W1);
        bw.Write(BoneWeightList.W2);
      }
      if(IsBoneWeightNumIs3Defined)
      {
        bw.Write(BoneWeightList.W1);
        bw.Write(BoneWeightList.W2);
        bw.Write(BoneWeightList.W3);
      }
      if(IsBoneIndRefDefined)
      {
        bw.Write(BoneWeightList.Ind1);
        bw.Write(BoneWeightList.Ind2);
        bw.Write(BoneWeightList.Ind3);
        bw.Write(BoneWeightList.Ind4);
      }
      if(IsNormalDefined)
      {
        bw.Write(Normal.X);
        bw.Write(Normal.Y);
        bw.Write(Normal.Z);
      }
      if(IsVertexIlluminationDefined)
      {
        bw.Write(VertexIllumination.R);
        bw.Write(VertexIllumination.G);
        bw.Write(VertexIllumination.B);
        bw.Write(VertexIllumination.A);
      }
      if (IsVertexColorDefined)
      {
        bw.Write(VertexColor.R);
        bw.Write(VertexColor.G);
        bw.Write(VertexColor.B);
        bw.Write(VertexColor.A);
      }
      if (IsUV1Defined)
      {
        bw.Write(Uv1.U);
        bw.Write(Uv1.V);
      }
      if (IsUV2Defined)
      {
        bw.Write(Uv2.U);
        bw.Write(Uv2.V);
      }
      if (IsUV3Defined)
      {
        bw.Write(Uv3.U);
        bw.Write(Uv3.V);
      }
      if (IsBumpMapNormalDefined)
      {
        bw.Write(BumpNormal.X);
        bw.Write(BumpNormal.Y);
        bw.Write(BumpNormal.Z);
      }
    }
  }
}
