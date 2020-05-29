using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Vec3 = System.Windows.Media.Media3D.Vector3D;

namespace SlrrLib.Model
{
  public class TrcLane : TrcObject
  {
    public int RaceNodeIndInFlattened
    {
      get;
      set;
    }
    public TrcNavigatorNode RaceNodeInFlattened_1
    {
      get;
      set;
    }
    public int ZeroableLaneIndInFlattened_1
    {
      get;
      set;
    }
    public TrcLane LaneInFlattened_1
    {
      get;
      set;
    }
    public byte AreIndicesRefsInReverseAsInTheRaceNode
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
    public int IndexOfMeInParentIntersection_1
    {
      get;
      set;
    }
    public int IndexOfMeInParentIntersection_2
    {
      get;
      set;
    }
    public float MinXOfPseudoSelf
    {
      get;
      set;
    }
    public float MaxXOfPseudoSelf
    {
      get;
      set;
    }
    public float MaxxTrueOfPseudoSelf
    {
      get;
      set;
    } = 0;
    public int PseudoSelfIndexInLaneArray
    {
      get;
      set;
    }
    public int ZeroableIndInFlattened_1
    {
      get;
      set;
    }
    public TrcObject InFlattened_1
    {
      get;
      set;
    } = null;
    public int ZeroableIndInFlattened_2
    {
      get;
      set;
    }
    public TrcObject InFlattened_2
    {
      get;
      set;
    } = null;
    public int ZeroableLaneIndInFlattened_2
    {
      get;
      set;
    }
    public TrcLane LeftAdjacentLane
    {
      get;
      set;
    }
    public int ZeroableLaneIndInFlattened_3
    {
      get;
      set;
    }
    public TrcLane RightAdjacentLane
    {
      get;
      set;
    }
    public float MaxSpeed
    {
      get;
      set;
    } = 13.8888f;
    public float EndFloatData_2
    {
      get;
      set;
    } = 0.9f;
    public int MinusOne
    {
      get;
      set;
    } = -1;
    public List<TrcSplineSegment> Spline
    {
      get;
      set;
    } = new List<TrcSplineSegment>();//should be count long
    public Vec3 EndPoint
    {
      get
      {
        return Spline.First().TargetLanePosition;
      }
      set
      {
        Spline.First().TargetLanePosition = value;
      }
    }
    public Vec3 StartPoint
    {
      get
      {
        return Spline.Last().SourceLanePosition;
      }
      set
      {
        Spline.Last().SourceLanePosition = value;
      }
    }
    public Vec3 EndNormal
    {
      get
      {
        return Spline.First().TargetLaneDeltaControlPoint;
      }
      set
      {
        Spline.First().TargetLaneDeltaControlPoint = value;
      }
    }
    public Vec3 StartNormal
    {
      get
      {
        return Spline.Last().SourceLaneMinusDeltaControlP;
      }
      set
      {
        Spline.Last().SourceLaneMinusDeltaControlP = value;
      }
    }

    public static TrcLane Load(BinaryReader br)
    {
      TrcLane ret = new TrcLane();
      ret.RaceNodeIndInFlattened = br.ReadInt32();
      ret.ZeroableLaneIndInFlattened_1 = br.ReadInt32();
      ret.AreIndicesRefsInReverseAsInTheRaceNode = br.ReadByte();
      ret.Pos1X = br.ReadSingle();
      ret.Pos1Y = br.ReadSingle();
      ret.Pos1Z = br.ReadSingle();
      ret.Pos2X = br.ReadSingle();
      ret.Pos2Y = br.ReadSingle();
      ret.Pos2Z = br.ReadSingle();

      ret.IndexOfMeInParentIntersection_1 = br.ReadInt32();
      ret.IndexOfMeInParentIntersection_2 = br.ReadInt32();
      ret.MinXOfPseudoSelf = br.ReadSingle();
      ret.MaxXOfPseudoSelf = br.ReadSingle();
      if(!Trc.loadPrevVersion)
        ret.MaxxTrueOfPseudoSelf = br.ReadSingle();
      ret.PseudoSelfIndexInLaneArray = br.ReadInt32();
      if (!Trc.loadPrevVersion)
      {
        ret.ZeroableIndInFlattened_1 = br.ReadInt32();
        ret.ZeroableIndInFlattened_2 = br.ReadInt32();
        ret.ZeroableLaneIndInFlattened_2 = br.ReadInt32();
        ret.ZeroableLaneIndInFlattened_3 = br.ReadInt32();

        ret.MaxSpeed = br.ReadSingle();
        ret.EndFloatData_2 = br.ReadSingle();
        ret.MinusOne = br.ReadInt32();
      }
      int count = br.ReadInt32();
      ret.Spline = new List<TrcSplineSegment>();
      for (int i = 0; i != count; ++i)
      {
        ret.Spline.Add(TrcSplineSegment.Load(br));
      }
      return ret;
    }

