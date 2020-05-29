using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class DynamicBoneWeightsV4
  {
    private float w1;
    private float w2;
    private float w3;
    private float w4;

    public bool IsW1Defined
    {
      get;
      set;
    }
    public bool IsW2Defined
    {
      get;
      set;
    }
    public bool IsW3Defined
    {
      get;
      set;
    }
    public bool IsW4Defined
    {
      get;
      set;
    }
    public bool IsBoneIndRefDefined
    {
      get;
      set;
    }
    public byte Ind1
    {
      get;
      set;
    }
    public byte Ind2
    {
      get;
      set;
    }
    public byte Ind3
    {
      get;
      set;
    }
    public byte Ind4
    {
      get;
      set;
    }
    public float W1
    {
      get
      {
        return w1;
      }
      set
      {
        w1 = value;
        IsW1Defined = true;
      }
    }
    public float W2
    {
      get
      {
        return w2;
      }
      set
      {
        IsW2Defined = true;
        w2 = value;
      }
    }
    public float W3
    {
      get
      {
        return w3;
      }
      set
      {
        IsW3Defined = true;
        w3 = value;
      }
    }
    public float W4
    {
      get
      {
        return w4;
      }
      set
      {
        IsW4Defined = true;
        w4 = value;
      }
    }
  }
}
