using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class BinaryVertexV3 : FileEntry
  {
    private static readonly int offsetVertexCoordX = 0;
    private static readonly int offsetVertexCoordY = 4;
    private static readonly int offsetVertexCoordZ = 8;
    private static readonly int offsetVertexNormalX = 12;
    private static readonly int offsetVertexNormalY = 16;
    private static readonly int offsetVertexNormalZ = 20;
    private static readonly int offsetUVChannel1X = 24;
    private static readonly int offsetUVChannel1Y = 28;
    private static readonly int offsetUVChannel2X = 32;
    private static readonly int offsetUVChannel2Y = 36;
    private static readonly int offsetVertexColorB = 40;
    private static readonly int offsetVertexColorG = 41;
    private static readonly int offsetVertexColorR = 42;
    private static readonly int offsetVertexColorA = 43;
    private static readonly int offsetUnkown1 = 44;
    private static readonly int offsetUnkown2 = 48;
    private static readonly int offsetUVChannel3X = 52;
    private static readonly int offsetUVChannel3Y = 56;
    private static readonly int offsetUnkown3 = 60;

    private BinaryMeshV3 correspondingModelData;

    public bool IsVertexCoordXDefined
    {
      get
      {
        return Size >= offsetVertexCoordX + 4;
      }
    }
    public bool IsVertexCoordYDefined
    {
      get
      {
        return Size >= offsetVertexCoordY + 4;
      }
    }
    public bool IsVertexCoordZDefined
    {
      get
      {
        return Size >= offsetVertexCoordZ + 4;
      }
    }
    public bool IsVertexNormalXDefined
    {
      get
      {
        return Size >= offsetVertexNormalX + 4;
      }
    }
    public bool IsVertexNormalYDefined
    {
      get
      {
        return Size >= offsetVertexNormalY + 4;
      }
    }
    public bool IsVertexNormalZDefined
    {
      get
      {
        return Size >= offsetVertexNormalZ + 4;
      }
    }
    public bool IsUVChannel1XDefined
    {
      get
      {
        return Size >= offsetUVChannel1X + 4;
      }
    }
    public bool IsUVChannel1YDefined
    {
      get
      {
        return Size >= offsetUVChannel1Y + 4;
      }
    }
    public bool IsUVChannel2XDefined
    {
      get
      {
        return Size >= offsetUVChannel2X + 4;
      }
    }
    public bool IsUVChannel2YDefined
    {
      get
      {
        return Size >= offsetUVChannel2Y + 4;
      }
    }
    public bool IsVertexColorBDefined
    {
      get
      {
        return Size >= offsetVertexColorB + 1;
      }
    }
    public bool IsVertexColorRDefined
    {
      get
      {
        return Size >= offsetVertexColorR + 1;
      }
    }
    public bool IsVertexColorGDefined
    {
      get
      {
        return Size >= offsetVertexColorG + 1;
      }
    }
    public bool IsVertexColorADefined
    {
      get
      {
        return Size >= offsetVertexColorA + 1;
      }
    }
    public bool IsUnkown1Defined
    {
      get
      {
        return Size >= offsetUnkown1 + 4;
      }
    }
    public bool IsUnkown2Defined
    {
      get
      {
        return Size >= offsetUnkown2 + 4;
      }
    }
    public bool IsUVChannel3XDefined
    {
      get
      {
        return Size >= offsetUVChannel3X + 4;
      }
    }
    public bool IsUVChannel3YDefined
    {
      get
      {
        return Size >= offsetUVChannel3Y + 4;
      }
    }
    public bool IsUnkown3Defined
    {
      get
      {
        return Size >= offsetUnkown3 + 4;
      }
    }
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
    public int Unkown1
    {
      get
      {
        return GetIntFromFile(offsetUnkown1);
      }
      set
      {
        SetIntInFile(value, offsetUnkown1);
      }
    }
    public int Unkown2
    {
      get
      {
        return GetIntFromFile(offsetUnkown2);
      }
      set
      {
        SetIntInFile(value, offsetUnkown2);
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
    public int Unkown3
    {
      get
      {
        return GetIntFromFile(offsetUnkown3);
      }
      set
      {
        SetIntInFile(value, offsetUnkown3);
      }
    }
    public override int Size
    {
      get
      {
        return correspondingModelData.VerticesSize / correspondingModelData.VertexCount;
      }
      set
      {
        throw new Exception("Can not set size of VertexData it should always be = correspondingModelData.verticesSize / correspondingModelData.vertexCount");
      }
    }

    public BinaryVertexV3(BinaryMeshV3 correspondingModel, FileCacheHolder fileCache, int offset, bool cache = false)
    :base(fileCache,offset,cache)
    {
      correspondingModelData = correspondingModel;
    }

    public override string ToString()
    {
      return VertexCoordX.ToString("F3") + "," + VertexCoordY.ToString("F3") + "," + VertexCoordZ.ToString("F3");
    }
  }
}
