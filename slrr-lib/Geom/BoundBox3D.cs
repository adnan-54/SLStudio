namespace SlrrLib.Geom
{
  public class BoundBox3D
  {
    public readonly static BoundBox3D MaxBound = new BoundBox3D(float.MaxValue,float.MaxValue,float.MaxValue,float.MinValue,float.MinValue,float.MinValue);

    public float MaxX
    {
      get;
      private set;
    }
    public float MaxY
    {
      get;
      private set;
    }
    public float MaxZ
    {
      get;
      private set;
    }
    public float MinX
    {
      get;
      private set;
    }
    public float MinY
    {
      get;
      private set;
    }
    public float MinZ
    {
      get;
      private set;
    }

    public BoundBox3D(float maxX,float maxY,float maxZ,float minX,float minY,float minZ)
    {
      MaxX = maxX;
      MaxY = maxY;
      MaxZ = maxZ;
      MinX = minX;
      MinY = minY;
      MinZ = minZ;
    }
    public BoundBox3D(BoundBox3D other)
    {
      MaxX = other.MaxX;
      MaxY = other.MaxY;
      MaxZ = other.MaxZ;
      MinX = other.MinX;
      MinY = other.MinY;
      MinZ = other.MinZ;
    }

    public bool IsPositionInside(float x,float y,float z)
    {
      return x <= MaxX && x >= MinX &&
             y <= MaxY && y >= MinY &&
             z <= MaxZ && z >= MinZ;
    }
  }
}
