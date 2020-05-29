using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Vec3 = System.Windows.Media.Media3D.Vector3D;

namespace SlrrLib.Model
{
  public class TrcSplineSegment
  {
    public Vec3 TargetLanePosition
    {
      get;
      set;
    }
    public Vec3 SourceLanePosition
    {
      get;
      set;
    }
    public Vec3 TargetLaneDeltaControlPoint
    {
      get;
      set;
    }
    public Vec3 SourceLaneMinusDeltaControlP
    {
      get;
      set;
    }
    public float LengthOfSplineAtTarget
    {
      get;
      set;
    }
    public float LengthOfSplineAtSource
    {
      get;
      set;
    }
    public byte UnkByte_1
    {
      get;
      set;
    }

    public static TrcSplineSegment Load(BinaryReader br)
    {
      TrcSplineSegment ret = new TrcSplineSegment();
      ret.TargetLanePosition = new Vec3
      {
        X = br.ReadSingle(),
        Y = br.ReadSingle(),
        Z = br.ReadSingle()
      };
      ret.SourceLanePosition = new Vec3
      {
        X = br.ReadSingle(),
        Y = br.ReadSingle(),
        Z = br.ReadSingle()
      };
      ret.TargetLaneDeltaControlPoint = new Vec3
      {
        X = br.ReadSingle(),
        Y = br.ReadSingle(),
        Z = br.ReadSingle()
      };
      ret.SourceLaneMinusDeltaControlP = new Vec3
      {
        X = br.ReadSingle(),
        Y = br.ReadSingle(),
        Z = br.ReadSingle()
      };
      ret.LengthOfSplineAtTarget=br.ReadSingle();
      ret.LengthOfSplineAtSource=br.ReadSingle();
      ret.UnkByte_1=br.ReadByte();
      return ret;
    }

    public void SetNormalLengthAsRatioOfLength(double ratio = 0.25)
    {
      var length = GetLengthOfSpline(10)*ratio;
      SourceLaneMinusDeltaControlP.Normalize();
      TargetLaneDeltaControlPoint.Normalize();
      SourceLaneMinusDeltaControlP *= length;
      TargetLaneDeltaControlPoint *= length;
    }
    public void Save(BinaryWriter bw)
    {
      bw.Write((float)TargetLanePosition.X);
      bw.Write((float)TargetLanePosition.Y);
      bw.Write((float)TargetLanePosition.Z);
      bw.Write((float)SourceLanePosition.X);
      bw.Write((float)SourceLanePosition.Y);
      bw.Write((float)SourceLanePosition.Z);
      bw.Write((float)TargetLaneDeltaControlPoint.X);
      bw.Write((float)TargetLaneDeltaControlPoint.Y);
      bw.Write((float)TargetLaneDeltaControlPoint.Z);
      bw.Write((float)SourceLaneMinusDeltaControlP.X);
      bw.Write((float)SourceLaneMinusDeltaControlP.Y);
      bw.Write((float)SourceLaneMinusDeltaControlP.Z);
      bw.Write((float)LengthOfSplineAtTarget);
      bw.Write((float)LengthOfSplineAtSource);
      UnkByte_1 = 1;
      bw.Write(UnkByte_1);
    }
    public Vec3 GetMaxPosition()
    {
      Vec3 ret = new Vec3(float.MinValue, float.MinValue, float.MinValue);
      if (ret.X < TargetLanePosition.X)
        ret.X = TargetLanePosition.X;
      if (ret.X < SourceLanePosition.X)
        ret.X = SourceLanePosition.X;

      if (ret.Y < TargetLanePosition.Y)
        ret.Y = TargetLanePosition.Y;
      if (ret.Y < SourceLanePosition.Y)
        ret.Y = SourceLanePosition.Y;

      if (ret.Z < TargetLanePosition.Z)
        ret.Z = TargetLanePosition.Z;
      if (ret.Z < SourceLanePosition.Z)
        ret.Z = SourceLanePosition.Z;

      return ret;
    }
    public Vec3 GetMinPosition()
    {
      Vec3 ret = new Vec3(float.MaxValue, float.MaxValue, float.MaxValue);
      if (ret.X > TargetLanePosition.X)
        ret.X = TargetLanePosition.X;
      if (ret.X > SourceLanePosition.X)
        ret.X = SourceLanePosition.X;

      if (ret.Y > TargetLanePosition.Y)
        ret.Y = TargetLanePosition.Y;
      if (ret.Y > SourceLanePosition.Y)
        ret.Y = SourceLanePosition.Y;

      if (ret.Z > TargetLanePosition.Z)
        ret.Z = TargetLanePosition.Z;
      if (ret.Z > SourceLanePosition.Z)
        ret.Z = SourceLanePosition.Z;

      return ret;
    }
    public TrcSplineSegment Copy()
    {
      TrcSplineSegment ret = new TrcSplineSegment();
      ret.TargetLanePosition = TargetLanePosition;
      ret.SourceLanePosition = SourceLanePosition;
      ret.TargetLaneDeltaControlPoint = TargetLaneDeltaControlPoint;
      ret.SourceLaneMinusDeltaControlP = SourceLaneMinusDeltaControlP;
      ret.LengthOfSplineAtTarget = LengthOfSplineAtTarget;
      ret.LengthOfSplineAtSource = LengthOfSplineAtSource;
      ret.UnkByte_1 = UnkByte_1;
      return ret;
    }
    public Vec3 EvaluateAtParam(float t)
    {
      var n1 = SourceLanePosition - (SourceLaneMinusDeltaControlP / Trc.DividerForBezier);
      var n2 = TargetLanePosition + (TargetLaneDeltaControlPoint / Trc.DividerForBezier);
      return (1 - t)*(1 - t)*(1 - t) * SourceLanePosition +
             3 * (1 - t)*(1 - t) * t * n1 +
             3 * (1 - t) * t * t * n2 +
             t * t * t * TargetLanePosition;
    }
    public float GetLengthOfSpline(int resolution)
    {
      if (resolution == 0)
        return (float)(TargetLanePosition-SourceLanePosition).Length;
      Vec3 first = EvaluateAtParam(0.0f);
      float step = 1.0f / (float)resolution;
      Vec3 second;
      double length = 0;
      for (float t = step; t <= 1.0f; t += step)
      {
        second = EvaluateAtParam(t);
        length += (second - first).Length;
        first = second;
        if (t + step >= 1.0f)
        {
          second = EvaluateAtParam(1.0f);
          length += (second - first).Length;
          break;
        }
      }
      return (float)length;
    }
    public override string ToString()
    {
      return "SplSegment";
    }
  }
}