    public void SplitSplSegmentAtParam(TrcSplineSegment spl, float t)
    {
      if (spl == null)
        return;
      int indexOfSpl = Spline.IndexOf(spl);
      var newP = spl.EvaluateAtParam(t);
      var toadSpl = new TrcSplineSegment();
      toadSpl.UnkByte_1 = 1;
      toadSpl.SourceLanePosition = spl.SourceLanePosition;
      toadSpl.SourceLaneMinusDeltaControlP = spl.SourceLaneMinusDeltaControlP;
      toadSpl.TargetLanePosition = newP;
      var normal = spl.SourceLanePosition - spl.TargetLanePosition;
      normal.Normalize();
      toadSpl.TargetLaneDeltaControlPoint = normal;
      spl.SourceLanePosition = newP;
      spl.SourceLaneMinusDeltaControlP = normal;
      Spline.Insert(indexOfSpl + 1, toadSpl);
    }
    public float GetSpeedDistortionDivider()
    {
      float ret = (MaxSpeed/15.0f - 13.8888f/15.0f)+1.0f;
      if (ret < 1.0f)
        ret = 1.0f;
      return ret;
    }
    public void SetProperAdjacencyOrder()
    {
      var curLinAppr = EndPoint - StartPoint;
      var adj1 = LeftAdjacentLane;
      var adj2 = RightAdjacentLane;
      RightAdjacentLane = null;
      LeftAdjacentLane = null;
      if (adj1 != null)
      {
        var refLinAppr = adj1.EndPoint - StartPoint;
        if (Vec3.CrossProduct(refLinAppr, curLinAppr).Y > 0.0)
        {
          RightAdjacentLane = adj1;
        }
        else
        {
          LeftAdjacentLane = adj1;
        }
      }
      if (adj2 != null)
      {
        var refLinAppr = adj2.EndPoint - StartPoint;
        if (Vec3.CrossProduct(refLinAppr, curLinAppr).Y > 0.0)
        {
          if (RightAdjacentLane != null)
            throw new Exception("Lane slot already set a lane can only have one adjacent lane on each side");
          RightAdjacentLane = adj2;
        }
        else
        {
          if (LeftAdjacentLane != null)
            throw new Exception("Lane slot already set a lane can only have one adjacent lane on each side");
          LeftAdjacentLane = adj2;
        }
      }
    }
    public void Save(BinaryWriter bw)
    {
      bw.Write(RaceNodeIndInFlattened);
      bw.Write(ZeroableLaneIndInFlattened_1);
      bw.Write(AreIndicesRefsInReverseAsInTheRaceNode);
      bw.Write(Pos1X);
      bw.Write(Pos1Y);
      bw.Write(Pos1Z);
      bw.Write(Pos2X);
      bw.Write(Pos2Y);
      bw.Write(Pos2Z);
      bw.Write(IndexOfMeInParentIntersection_1);
      bw.Write(IndexOfMeInParentIntersection_2);
      bw.Write(MinXOfPseudoSelf);
      bw.Write(MaxXOfPseudoSelf);
      bw.Write(MaxxTrueOfPseudoSelf);
      bw.Write(PseudoSelfIndexInLaneArray);
      bw.Write(ZeroableIndInFlattened_1);
      bw.Write(ZeroableIndInFlattened_2);
      bw.Write(ZeroableLaneIndInFlattened_2);
      bw.Write(ZeroableLaneIndInFlattened_3);
      bw.Write(MaxSpeed);
      bw.Write(EndFloatData_2);
      bw.Write(MinusOne);
      bw.Write(Spline.Count);
      foreach (var flDat in Spline)
      {
        flDat.Save(bw);
      }
    }
    public void SetPositionToAvgFromFloatDatas()
    {
      Pos1X = (float)StartPoint.X;
      Pos1Y = (float)StartPoint.Y;
      Pos1Z = (float)StartPoint.Z;
      Pos2X = (float)EndPoint.X;
      Pos2Y = (float)EndPoint.Y;
      Pos2Z = (float)EndPoint.Z;
    }
    public TrcLane Copy()
    {
      TrcLane ret = new TrcLane();
      ret.RaceNodeIndInFlattened = RaceNodeIndInFlattened;
      ret.RaceNodeInFlattened_1 = RaceNodeInFlattened_1;
      ret.ZeroableLaneIndInFlattened_1 = ZeroableLaneIndInFlattened_1;
      ret.LaneInFlattened_1 = LaneInFlattened_1;
      ret.AreIndicesRefsInReverseAsInTheRaceNode = AreIndicesRefsInReverseAsInTheRaceNode;
      ret.Pos1X = Pos1X;
      ret.Pos1Y = Pos1Y;
      ret.Pos1Z = Pos1Z;
      ret.Pos2X = Pos2X;
      ret.Pos2Y = Pos2Y;
      ret.Pos2Z = Pos2Z;
      ret.IndexOfMeInParentIntersection_1 = IndexOfMeInParentIntersection_1;
      ret.IndexOfMeInParentIntersection_2 = IndexOfMeInParentIntersection_2;
      ret.MinXOfPseudoSelf = MinXOfPseudoSelf;
      ret.MaxXOfPseudoSelf = MaxXOfPseudoSelf;
      ret.MaxxTrueOfPseudoSelf = MaxxTrueOfPseudoSelf;
      ret.PseudoSelfIndexInLaneArray = PseudoSelfIndexInLaneArray;
      ret.ZeroableIndInFlattened_1 = ZeroableIndInFlattened_1;
      ret.InFlattened_1 = InFlattened_1;
      ret.ZeroableIndInFlattened_2 = ZeroableIndInFlattened_2;
      ret.InFlattened_2 = InFlattened_2;
      ret.ZeroableLaneIndInFlattened_2 = ZeroableLaneIndInFlattened_2;
      ret.LeftAdjacentLane = LeftAdjacentLane;
      ret.ZeroableLaneIndInFlattened_3 = ZeroableLaneIndInFlattened_3;
      ret.RightAdjacentLane = RightAdjacentLane;
      ret.MaxSpeed = MaxSpeed;
      ret.EndFloatData_2 = EndFloatData_2;
      ret.MinusOne = MinusOne;
      ret.Spline = Spline.Select(x => x.Copy()).ToList();
      return ret;
    }
    public TrcLane ReverseLaneInPlace()
    {
      foreach (var spl in Spline)
      {
        TrcSplineSegment splBak = spl.Copy();
        spl.SourceLaneMinusDeltaControlP = -splBak.TargetLaneDeltaControlPoint;
        spl.SourceLanePosition = splBak.TargetLanePosition;
        spl.LengthOfSplineAtSource = splBak.LengthOfSplineAtTarget;
        spl.TargetLaneDeltaControlPoint = -splBak.SourceLaneMinusDeltaControlP;
        spl.TargetLanePosition = splBak.SourceLanePosition;
        spl.LengthOfSplineAtTarget = splBak.LengthOfSplineAtSource;
      }
      Spline.Reverse();
      return this;
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
