using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
    public enum NamedModelSource
    {
        BodyLine,
        Mesh,
        Click,//wont retrive
        Deformable//wont retrive
    }

    public class NamedModel
    {
        public GeometryModel3D ModelGeom;
        public string Name;
        public NamedModelSource SourceOfModel;
        public Vector3D Translate;
        public Vector3D Ypr;

        public override string ToString()
        {
            return Name;
        }
    }
}