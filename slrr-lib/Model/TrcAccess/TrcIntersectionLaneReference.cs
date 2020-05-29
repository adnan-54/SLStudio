using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class TrcIntersectionLaneReference
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
    public byte IsInboundLane
    {
      get;
      set;
    }

    public static TrcIntersectionLaneReference Load(BinaryReader br)
    {
      TrcIntersectionLaneReference ret = new TrcIntersectionLaneReference();
      ret.LaneIndInFlattened = br.ReadInt32();
      ret.IsInboundLane = br.ReadByte();
      return ret;
    }

    public void Save(BinaryWriter bw)
    {
      bw.Write(LaneIndInFlattened);
      bw.Write(IsInboundLane);
    }
  }
}
