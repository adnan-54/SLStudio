using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public struct BasicXYZ
  {
    public float X;
    public float Y;
    public float Z;

    public override string ToString()
    {
      return X.ToString("F3") + ", " + Y.ToString("F3") + ", " + Z.ToString("F3");
    }

    public float DistanceSqrFrom(BasicXYZ other)
    {
      float x = other.X - X;
      float y = other.Y - Y;
      float z = other.Z - Z;
      return (float)(x * x + y * y + z * z);
    }
  }
}
