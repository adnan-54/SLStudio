using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class SplNode
  {
    public float x;
    public float y;
    public float z;

    public SplNode(float x,float y,float z)
    {
      this.x = x;
      this.y = y;
      this.z = z;
    }
    public SplNode()
    {
    }
    public SplNode(SplNode other)
    {
      x = other.x;
      y = other.y;
      z = other.z;
    }

    public float Length()
    {
      return (float)Math.Sqrt(x * x + y * y + z * z);
    }
    public void Normalize()
    {
      float l = (float)Math.Sqrt(x * x + y * y + z * z);
      if (l == 0)
        return;
      x /= l;
      y /= l;
      z /= l;
    }
    public void Abs()
    {
      x *= Math.Sign(x);
      y *= Math.Sign(y);
      z *= Math.Sign(z);
    }
    public void CopyFrom(SplNode other)
    {
      x = other.x;
      y = other.y;
      z = other.z;
    }

    public static SplNode operator +(SplNode c1, SplNode c2)
    {
      return new SplNode(c1.x + c2.x,c1.y + c2.y,c1.z + c2.z);
    }
    public static SplNode operator -(SplNode c1, SplNode c2)
    {
      return new SplNode(c1.x - c2.x, c1.y - c2.y, c1.z - c2.z);
    }
    public static SplNode operator *(SplNode c1, float c2)
    {
      return new SplNode(c1.x * c2, c1.y * c2, c1.z * c2);
    }
    public static SplNode operator *(float c1, SplNode c2)
    {
      return c2*c1;
    }

    public override string ToString()
    {
      return x.ToString("F4") + " " + y.ToString("F4") + " " + z.ToString("F4");
    }
  }
}
