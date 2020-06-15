using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
    public class InnerPhysGeometry
    {
        private SlrrLib.Model.BinaryInnerPhysEntry mesh;

        public static MeshGeometry3D GetMeshFromModelData(SlrrLib.Model.BinaryInnerPhysEntry curMesh)
        {
            MeshGeometry3D obj = new MeshGeometry3D();
            obj.Positions = new Point3DCollection(curMesh.Vetices.Select(x => new Point3D(x.VertexX, x.VertexY, x.VertexZ)));
            List<int> indices = new List<int>();
            Dictionary<int, Vector3D> normals = new Dictionary<int, Vector3D>();
            foreach (var firstChunk in curMesh.FacingProperties)
            {
                var norm = new Vector3D(firstChunk.NormalX, firstChunk.NormalY, firstChunk.NormalZ);
                indices.Add(firstChunk.TriIndex0);
                indices.Add(firstChunk.TriIndex1);
                indices.Add(firstChunk.TriIndex2);
                normals[firstChunk.TriIndex0] = norm;
                normals[firstChunk.TriIndex1] = norm;
                normals[firstChunk.TriIndex2] = norm;
            }
            obj.TriangleIndices = new Int32Collection(indices);
            obj.Normals = new Vector3DCollection(normals.OrderBy(x => x.Key).Select(x => x.Value));
            obj.TextureCoordinates = new PointCollection(curMesh.Vetices.Select(x => new Point(0.5, 0.5)));
            return obj;
        }

        public static GeometryModel3D GetModelFromModelData(SlrrLib.Model.BinaryInnerPhysEntry curMesh)
        {
            Brush bFront = new SolidColorBrush(Colors.Green);
            DiffuseMaterial material = new DiffuseMaterial();
            material.Brush = bFront;
            DiffuseMaterial backMaterial = new DiffuseMaterial();
            Brush b = new SolidColorBrush(Colors.DarkBlue);
            b.Opacity = 0.7;
            backMaterial.Brush = b;
            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = GetMeshFromModelData(curMesh);
            model.Material = material;
            model.BackMaterial = backMaterial;
            return model;
        }

        public InnerPhysGeometry(SlrrLib.Model.BinaryInnerPhysEntry model)
        {
            mesh = model;
        }

        public IEnumerable<GeometryModel3D> WpfModels()
        {
            List<GeometryModel3D> ret = new List<GeometryModel3D>();
            int meshInd = 0;
            var toad = GetModelFromModelData(mesh);
            ret.Add(toad);
            meshInd++;
            return ret;
        }

        public Vector3D AvaragePosition()
        {
            Vector3D ret = new Vector3D(0, 0, 0);
            double weightSum = 0;
            foreach (var meshDat in mesh.Vetices)
            {
                weightSum++;
                ret.X += meshDat.VertexX;
                ret.Y += meshDat.VertexY;
                ret.Z += meshDat.VertexZ;
            }
            if (weightSum == 0)
                weightSum = 1;
            return ret / weightSum;
        }

        public bool HasVertexInBound(float maxX, float maxY, float maxZ, float minX, float minY, float minZ)
        {
            return mesh.Vetices.Any(x =>
                                    x.VertexX > minX && x.VertexX < maxX &&
                                    x.VertexY > minY && x.VertexY < maxY &&
                                    x.VertexZ > minZ && x.VertexZ < maxZ);
        }

        public bool IsInBound(float maxX, float maxY, float maxZ, float minX, float minY, float minZ)
        {
            float realMinX = mesh.Vetices.Min(x => x.VertexX);
            float realMinY = mesh.Vetices.Min(x => x.VertexY);
            float realMinZ = mesh.Vetices.Min(x => x.VertexZ);
            float realMaxX = mesh.Vetices.Max(x => x.VertexX);
            float realMaxY = mesh.Vetices.Max(x => x.VertexY);
            float realMaxZ = mesh.Vetices.Max(x => x.VertexZ);
            return realMaxX < maxX && realMaxY < maxY && realMaxZ < maxZ &&
                   realMinX > minX && realMinY > minY && realMinZ > minZ;
        }
    }
}