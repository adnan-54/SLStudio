using Vec3 = System.Windows.Media.Media3D.Vector3D;

namespace SlrrLib.Model
{
    public class TrcWalkControlPoint3D
    {
        public Vec3 Position
        {
            get;
            set;
        }

        public Vec3 Normal
        {
            get;
            set;
        }

        public TrcWalkControlPoint3D()
        {
        }

        public TrcWalkControlPoint3D(Vec3 p, Vec3 n)
        {
            Position = p;
            Normal = n;
        }
    }
}