using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class BinaryPolyVertexData : FileEntry
  {
    protected static readonly int offsetVertexCoordX = 0;
    protected static readonly int offsetVertexCoordY = 4;
    protected static readonly int offsetVertexCoordZ = 8;
    protected static readonly int offsetVertexNormalX = 12;
    protected static readonly int offsetVertexNormalY = 16;
    protected static readonly int offsetVertexNormalZ = 20;
    protected static readonly int offsetVertexColorR = 24;
    protected static readonly int offsetVertexColorG = 25;
    protected static readonly int offsetVertexColorB = 26;
    protected static readonly int offsetVertexColorA = 27;
    protected static readonly int offsetVertexIlluminationR = 28;
    protected static readonly int offsetVertexIlluminationG = 29;
    protected static readonly int offsetVertexIlluminationB = 30;
    protected static readonly int offsetVertexIlluminationA = 31;
    protected static readonly int offsetUVChannel1X = 32;
    protected static readonly int offsetUVChannel1Y = 36;
    protected static readonly int offsetUVChannel2X = 40;
    protected static readonly int offsetUVChannel2Y = 44;
    protected static readonly int offsetUVChannel3X = 48;
    protected static readonly int offsetUVChannel3Y = 52;

    public float VertexCoordX
    {
      get
      {
        return GetFloatFromFile(offsetVertexCoordX);
      }
      set
      {
        SetFloatInFile(value, offsetVertexCoordX);
      }
    }
    public float VertexCoordY
    {
      get
      {
        return GetFloatFromFile(offsetVertexCoordY);
      }
      set
      {
        SetFloatInFile(value, offsetVertexCoordY);
      }
    }
    public float VertexCoordZ
    {
      get
      {
        return GetFloatFromFile(offsetVertexCoordZ);
      }
      set
      {
        SetFloatInFile(value, offsetVertexCoordZ);
      }
    }
    public float VertexNormalX
    {
      get
      {
        return GetFloatFromFile(offsetVertexNormalX);
      }
      set
      {
        SetFloatInFile(value, offsetVertexNormalX);
      }
    }
    public float VertexNormalY
    {
      get
      {
        return GetFloatFromFile(offsetVertexNormalY);
      }
      set
      {
        SetFloatInFile(value, offsetVertexNormalY);
      }
    }
    public float VertexNormalZ
    {
      get
      {
        return GetFloatFromFile(offsetVertexNormalZ);
      }
      set
      {
        SetFloatInFile(value, offsetVertexNormalZ);
      }
    }
    public float UVChannel1X
    {
      get
      {
        return GetFloatFromFile(offsetUVChannel1X);
      }
      set
      {
        SetFloatInFile(value, offsetUVChannel1X);
      }
    }
    public float UVChannel1Y
    {
      get
      {
        return GetFloatFromFile(offsetUVChannel1Y);
      }
      set
      {
        SetFloatInFile(value, offsetUVChannel1Y);
      }
    }
    public float UVChannel2X
    {
      get
      {
        return GetFloatFromFile(offsetUVChannel2X);
      }
      set
      {
        SetFloatInFile(value, offsetUVChannel2X);
      }
    }
    public float UVChannel2Y
    {
      get
      {
        return GetFloatFromFile(offsetUVChannel2Y);
      }
      set
      {
        SetFloatInFile(value, offsetUVChannel2Y);
      }
    }
    public float UVChannel3X
    {
      get
      {
        return GetFloatFromFile(offsetUVChannel3X);
      }
      set
      {
        SetFloatInFile(value, offsetUVChannel3X);
      }
    }
    public float UVChannel3Y
    {
      get
      {
        return GetFloatFromFile(offsetUVChannel3Y);
      }
      set
      {
        SetFloatInFile(value, offsetUVChannel3Y);
      }
    }
    public byte VertexColorR
    {
      get
      {
        return GetByteFromFile(offsetVertexColorR);
      }
      set
      {
        SetByteInFile(value, offsetVertexColorR);
      }
    }
    public byte VertexColorG
    {
      get
      {
        return GetByteFromFile(offsetVertexColorG);
      }
      set
      {
        SetByteInFile(value, offsetVertexColorG);
      }
    }
    public byte VertexColorB
    {
      get
      {
        return GetByteFromFile(offsetVertexColorB);
      }
      set
      {
        SetByteInFile(value, offsetVertexColorB);
      }
    }
    public byte VertexColorA
    {
      get
      {
        return GetByteFromFile(offsetVertexColorA);
      }
      set
      {
        SetByteInFile(value, offsetVertexColorA);
      }
    }
    public byte VertexIlluminationR
    {
      get
      {
        return GetByteFromFile(offsetVertexIlluminationR);
      }
      set
      {
        SetByteInFile(value, offsetVertexIlluminationR);
      }
    }
    public byte VertexIlluminationG
    {
      get
      {
        return GetByteFromFile(offsetVertexIlluminationG);
      }
      set
      {
        SetByteInFile(value, offsetVertexIlluminationG);
      }
    }
    public byte VertexIlluminationB
    {
      get
      {
        return GetByteFromFile(offsetVertexIlluminationB);
      }
      set
      {
        SetByteInFile(value, offsetVertexIlluminationB);
      }
    }
    public byte VertexIlluminationA
    {
      get
      {
        return GetByteFromFile(offsetVertexIlluminationA);
      }
      set
      {
        SetByteInFile(value, offsetVertexIlluminationA);
      }
    }
    public override int Size
    {
      get
      {
        return 56;
      }
      set
      {
        throw new Exception("Can not set size of VertexData it should always be 56");
      }
    }

    public BinaryPolyVertexData(FileCacheHolder fileCache, int offset, bool cache = false)
    : base(fileCache, offset, cache)
    {

    }

    public override string ToString()
    {
      return VertexCoordX.ToString("F3") + "," + VertexCoordY.ToString("F3") + "," + VertexCoordZ.ToString("F3");
    }
  }
}
