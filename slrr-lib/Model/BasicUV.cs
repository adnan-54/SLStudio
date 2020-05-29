using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public struct BasicUV
  {
    public float U;
    public float V;

    public override string ToString()
    {
      return U.ToString("F3") + ", " + V.ToString("F3");
    }
  }
}
