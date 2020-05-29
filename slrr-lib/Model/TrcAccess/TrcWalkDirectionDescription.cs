using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vec3 = System.Windows.Media.Media3D.Vector3D;

namespace SlrrLib.Model
{
  public class TrcWalkDirectionDescription
  {
    //these define a direction as (P2-P1).Normalize
    public float X1
    {
      get;
      set;
    }
    public float Y1
    {
      get;
      set;
    }
    public float X2
    {
      get;
      set;
    }
    public float Y2
    {
      get;
      set;
    }
    public int OtherWalkIndex
    {
      get;
      set;
    }
    public TrcWalk OtherWalk
    {
      get;
      set;
    }
    public Vec3 Pos1
    {
      get
      {
        return new Vec3(X1, 0, Y1);
      }
      set
      {
        X1 = (float)value.X;
        Y1 = (float)value.Z;
      }
    }
    public Vec3 Pos2
    {
      get
      {
        return new Vec3(X2, 0, Y2);
      }
      set
      {
        X2 = (float)value.X;
        Y2 = (float)value.Z;
      }
    }
  }
}
