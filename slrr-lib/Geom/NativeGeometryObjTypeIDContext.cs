using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
  public class NativeGeometryObjTypeIDContext
  {
    public int typeID;
    public Vector3D pos;
    public Vector3D ypr;

    public NativeGeometryObjTypeIDContext(int typeID, Vector3D pos, Vector3D ypr)
    {
      this.pos = pos;
      this.typeID = typeID;
      this.ypr = ypr;
    }
  }
}
