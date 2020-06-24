using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
    public class NativeGeometryScxContext
    {
        public string scxFnam;
        public Vector3D pos;
        public Vector3D ypr;

        public NativeGeometryScxContext(string scxFnam, Vector3D pos, Vector3D ypr)
        {
            this.pos = pos;
            this.scxFnam = scxFnam;
            this.ypr = ypr;
        }
    }
}