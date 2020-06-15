using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
    public class ScxV3Geometry
    {
        private SlrrLib.Model.BinaryScxV3 scxv3Mesh;

        public ScxV3Geometry(SlrrLib.Model.BinaryScxV3 model)
        {
            scxv3Mesh = model;
        }

        public Vector3D AvaragePosition()
        {
            Vector3D ret = new Vector3D(0, 0, 0);
            double weightSum = 0;
            foreach (var meshDat in scxv3Mesh.Models)
            {
                SlrrLib.Model.BinaryMeshV3 datV3 = meshDat as SlrrLib.Model.BinaryMeshV3;
                foreach (var vecPos in datV3.VertexDatas.Select(x => new Vector3D(x.VertexCoordX, x.VertexCoordY, x.VertexCoordZ)))
                {
                    weightSum++;
                    ret += vecPos;
                }
            }
            return ret / weightSum;
        }

        public IEnumerable<GeometryModel3D> WpfModels(int uvindex, int selectedIndex = -1)
        {
            List<GeometryModel3D> ret = new List<GeometryModel3D>();
            int meshInd = 0;
            foreach (var meshDat in scxv3Mesh.Models)
            {
                SlrrLib.Model.BinaryMeshV3 datV3 = meshDat as SlrrLib.Model.BinaryMeshV3;
                var toad = getModelFromModelDataV3(datV3, uvindex, meshInd == selectedIndex || selectedIndex == -1);
                ret.Add(toad);
                meshInd++;
            }
            return ret;
        }

        public GeometryModel3D WpfModelForIndex(int uvindex, int meshindex, int selectedIndex = -1)
        {
            return getModelFromModelDataV3(scxv3Mesh.Models.ElementAt(meshindex), uvindex, meshindex == selectedIndex || selectedIndex == -1);
        }

        private MeshGeometry3D getMeshFromModelDataV3(SlrrLib.Model.BinaryMeshV3 meshData, int uvindex)
        {
            MeshGeometry3D obj = new MeshGeometry3D();
            obj.Positions = new Point3DCollection(meshData.VertexDatas.Select(x => new Point3D(x.VertexCoordX, x.VertexCoordY, x.VertexCoordZ)));
            obj.TriangleIndices = new Int32Collection(meshData.VertexIndices);
            obj.Normals = new Vector3DCollection(meshData.VertexDatas.Select(x => new Vector3D(x.VertexNormalX, x.VertexNormalY, x.VertexNormalZ)));
            if (uvindex == 3 && meshData.VertexDatas.All(x => x.IsUVChannel3XDefined && x.IsUVChannel3YDefined))
                obj.TextureCoordinates = new PointCollection(meshData.VertexDatas.Select(x => new Point(x.UVChannel3X, x.UVChannel3Y)));
            else if (uvindex == 2 && meshData.VertexDatas.All(x => x.IsUVChannel2XDefined && x.IsUVChannel2YDefined))
                obj.TextureCoordinates = new PointCollection(meshData.VertexDatas.Select(x => new Point(x.UVChannel2X, x.UVChannel2Y)));
            else if (uvindex == 1 && meshData.VertexDatas.All(x => x.IsUVChannel1XDefined && x.IsUVChannel1YDefined))
                obj.TextureCoordinates = new PointCollection(meshData.VertexDatas.Select(x => new Point(x.UVChannel1X, x.UVChannel1Y)));
            else
                obj.TextureCoordinates = new PointCollection(meshData.VertexDatas.Select(x => new Point(0, 0)));
            return obj;
        }

        private GeometryModel3D getModelFromModelDataV3(SlrrLib.Model.BinaryMeshV3 meshData, int uvindex, bool selected = true)
        {
            ImageBrush imgBrush = new ImageBrush();

            if (selected)
                imgBrush.ImageSource = new BitmapImage(new Uri(@"grid.png", UriKind.Relative));
            else
                imgBrush.ImageSource = new BitmapImage(new Uri(@"grid_gray.png", UriKind.Relative));

            imgBrush.ViewportUnits = BrushMappingMode.Absolute;

            DiffuseMaterial material = new DiffuseMaterial();
            material.Brush = imgBrush;
            DiffuseMaterial backMaterial = new DiffuseMaterial();
            if (selected)
            {
                backMaterial.Brush = Brushes.DarkCyan;
            }
            else
            {
                Brush b = new SolidColorBrush(Colors.DarkBlue);
                b.Opacity = 0.7;
                backMaterial.Brush = b;
            }
            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = getMeshFromModelDataV3(meshData, uvindex);
            model.Material = material;
            model.BackMaterial = backMaterial;
            return model;
        }
    }
}