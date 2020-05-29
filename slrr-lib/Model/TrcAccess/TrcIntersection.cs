using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class TrcIntersection : TrcObject
  {
    public List<TrcIntersectionLaneReference> LaneRefs
    {
      get;
      set;
    } = new List<TrcIntersectionLaneReference>();
    public List<TrcIntersectionLane> Crossings
    {
      get;
      set;
    } = new List<TrcIntersectionLane>();
    public float PositionXData
    {
      get;
      set;
    }
    public float PositionYData
    {
      get;
      set;
    }
    public float PositionZData
    {
      get;
      set;
    }
    public int TerminatingZero
    {
      get;
      private set;
    }

    public static TrcIntersection Load(BinaryReader br)
    {
      TrcIntersection ret = new TrcIntersection();
      int count1 = br.ReadInt32();
      ret.LaneRefs = new List<TrcIntersectionLaneReference>();
      for (int i = 0; i != count1; ++i)
      {
        ret.LaneRefs.Add(TrcIntersectionLaneReference.Load(br));
      }
      int count2 = br.ReadInt32();
      ret.Crossings = new List<TrcIntersectionLane>();
      for (int i = 0; i != count2; ++i)
      {
        ret.Crossings.Add(TrcIntersectionLane.Load(br));
      }
      ret.PositionXData = br.ReadSingle();
      ret.PositionYData = br.ReadSingle();
      ret.PositionZData = br.ReadSingle();
      ret.TerminatingZero = br.ReadInt32();
      return ret;
    }

    public void Save(BinaryWriter bw)
    {
      bw.Write(LaneRefs.Count);
      foreach (var dat in LaneRefs)
      {
        dat.Save(bw);
      }
      bw.Write(Crossings.Count);
      foreach (var flDat in Crossings)
      {
        flDat.Save(bw);
      }
      bw.Write(PositionXData);
      bw.Write(PositionYData);
      bw.Write(PositionZData);
      bw.Write(TerminatingZero);
    }
    public void SetPositionToAvgFromFloatDatas()
    {
      PositionXData = 0;
      PositionYData = 0;
      PositionZData = 0;
      float count = 0;
      foreach (var fltDat in Crossings)
      {
        PositionXData += (float)fltDat.LaneShape.SourceLanePosition.X;
        PositionYData += (float)fltDat.LaneShape.SourceLanePosition.Y;
        PositionZData += (float)fltDat.LaneShape.SourceLanePosition.Z;
        PositionXData += (float)fltDat.LaneShape.TargetLanePosition.X;
        PositionYData += (float)fltDat.LaneShape.TargetLanePosition.Y;
        PositionZData += (float)fltDat.LaneShape.TargetLanePosition.Z;
        count += 2;
      }
      if (count != 0)
      {
        PositionXData /= count;
        PositionYData /= count;
        PositionZData /= count;
      }
    }
    public IEnumerable<TrcSplineSegment> GetAllFloatDatas()
    {
      return Crossings.Select(x => x.LaneShape);
    }
    public void SetSplineLengths(int resolution = 10)
    {
      foreach (var cross in Crossings)
      {
        cross.LaneShape.LengthOfSplineAtSource = cross.LaneShape.LengthOfSplineAtTarget = cross.LaneShape.GetLengthOfSpline(resolution);
      }
    }
  }
}
