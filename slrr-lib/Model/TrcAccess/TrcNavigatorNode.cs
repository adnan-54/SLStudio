using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Vec3 = System.Windows.Media.Media3D.Vector3D;

namespace SlrrLib.Model
{
  public class TrcNavigatorNode : TrcObject
  {
    public int LaneIndInFlattened
    {
      get;
      set;
    }
    public TrcLane LaneInFlattened_1
    {
      get;
      set;
    }
    public int IntersectionIndInFlattened_1
    {
      get;
      set;
    }
    public TrcIntersection IntersectionInFlattened_1
    {
      get;
      set;
    }
    public int IntersectionIndInFlattened_2
    {
      get;
      set;
    }
    public TrcIntersection IntersectionInFlattened_2
    {
      get;
      set;
    }
    public float Pos1X
    {
      get;
      set;
    }
    public float Pos1Y
    {
      get;
      set;
    }
    public float Pos1Z
    {
      get;
      set;
    }
    public float Pos2X
    {
      get;
      set;
    }
    public float Pos2Y
    {
      get;
      set;
    }
    public float Pos2Z
    {
      get;
      set;
    }
    public int UnkZero
    {
      get;
      set;
    }
    public List<TrcSplineSegment> Spline
    {
      get;
      set;
    } = new List<TrcSplineSegment>();

    public static TrcNavigatorNode Load(BinaryReader br)
    {
      TrcNavigatorNode ret = new TrcNavigatorNode();
      ret.LaneIndInFlattened = br.ReadInt32();
      ret.IntersectionIndInFlattened_1 = br.ReadInt32();
      ret.IntersectionIndInFlattened_2 = br.ReadInt32();
      ret.Pos1X= br.ReadSingle();
      ret.Pos1Y= br.ReadSingle();
      ret.Pos1Z= br.ReadSingle();
      ret.Pos2X= br.ReadSingle();
      ret.Pos2Y= br.ReadSingle();
      ret.Pos2Z= br.ReadSingle();
      ret.UnkZero= br.ReadInt32();
      int count = br.ReadInt32();
      ret.Spline = new List<TrcSplineSegment>();
      for (int i = 0; i != count; ++i)
      {
        ret.Spline.Add(TrcSplineSegment.Load(br));
      }
      return ret;
    }

    public void Save(BinaryWriter bw)
    {
      bw.Write(LaneIndInFlattened);
      bw.Write(IntersectionIndInFlattened_1);
      bw.Write(IntersectionIndInFlattened_2);
      bw.Write(Pos1X);
      bw.Write(Pos1Y);
      bw.Write(Pos1Z);
      bw.Write(Pos2X);
      bw.Write(Pos2Y);
      bw.Write(Pos2Z);
      bw.Write(UnkZero);
      bw.Write(Spline.Count);
      foreach (var flDat in Spline)
      {
        flDat.Save(bw);
      }
    }
    public Vec3 GetEndPoint()
    {
      return Spline.First().TargetLanePosition;
    }
    public Vec3 GetStartPoint()
    {
      return Spline.Last().SourceLanePosition;
    }
    public bool CheckFlow()
    {
      for (int i = 0; i != Spline.Count-1; ++i)
      {
        if ((Spline[i].SourceLanePosition - Spline[i+1].TargetLanePosition).Length > 0.1)
          return false;
        var norm1 = Spline[i].SourceLaneMinusDeltaControlP;
        norm1.Normalize();
        var norm2 = Spline[i+1].TargetLaneDeltaControlPoint;
        norm2.Normalize();
        if ((norm1 - norm2).Length > 0.1)
          return false;
        var endP = GetEndPoint();
        var startP = GetStartPoint();
        var minStartP1 = IntersectionInFlattened_1.Crossings.Min(x => (x.LaneShape.TargetLanePosition - startP).Length);
        var minStartP2 = IntersectionInFlattened_2.Crossings.Min(x => (x.LaneShape.TargetLanePosition - startP).Length);
        var minEndP1 = IntersectionInFlattened_2.Crossings.Min(x => (x.LaneShape.SourceLanePosition - endP).Length);
        var minEndP2 = IntersectionInFlattened_1.Crossings.Min(x => (x.LaneShape.SourceLanePosition - endP).Length);
        if (minStartP1 > minStartP2)
          return false;
        if (minEndP1 > minEndP2)
          return false;
      }
      return true;
    }
    public void SetPositionToAvgFromFloatDatas()
    {
      var endP = GetEndPoint();
      var startP = GetStartPoint();
      Pos1X = (float)startP.X;
      Pos1Y = (float)startP.Y;
      Pos1Z = (float)startP.Z;
      Pos2X = (float)endP.X;
      Pos2Y = (float)endP.Y;
      Pos2Z = (float)endP.Z;
    }
    public void SetSplineLengths(int resolution = 10)
    {
      foreach (var spl in Spline)
      {
        spl.LengthOfSplineAtSource = spl.LengthOfSplineAtTarget = spl.GetLengthOfSpline(resolution);
      }
    }
  }
}
