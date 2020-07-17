using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
    public class InnerPolyGeometry
    {
        private SlrrLib.Model.BinaryInnerPolyEntry mesh;

        public static MeshGeometry3D GetMeshFromModelData(SlrrLib.Model.BinaryInnerPolyMesh curMesh)
        {
            MeshGeometry3D obj = new MeshGeometry3D();
            obj.Positions = new Point3DCollection(curMesh.Vertices.Select(x => new Point3D(x.VertexCoordX, x.VertexCoordY, x.VertexCoordZ)));
            obj.TriangleIndices = new Int32Collection(curMesh.Indices);
            obj.Normals = new Vector3DCollection(curMesh.Vertices.Select(x => new Vector3D(x.VertexNormalX, x.VertexNormalY, x.VertexNormalZ)));
            obj.TextureCoordinates = new PointCollection(curMesh.Vertices.Select(x => new Point(x.UVChannel1X, x.UVChannel1Y)));
            //obj.TextureCoordinates = new PointCollection(curMesh.Vertices.Select(x => new Point(Math.Max(0.001f,Math.Min(x.VertexColorR,0.999f)), 0)));
            return obj;
        }

        public static GeometryModel3D GetModelFromModelData(SlrrLib.Model.BinaryInnerPolyMesh curMesh)
        {
            Brush bFront = new SolidColorBrush(Colors.LightCyan);
            DiffuseMaterial material = new DiffuseMaterial();
            material.Brush = bFront;
            DiffuseMaterial backMaterial = new DiffuseMaterial();
            Brush b = new SolidColorBrush(Colors.DarkRed);
            b.Opacity = 0.7;
            backMaterial.Brush = b;
            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = GetMeshFromModelData(curMesh);
            model.Material = material;
            model.BackMaterial = backMaterial;
            return model;
        }

        public InnerPolyGeometry(SlrrLib.Model.BinaryInnerPolyEntry model)
        {
            mesh = model;
        }

        public IEnumerable<GeometryModel3D> GetWpfModels()
        {
            List<GeometryModel3D> ret = new List<GeometryModel3D>();
            int meshInd = 0;
            foreach (var meshData in mesh.Meshes)
            {
                var toad = GetModelFromModelData(meshData);
                ret.Add(toad);
                meshInd++;
            }
            return ret;
        }

        public Vector3D AvaragePosition()
        {
            Vector3D ret = new Vector3D(0, 0, 0);
            double weightSum = 0;
            foreach (var meshDat in mesh.Meshes)
            {
                foreach (var vecPos in meshDat.Vertices)
                {
                    weightSum++;
                    ret.X += vecPos.VertexCoordX;
                    ret.Y += vecPos.VertexCoordY;
                    ret.Z += vecPos.VertexCoordZ;
                }
            }
            return ret / weightSum;
        }

        public bool HasVertexInBound(float maxX, float maxY, float maxZ, float minX, float minY, float minZ)
        {
            return mesh.Meshes.Any(x => x.Vertices.Any(y =>
                                   y.VertexCoordX > minX && y.VertexCoordX < maxX &&
                                   y.VertexCoordY > minY && y.VertexCoordY < maxY &&
                                   y.VertexCoordZ > minZ && y.VertexCoordZ < maxZ
                                                      ));
        }

        public bool IsInBound(float maxX, float maxY, float maxZ, float minX, float minY, float minZ)
        {
            float realMinX = mesh.Meshes.Min(x => x.Vertices.Min(y => y.VertexCoordX));
            float realMinY = mesh.Meshes.Min(x => x.Vertices.Min(y => y.VertexCoordY));
            float realMinZ = mesh.Meshes.Min(x => x.Vertices.Min(y => y.VertexCoordZ));
            float realMaxX = mesh.Meshes.Max(x => x.Vertices.Min(y => y.VertexCoordX));
            float realMaxY = mesh.Meshes.Max(x => x.Vertices.Min(y => y.VertexCoordY));
            float realMaxZ = mesh.Meshes.Max(x => x.Vertices.Min(y => y.VertexCoordZ));
            return realMaxX < maxX && realMaxY < maxY && realMaxZ < maxZ &&
                   realMinX > minX && realMinY > minY && realMinZ > minZ;
        }
    }
}