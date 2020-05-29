using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public enum TrcCrossingType
  {
    StraightNonInterfering = 0,
    TurnRightOrNonInterferingTurn = 1,
    InterferingOrLaneCrossingTurn = -1
  }
  public class TrcIntersectionLane
  {
    public byte IndOfFROMLaneInParnet
    {
      get;
      set;
    }
    public byte IndOfTOLaneInParnet
    {
      get;
      set;
    }
    public short LanePriority
    {
      get;
      set;
    }
    public int OnlyZeroSeen
    {
      get;
      set;
    }
    public TrcCrossingType CrossingDescription
    {
      get;
      set;
    } = 0;
    public TrcSplineSegment LaneShape
    {
      get;
      set;
    }

    public static TrcIntersectionLane Load(BinaryReader br)
    {
      TrcIntersectionLane ret = new TrcIntersectionLane();
      ret.IndOfFROMLaneInParnet = br.ReadByte();
      ret.IndOfTOLaneInParnet = br.ReadByte();
      ret.LanePriority = br.ReadInt16();
      ret.OnlyZeroSeen = br.ReadInt32();
      if(!Trc.loadPrevVersion)
        ret.CrossingDescription = (TrcCrossingType)br.ReadInt32();
      ret.LaneShape = TrcSplineSegment.Load(br);
      return ret;
    }

    public void Save(BinaryWriter bw)
    {
      bw.Write(IndOfFROMLaneInParnet);
      bw.Write(IndOfTOLaneInParnet);
      bw.Write(LanePriority);
      bw.Write(OnlyZeroSeen);
      bw.Write((int)CrossingDescription);
      LaneShape.Save(bw);
    }
    public override string ToString()
    {
      return "Crossing lane " + CrossingDescription.ToString();
    }
  }
}
