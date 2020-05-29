using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
  public class UVUtil
  {
    public static Vector3D ScaleRotate(Vector3D pos,UVGenerationParams genParams)
    {
      Vector3D rotAxis = new Vector3D(1, 0, 0);
      Vector3D rotAxisZ = new Vector3D(0, 0, 1);
      float posFactor = (float)pos.Z;
      bool posFactNeg = posFactor < 0;
      if ((posFactor < Math.Abs(genParams.rotationScaleMinPosFront) && posFactor >= 0)||
          (posFactor > -Math.Abs(genParams.rotationScaleMinPosBack) && posFactor < 0))
      {
        posFactor = 0;
      }
      else
      {
        if (posFactor > 0)
          posFactor -= genParams.rotationScaleMinPosFront;
        else
          posFactor += genParams.rotationScaleMinPosBack;
      }
      AxisAngleRotation3D rot = new AxisAngleRotation3D();
      if (posFactNeg)
      {
        rot.Angle = Math.PI * genParams.rotationScaleBack * posFactor;
        rot.Axis = rotAxis;
        //AxisAngleRotation3D rot = new AxisAngleRotation3D(rotAxis, Math.PI * scaleBack * posFactor);
        var rotTrasn = new RotateTransform3D(rot);
        var ret = pos;
        ret = rotTrasn.Transform(ret);
        ret.X *= (1.0 + (genParams.rotationDefomrScaleXBack) * Math.Abs(posFactor));
        ret.Y *= (1.0 + (genParams.rotationDefomrScaleYBack) * Math.Abs(posFactor));
        ret.Z *= (1.0 + (genParams.rotationDefomrScaleZBack) * Math.Abs(posFactor));
        return ret;
      }
      else
      {
        rot.Angle = Math.PI * genParams.rotationScaleFront * posFactor;
        rot.Axis = rotAxis;
        //AxisAngleRotation3D rot = new AxisAngleRotation3D(rotAxis, Math.PI * scaleFront * posFactor);
        var rotTrasn = new RotateTransform3D(rot);
        var ret = pos;
        ret = rotTrasn.Transform(ret);
        ret.X *= (1.0 + (genParams.rotationDefomrScaleXFront) * Math.Abs(posFactor));
        ret.Y *= (1.0 + (genParams.rotationDefomrScaleYFront) * Math.Abs(posFactor));
        ret.Z *= (1.0 + (genParams.rotationDefomrScaleZFront) * Math.Abs(posFactor));
        return ret;
      }
    }
    public static SlrrLib.Model.DynamicMeshV4 MinimalCopyFromV3(SlrrLib.Model.DynamicMeshV3 m,int UVNum)
    {
      SlrrLib.Model.DynamicMeshV4 mv4 = new SlrrLib.Model.DynamicMeshV4();
      mv4.VertexData = new SlrrLib.Model.DynamicVertexV4(null);
      mv4.VertexData.VertexDataList = new List<SlrrLib.Model.DynamicCompleteVertexDataV4>();
      foreach (var v3Data in m.VertexDatas)
      {
        var toad = new SlrrLib.Model.DynamicCompleteVertexDataV4();
        if (UVNum == 1)
        {
          toad.IsUV1Defined = true;
          toad.Uv1 = new Model.BasicUV
          {
            U = v3Data.UVChannel1X,
            V = v3Data.UVChannel1Y
          };
        }
        if (UVNum == 2)
        {
          toad.IsUV2Defined = true;
          toad.Uv2 = new Model.BasicUV
          {
            U = v3Data.UVChannel2X,
            V = v3Data.UVChannel2Y
          };
        }
        if (UVNum == 3)
        {
          toad.IsUV3Defined = true;
          toad.Uv3 = new Model.BasicUV
          {
            U = v3Data.UVChannel3X,
            V = v3Data.UVChannel3Y
          };
        }
        toad.Position = new Model.BasicXYZ
        {
          X = v3Data.VertexCoordX,
          Y = v3Data.VertexCoordY,
          Z = v3Data.VertexCoordZ,
        };
        toad.Normal = new Model.BasicXYZ
        {
          X = v3Data.VertexNormalX,
          Y = v3Data.VertexNormalY,
          Z = v3Data.VertexNormalZ,
        };
        mv4.VertexData.VertexDataList.Add(toad);
      }
      return mv4;
    }
    public static void FillUVsFromMinimalCopy(SlrrLib.Model.DynamicMeshV3 mToFill, SlrrLib.Model.DynamicMeshV4 m, int UVNum)
    {
      for(int vert_i = 0; vert_i != m.VertexData.VertexDataList.Count; ++vert_i)
      {
        if (UVNum == 1)
        {
          mToFill.VertexDatas[vert_i].UVChannel1X = m.VertexData.VertexDataList[vert_i].Uv1.U;
          mToFill.VertexDatas[vert_i].UVChannel1Y = m.VertexData.VertexDataList[vert_i].Uv1.V;
        }
        if (UVNum == 2)
        {
          mToFill.VertexDatas[vert_i].UVChannel2X = m.VertexData.VertexDataList[vert_i].Uv2.U;
          mToFill.VertexDatas[vert_i].UVChannel2Y = m.VertexData.VertexDataList[vert_i].Uv2.V;
        }
        if (UVNum == 3)
        {
          mToFill.VertexDatas[vert_i].UVChannel3X = m.VertexData.VertexDataList[vert_i].Uv3.U;
          mToFill.VertexDatas[vert_i].UVChannel3Y = m.VertexData.VertexDataList[vert_i].Uv3.V;
        }
      }
    }
    public static void GenerateUVSphere(SlrrLib.Model.DynamicMeshV3 m, int UVNum, UVGenerationParams genParams, Transform3D trans = null)
    {
      var m4Cpy = MinimalCopyFromV3(m, UVNum);
      GenerateUVSphere(m4Cpy, UVNum, genParams, trans);
      FillUVsFromMinimalCopy(m, m4Cpy, UVNum);
    }
    public static void GenerateUVSphere(SlrrLib.Model.DynamicMeshV4 m, int UVNum,UVGenerationParams genParams, Transform3D trans = null)
    {
      const double PI = 3.14159265359;
      Transform3D transClone = null;
      if (trans != null)
      {
        transClone = trans.Clone();
        transClone.Freeze();
      }
      Vector3D rotAxisZ = new Vector3D(0, 0, 1);
      Vector3D rotAxisX = new Vector3D(1, 0, 0);
      Vector3D rotAxisY = new Vector3D(0, 1, 0);
      AxisAngleRotation3D rotZ = new AxisAngleRotation3D(rotAxisZ, genParams.rotateZ);
      var rotZTrasn = new RotateTransform3D(rotZ);
      var rotAxisX2 = rotZTrasn.Transform(rotAxisX);
      var rotAxisY2 = rotZTrasn.Transform(rotAxisY);
      AxisAngleRotation3D rotX = new AxisAngleRotation3D(rotAxisX2, genParams.rotateX);
      var rotXTrasn = new RotateTransform3D(rotX);
      var rotAxisY3 = rotXTrasn.Transform(rotAxisY2);
      AxisAngleRotation3D rotY = new AxisAngleRotation3D(rotAxisY3, genParams.rotateY);
      var rotYTrasn = new RotateTransform3D(rotY);
      foreach (var vert in m.VertexData.VertexDataList)
      {
        Vector3D position = new Vector3D(vert.Position.X, vert.Position.Y, vert.Position.Z);
        if(trans != null)
        {
          //cfg offset
          position = transClone.Transform(position);
          position.X += transClone.Value.OffsetX;
          position.Y += transClone.Value.OffsetY;
          position.Z += transClone.Value.OffsetZ;
        }
        //offset
        position.X = (position.X) + genParams.offX;
        position.Y = (position.Y) + genParams.offY;
        position.Z = (position.Z) + genParams.offZ;
        //deform
        position = ScaleRotate(position,genParams);
        //rotation


        position = rotZTrasn.Transform(position);
        position = rotXTrasn.Transform(position);
        rotAxisY = rotXTrasn.Transform(rotAxisY);
        position = rotYTrasn.Transform(position);
        //scale
        position.X = (position.X * genParams.scale * genParams.scaleX);
        position.Y = (position.Y * genParams.scale * genParams.scaleY);
        position.Z = (position.Z * genParams.scale * genParams.scaleZ);
        var n = vert.Normal;
        n.X = (float)(position.X);
        n.Y = (float)(position.Y);
        n.Z = (float)(position.Z);
        float u = (float)(((Math.Atan2(n.X, n.Y) / (2.0*PI)) + 1.0) / 2.0);
        float v = (float)(((Math.Asin(n.Z) / PI) + 1.0) / 2.0);
        if (UVNum == 1)
        {
          vert.IsUV1Defined = true;
          vert.Uv1 = new Model.BasicUV
          {
            U = u,
            V = v
          };
        }
        if (UVNum == 2)
        {
          vert.IsUV2Defined = true;
          vert.Uv2 = new Model.BasicUV
          {
            U = u,
            V = v
          };
        }
        if (UVNum == 3)
        {
          vert.IsUV3Defined = true;
          vert.Uv3 = new Model.BasicUV
          {
            U = u,
            V = v
          };
        }
      }
    }
    public static void GenerateUVBox(SlrrLib.Model.DynamicMeshV3 m, int UVNum,UVGenerationParams genParams, float maxLengthPos, Transform3D trans = null)
    {
      var m4Cpy = MinimalCopyFromV3(m, UVNum);
      GenerateUVBox(m4Cpy,UVNum, genParams, maxLengthPos, trans);
      FillUVsFromMinimalCopy(m, m4Cpy, UVNum);
    }
    public static void GenerateUVBox(SlrrLib.Model.DynamicMeshV4 m, int UVNum,UVGenerationParams genParams, float maxLengthPos, Transform3D trans = null)
    {
      foreach (var vert in m.VertexData.VertexDataList)
      {
        Vector3D position = new Vector3D(vert.Position.X, vert.Position.Y, vert.Position.Z);
        if (trans != null)
        {
          position = trans.Transform(position);
          position.X += trans.Value.OffsetX;
          position.Y += trans.Value.OffsetY;
          position.Z += trans.Value.OffsetZ;
          Vector3D rotAxisX = new Vector3D(1, 0, 0);
          AxisAngleRotation3D rotX = new AxisAngleRotation3D(rotAxisX, genParams.rotateX);
          var rotXTrasn = new RotateTransform3D(rotX);
          position = rotXTrasn.Transform(position);
          Vector3D rotAxisY = new Vector3D(0, 1, 0);
          AxisAngleRotation3D rotY = new AxisAngleRotation3D(rotAxisY, genParams.rotateY);
          var rotYTrasn = new RotateTransform3D(rotY);
          position = rotYTrasn.Transform(position);
          Vector3D rotAxisZ = new Vector3D(0, 0, 1);
          AxisAngleRotation3D rotZ = new AxisAngleRotation3D(rotAxisZ, genParams.rotateZ);
          var rotZTrasn = new RotateTransform3D(rotZ);
          position = rotZTrasn.Transform(position);
          position = ScaleRotate(position, genParams);
        }
        Vector3D normal = new Vector3D(vert.Normal.X, vert.Normal.Y, vert.Normal.Z);
        if (trans != null)
        {
          normal = trans.Transform(normal);
          normal = ScaleRotate(normal, genParams);
        }
        var n = position;
        n.X /= maxLengthPos;
        n.Y /= maxLengthPos;
        n.Z /= maxLengthPos;
        n.X *= genParams.scale;
        n.Y *= genParams.scale;
        n.Z *= genParams.scale;
        n.X *= genParams.scaleX;
        n.Y *= genParams.scaleY;
        n.Z *= genParams.scaleZ;
        n.X += genParams.offX;
        n.Y += genParams.offY;
        n.Z += genParams.offZ;
        var absN = normal;
        float l = (float)Math.Sqrt(n.X * n.X + n.Y * n.Y + n.Z * n.Z);
        absN.X = (float)Math.Abs(absN.X);
        absN.Y = (float)Math.Abs(absN.Y);
        absN.Z = (float)Math.Abs(absN.Z);
        float u = 0;
        float v = 0;
        if (absN.X > absN.Y && absN.X > absN.Z)
        {
          u = (float)n.Y;
          v = (float)n.Z;
        }
        else if (absN.Z > absN.Y && absN.Z > absN.X)
        {
          u = (float)n.X;
          v = (float)n.Y;
        }
        else
        {
          u = (float)n.X;
          v = (float)n.Z;
        }
        if (UVNum == 1)
        {
          vert.IsUV1Defined = true;
          vert.Uv1 = new Model.BasicUV
          {
            U = u,
            V = v
          };
        }
        if (UVNum == 2)
        {
          vert.IsUV2Defined = true;
          vert.Uv2 = new Model.BasicUV
          {
            U = u,
            V = v
          };
        }
        if (UVNum == 3)
        {
          vert.IsUV3Defined = true;
          vert.Uv3 = new Model.BasicUV
          {
            U = u,
            V = v
          };
        }
      }
    }
    public static void GenerateUVNormalize(SlrrLib.Model.DynamicMeshV3 m, int UVNum, UVCoordinateBounds bounds)
    {
      var m4Cpy = MinimalCopyFromV3(m, UVNum);
      GenerateUVNormalize(m4Cpy, UVNum, bounds);
      FillUVsFromMinimalCopy(m, m4Cpy, UVNum);
    }
    public static void GenerateUVNormalize(SlrrLib.Model.DynamicMeshV4 m, int UVNum, UVCoordinateBounds bounds)
    {
      float maxUV = Math.Max(bounds.maxU, bounds.maxV);
      float minUV = Math.Min(bounds.minU, bounds.minV);
      float maxU = maxUV;
      float minU = minUV;
      float maxV = maxUV;
      float minV = minUV;
      foreach (var vert in m.VertexData.VertexDataList)
      {
        if (UVNum == 1)
        {
          vert.IsUV1Defined = true;
          vert.Uv1 = new Model.BasicUV
          {
            U = (vert.Uv1.U - minU) / (maxU - minU),
            V = (vert.Uv1.V - minV) / (maxV - minV)
          };
        }
        if (UVNum == 2)
        {
          vert.IsUV2Defined = true;
          vert.Uv2 = new Model.BasicUV
          {
            U = (vert.Uv2.U - minU) / (maxU - minU),
            V = (vert.Uv2.V - minV) / (maxV - minV)
          };
        }
        if (UVNum == 3)
        {
          vert.IsUV3Defined = true;
          vert.Uv3 = new Model.BasicUV
          {
            U = (vert.Uv3.U - minU) / (maxU - minU),
            V = (vert.Uv3.V - minV) / (maxV - minV)
          };
        }
      }
    }
  }

  public struct UVCoordinateBounds
  {
    public float maxU;
    public float minU;
    public float minV;
    public float maxV;
  }

  public struct UVGenerationParams
  {
    public float scale;
    public float offX;
    public float offY;
    public float offZ;
    public float scaleX;
    public float scaleY;
    public float scaleZ;
    public float rotationScaleFront;
    public float rotationScaleMinPosFront;
    public float rotationDefomrScaleXFront;
    public float rotationDefomrScaleYFront;
    public float rotationDefomrScaleZFront;
    public float rotationScaleBack;
    public float rotationScaleMinPosBack;
    public float rotationDefomrScaleXBack;
    public float rotationDefomrScaleYBack;
    public float rotationDefomrScaleZBack;
    public float rotateX;
    public float rotateY;
    public float rotateZ;
  }
}
