using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using Vec3 = System.Windows.Media.Media3D.Vector3D;

namespace SlrrLib.Model
{
  public class TrcWalk
  {
    private static readonly bool loadPrevVersion = false;

    private int firstChunkCount;
    private int relatedWalkIndicesCount;

    public Vec3 SpatialPosition
    {
      get;
      set;
    }
    public Vec3 GlobalPavementPos
    {
      get;
      set;
    }
    /// <summary>
    /// Slope only counts if there are not 2 type23 adjacencies
    /// (only 2 or 0 type23 adjacency seen no 1 or >2)
    /// </summary>
    public float Slope1
    {
      get;
      set;
    }//no one side slope
    public float Slope2
    {
      get;
      set;
    }//slope only present if pos are present (!=0 means present here)
    public List<TrcWalkSurfaceSegment> AdjacencyDirections
    {
      get;
      set;
    } = new List<TrcWalkSurfaceSegment>();
    public List<int> RelatedWalkIndices
    {
      get;
      set;
    } = new List<int>();
    public List<TrcWalk> RelatedWalks
    {
      get;
      set;
    } = new List<TrcWalk>();

    public static string EncodeWalkData(byte[] walkData)
    {
      string dict = "ABCDEFGHIJKLMNOP";
      StringBuilder sb = new StringBuilder();
      foreach (byte dat in walkData)
      {
        sb.Append(dict[(dat / 0x10) & 0xf]);
        sb.Append(dict[dat & 0xf]);
      }
      var byts = BitConverter.GetBytes(walkData.Length);
      return dict[(byts[0] / 0x10) & 0xf].ToString() + dict[byts[0]& 0xf].ToString() +
             dict[(byts[1] / 0x10) & 0xf].ToString() + dict[byts[1]& 0xf].ToString() +
             dict[(byts[2] / 0x10) & 0xf].ToString() + dict[byts[2]& 0xf].ToString() +
             dict[(byts[3] / 0x10) & 0xf].ToString() + dict[byts[3]& 0xf].ToString() + sb.ToString();
    }
    public static byte[] DecodeWalkData(string walk)
    {
      byte[] lengthData = new byte[4];
      var bytes = ASCIIEncoding.ASCII.GetBytes(walk);
      for (int i = 0; i != 8; i += 2)
      {
        lengthData[i/2] = (byte)((byte)((bytes[i] + 15) << 4) | (byte)(bytes[i + 1] - 65));
      }
      int length = BitConverter.ToInt32(lengthData, 0);
      byte[] ret = new byte[length];
      for (int i = 8; i < bytes.Length; i += 2)
      {
        ret[(i-8)/2] = (byte)((byte)((bytes[i] + 15) << 4) | (byte)(bytes[i + 1] - 65));
      }
      return ret;
    }
    public static TrcWalk LoadXML(string fnam)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(TrcWalk));
      StreamReader reader = new StreamReader(fnam);
      TrcWalk ret = (TrcWalk)serializer.Deserialize(reader);
      reader.Close();
      return ret;
    }

    public TrcWalk(string encodedWalk)
    {
      var byts = DecodeWalkData(encodedWalk);
      int offset = 0;
      float x = BitConverter.ToSingle(byts, offset);
      offset += 4;
      float z = BitConverter.ToSingle(byts, offset);
      offset += 4;

      Slope1 = BitConverter.ToSingle(byts, offset);
      offset += 4;
      Slope2 = BitConverter.ToSingle(byts, offset);
      offset += 4;
      float y = BitConverter.ToSingle(byts, offset);
      offset += 4;

      GlobalPavementPos = new Vec3
      {
        X = x,
        Y = y,
        Z = z
      };

      firstChunkCount = BitConverter.ToInt32(byts, offset);
      offset += 4;

      for (int i = 0; i != firstChunkCount; ++i)
      {
        int FirstChunkType = 21;
        if (!TrcWalk.loadPrevVersion)
        {
          FirstChunkType = BitConverter.ToInt32(byts, offset);
          offset += 4;
        }
        TrcWalkSurfaceSegment toad = new TrcWalkSurfaceSegment();
        toad.Type = FirstChunkType;
        if (FirstChunkType == 23)
        {
          toad.CountOfPoints = BitConverter.ToInt32(byts, offset);
          offset += 4;

          for (int j = 0; j != toad.CountOfPoints+1; ++j)
          {
            TrcWalkControlPoint3D toadPos = new TrcWalkControlPoint3D();
            toadPos.Position = readVec3(byts, offset);
            offset += 12;
            toadPos.Normal = readVec3(byts, offset);
            offset += 12;
            toad.ControlPoints.Add(toadPos);
          }
        }
        toad.Direction = readCommonChunk(byts, offset);
        offset += 20;
        AdjacencyDirections.Add(toad);
      }

      relatedWalkIndicesCount = BitConverter.ToInt32(byts, offset);
      offset += 4;

      for (int i = 0; i != relatedWalkIndicesCount; ++i)
      {
        RelatedWalkIndices.Add(BitConverter.ToInt32(byts, offset));
        offset += 4;
      }
    }
    public TrcWalk()
    {

    }

    public TrcWalkSurfaceSegment GetFirstSplineAdjacency()
    {
      return AdjacencyDirections.FirstOrDefault(x => x.Type == 23);
    }
    public TrcWalkSurfaceSegment GetLastSplineAdjacency()
    {
      return AdjacencyDirections.LastOrDefault(x => x.Type == 23);
    }
    public TrcWalkSurfaceSegment GetNextSplineAdjacency(TrcWalkSurfaceSegment current)
    {
      int cur_i = AdjacencyDirections.IndexOf(current);
      if (cur_i == -1)
        return null;
      if (!HasType23Adjacency())
        return null;
      cur_i++;
      cur_i %= AdjacencyDirections.Count;
      while (AdjacencyDirections[cur_i].Type != 23)
      {
        cur_i++;
        cur_i %= AdjacencyDirections.Count;
      }
      return AdjacencyDirections[cur_i];
    }
    public TrcWalkSurfaceSegment GetPrevSplineAdjacency(TrcWalkSurfaceSegment current)
    {
      int cur_i = AdjacencyDirections.IndexOf(current);
      if (cur_i == -1)
        return null;
      if (!HasType23Adjacency())
        return null;
      cur_i--;
      if(cur_i == -1)
        cur_i = AdjacencyDirections.Count-1;
      while (AdjacencyDirections[cur_i].Type != 23)
      {
        cur_i--;
        if(cur_i == -1)
          cur_i = AdjacencyDirections.Count-1;
      }
      return AdjacencyDirections[cur_i];
    }
    public bool HasType23Adjacency()
    {
      return AdjacencyDirections.Any(x => x.Type == 23);
    }
    public bool IsTriangleLike(double minSideSize = 0.0001)
    {
      if (AdjacencyDirections.Count == 3)
        return true;
      foreach (var adj in AdjacencyDirections)
      {
        float XDiff = adj.Direction.X2 - adj.Direction.X1;
        float YDiff = adj.Direction.Y2 - adj.Direction.Y1;
        if (Math.Sqrt(XDiff * XDiff + YDiff * YDiff) <= minSideSize)
        {
          return true;
        }
      }
      return false;
    }
    public TrcWalk GetFirstAdjacent()
    {
      var adj = AdjacencyDirections.FirstOrDefault(x => x.Type == 22);
      if (adj == null)
        return null;
      return adj.Direction.OtherWalk;
    }
    public TrcWalk GetSecondAdjacent()
    {
      TrcWalk adj = null;
      foreach (var dir in AdjacencyDirections)
      {
        if (dir.Type == 22 && adj == null)
          adj = dir.Direction.OtherWalk;
        else if (dir.Type == 22)
          return dir.Direction.OtherWalk;
      }
      return null;
    }
    public Vec3 GetGlobalSourcePosition(TrcWalkSurfaceSegment seg)
    {
      if (HasType23Adjacency())
      {
        if (seg.Type != 23)
        {
          return new Vec3(seg.Direction.X1,
                          GetNextSplineAdjacency(seg).ControlPoints.Last().Position.Y,
                          seg.Direction.Y1);
        }
        else
        {
          return seg.ControlPoints.First().Position;
        }
      }
      else
      {
        Vec3 LocalPavementPos = GlobalPavementPos;
        return new Vec3(seg.Direction.X1, 0, seg.Direction.Y1)+LocalPavementPos;
      }
    }
    public Vec3 GetGlobalTargetPosition(TrcWalkSurfaceSegment seg)
    {
      if (HasType23Adjacency())
      {
        if (seg.Type != 23)
        {
          return new Vec3(seg.Direction.X2,
                          GetNextSplineAdjacency(seg).ControlPoints.Last().Position.Y,
                          seg.Direction.Y2);
        }
        else
        {
          return seg.ControlPoints.Last().Position;
        }
      }
      else
      {
        Vec3 LocalPavementPos = GlobalPavementPos;
        return new Vec3(seg.Direction.X2, 0, seg.Direction.Y2)+LocalPavementPos;
      }
    }
    public void SetGlobalSourcePoint(Vec3 value,TrcWalkSurfaceSegment seg)
    {
      if (HasType23Adjacency())
      {
        if (seg.Type != 23)
        {
          seg.Direction.X1 = (float)value.X;
          var localControlP = GetPrevSplineAdjacency(seg).ControlPoints.First();
          localControlP.Position = new Vec3(localControlP.Position.X,value.Y,localControlP.Position.Z);
          seg.Direction.Y1 = (float)value.Z;
        }
        else
        {
          seg.Direction.X1 = (float)value.X;
          seg.Direction.Y1 = (float)value.Z;
          seg.ControlPoints.First().Position = value;
        }
      }
      else
      {
        Vec3 LocalPavementPos = GlobalPavementPos;
        Vec3 local = value - LocalPavementPos;
        seg.Direction.X1 = (float)local.X;
        seg.Direction.Y1 = (float)local.Z;
        GlobalPavementPos = new Vec3(GlobalPavementPos.X,value.Y,GlobalPavementPos.Z);
      }
    }
    public void SetGlobalTargetPoint(Vec3 value,TrcWalkSurfaceSegment seg)
    {
      if (HasType23Adjacency())
      {
        if (seg.Type != 23)
        {
          seg.Direction.X2 = (float)value.X;
          var localControlP = GetNextSplineAdjacency(seg).ControlPoints.Last();
          localControlP.Position = new Vec3(localControlP.Position.X,value.Y,localControlP.Position.Z);
          seg.Direction.Y2 = (float)value.Z;
        }
        else
        {
          seg.Direction.X2 = (float)value.X;
          seg.Direction.Y2 = (float)value.Z;
          seg.ControlPoints.Last().Position = value;
        }
      }
      else
      {
        Vec3 LocalPavementPos = GlobalPavementPos;
        Vec3 local = value - LocalPavementPos;
        seg.Direction.X2 = (float)local.X;
        seg.Direction.Y2 = (float)local.Z;
        GlobalPavementPos = new Vec3(GlobalPavementPos.X,value.Y,GlobalPavementPos.Z);
      }
    }
    public void RecalcPosition()
    {
      Vec3 avgPos = new Vec3();
      if (HasType23Adjacency())
      {
        var frst = GetFirstSplineAdjacency();
        var scnd = GetLastSplineAdjacency();
        avgPos = (frst.EvaluateMiddleSpline(0.5f) + scnd.EvaluateMiddleSpline(0.5f)) * 0.5;
        SpatialPosition = avgPos+new Vec3(0,1.0,0);
      }
      else
      {
        double div = 0;
        foreach (var adj in AdjacencyDirections)
        {
          avgPos += GetGlobalSourcePosition(adj);
          avgPos += GetGlobalTargetPosition(adj);
          div += 2.0;
        }
        avgPos /= div;
        var trans = avgPos - GlobalPavementPos;
        trans.Y = 0;
        GlobalPavementPos += trans;
        foreach (var adj in AdjacencyDirections)
        {
          adj.Direction.Pos1 -= trans;
          adj.Direction.Pos2 -= trans;
        }
      }

    }
    public void SaveXMLAs(string fnam)
    {
      XmlSerializer x = new XmlSerializer(typeof(TrcWalk));
      TextWriter writer = new StreamWriter(fnam);
      x.Serialize(writer, this);
      writer.Close();
    }
    public string AsEncoded()
    {
      List<byte[]> uglybuff = new List<byte[]>();
      uglybuff.Add(BitConverter.GetBytes((float)GlobalPavementPos.X));
      uglybuff.Add(BitConverter.GetBytes((float)GlobalPavementPos.Z));
      uglybuff.Add(BitConverter.GetBytes(Slope1));
      uglybuff.Add(BitConverter.GetBytes(Slope2));
      uglybuff.Add(BitConverter.GetBytes((float)GlobalPavementPos.Y));
      uglybuff.Add(BitConverter.GetBytes(AdjacencyDirections.Count));
      foreach (var dat in AdjacencyDirections)
      {
        uglybuff.Add(BitConverter.GetBytes(dat.Type));
        if (dat.Type == 23)
        {
          uglybuff.Add(BitConverter.GetBytes(dat.ControlPoints.Count-1));
          foreach (var point in dat.ControlPoints)
          {
            point.Normal.Normalize();
            uglybuff.Add(BitConverter.GetBytes((float)point.Position.X));
            uglybuff.Add(BitConverter.GetBytes((float)point.Position.Y));
            uglybuff.Add(BitConverter.GetBytes((float)point.Position.Z));
            uglybuff.Add(BitConverter.GetBytes((float)point.Normal.X));
            uglybuff.Add(BitConverter.GetBytes((float)point.Normal.Y));
            uglybuff.Add(BitConverter.GetBytes((float)point.Normal.Z));
          }
        }
        uglybuff.Add(BitConverter.GetBytes((float)dat.Direction.X1));
        uglybuff.Add(BitConverter.GetBytes((float)dat.Direction.Y1));
        uglybuff.Add(BitConverter.GetBytes((float)dat.Direction.X2));
        uglybuff.Add(BitConverter.GetBytes((float)dat.Direction.Y2));
        uglybuff.Add(BitConverter.GetBytes(dat.Direction.OtherWalkIndex));
      }
      uglybuff.Add(BitConverter.GetBytes(RelatedWalkIndices.Count));
      foreach (var intRef in RelatedWalkIndices)
      {
        uglybuff.Add(BitConverter.GetBytes(intRef));
      }
      return EncodeWalkData(uglybuff.SelectMany(x => x).ToArray());
    }

    private Vec3 readVec3(byte[] byts, int offset)
    {
      return new Vec3(BitConverter.ToSingle(byts, offset),
                      BitConverter.ToSingle(byts, offset+4),
                      BitConverter.ToSingle(byts, offset+8));
    }
    private TrcWalkDirectionDescription readCommonChunk(byte[] byts, int offset)
    {
      return new TrcWalkDirectionDescription()
      {
        X1 = BitConverter.ToSingle(byts, offset),
        Y1 = BitConverter.ToSingle(byts, offset+4),
        X2 = BitConverter.ToSingle(byts, offset+8),
        Y2 = BitConverter.ToSingle(byts, offset+12),
        OtherWalkIndex = BitConverter.ToInt32(byts, offset+16)
      };
    }
  }
}
