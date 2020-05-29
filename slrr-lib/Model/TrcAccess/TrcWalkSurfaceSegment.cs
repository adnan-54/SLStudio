using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vec3 = System.Windows.Media.Media3D.Vector3D;

namespace SlrrLib.Model
{
  public enum TrcWalkSegmentTypeDescription
  {
    SimpleOtherConnectingSegmentWithNoOtherWalkRef = 21,
    SimpleSelfConnectingSegmentWithOtherWalkRef = 22,
    SplineOtherConnectingSegmentWithNoOtherWalkRef = 23
  }

  public class TrcWalkSurfaceSegment
  {
    /// <summary>
    /// 21 -> OtherWalkIndex == -1 | 22 -> OtherWalkIndex != -1 | 23 -> OtherWalkIndex == -1
    /// </summary>
    public int Type
    {
      get;
      set;
    }
    public TrcWalkSegmentTypeDescription PrettyType
    {
      get
      {
        return (TrcWalkSegmentTypeDescription)Type;
      }
      set
      {
        Type = (int)value;
      }
    }
    public int CountOfPoints
    {
      get;
      set;
    }
    public List<TrcWalkControlPoint3D> ControlPoints
    {
      get;
      set;
    } = new List<TrcWalkControlPoint3D>();
    public TrcWalkDirectionDescription Direction
    {
      get;
      set;
    } = new TrcWalkDirectionDescription();

    public void InsertHalfwayControlPointAfter(int index)
    {
      if (index == ControlPoints.Count - 1)
        return;
      var source = ControlPoints[index];
      var target = ControlPoints[index+1];
      var toad = new TrcWalkControlPoint3D();
      toad.Position = EvaluateSpline(index,0.5f);
      toad.Normal = source.Normal + target.Normal;
      toad.Normal *= 0.5;
      ControlPoints.Insert(index+1, toad);
    }
    public void InsertHalfwayControlPointBefore(int index)
    {
      if (index == 0)
        return;
      var source = ControlPoints[index];
      var target = ControlPoints[index-1];
      var toad = new TrcWalkControlPoint3D();
      toad.Position = EvaluateSpline(index-1,0.5f);
      toad.Normal = source.Normal + target.Normal;
      toad.Normal *= 0.5;
      ControlPoints.Insert(index, toad);
    }
    public Vec3 EvaluateMiddleSpline(float t)
    {
      if (Type != 23)
        return new Vec3();
      if (ControlPoints.Count % 2 == 1)
      {
        return ControlPoints[ControlPoints.Count / 2].Position;
      }
      var middle1 = ControlPoints[ControlPoints.Count / 2 - 1];
      var middle2 = ControlPoints[ControlPoints.Count / 2];
      var n1 = -middle2.Normal;
      var n2 = -middle1.Normal;
      var p1v = middle2.Position;
      var p2v = middle1.Position;
      n1.Normalize();
      n2.Normalize();
      var m1 = (n1 * (p1v - p2v).Length/3.14)+p1v;
      var m2 = (n2 * (p1v - p2v).Length/3.14)+p2v;
      return Math.Pow(1 - t, 3) * p1v +
             3 * Math.Pow(1 - t, 2) * t * m1 +
             3 * (1 - t) * t * t * m2 +
             t * t * t * p2v;
    }
    public Vec3 EvaluateSpline(int beginningControlPointIndex,float t)
    {
      if (Type != 23)
        return new Vec3();
      var middle1 = ControlPoints[beginningControlPointIndex];
      var middle2 = ControlPoints[beginningControlPointIndex+1];
      var n1 = middle1.Normal;
      var n2 = middle2.Normal;
      var p1v = middle1.Position;
      var p2v = middle2.Position;
      n1.Normalize();
      n2.Normalize();
      var m1 = (n1 * (p1v - p2v).Length/3.14)+p1v;
      var m2 = (n2 * (p1v - p2v).Length/3.14)+p2v;
      return Math.Pow(1 - t, 3) * p1v +
             3 * Math.Pow(1 - t, 2) * t * m1 +
             3 * (1 - t) * t * t * m2 +
             t * t * t * p2v;
    }
    public override string ToString()
    {
      return Enum.GetName(typeof(TrcWalkSegmentTypeDescription), PrettyType);
    }
  }
}
