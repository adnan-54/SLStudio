using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
  public class ScxV4Geometry
  {
    private SlrrLib.Model.BinaryScxV4 scxv4Mesh;

    public ScxV4Geometry(SlrrLib.Model.BinaryScxV4 model)
    {
      scxv4Mesh = model;
    }

    public IEnumerable<GeometryModel3D> WpfModels(int uvindex)
    {
      List<GeometryModel3D> ret = new List<GeometryModel3D>();
      int meshInd = 0;
      foreach (var meshDat in scxv4Mesh.FaceDefs)
      {
        var toad = getModelFromModelDataV4(meshDat, uvindex);
        ret.Add(toad);
        meshInd++;
      }
      return ret;
    }
    public GeometryModel3D WpfModelForIndex(int uvindex, int meshindex)
    {
      return getModelFromModelDataV4(scxv4Mesh.FaceDefs.ElementAt(meshindex), uvindex);
    }
    public Vector3D AvaragePosition()
    {
      Vector3D ret = new Vector3D(0, 0, 0);
      double weightSum = 0;
      foreach (var meshDat in scxv4Mesh.MeshDatas)
      {
        foreach (var vecPos in meshDat.Positions)
        {
          weightSum++;
          ret.X += vecPos.X;
          ret.Y += vecPos.Y;
          ret.Z += vecPos.Z;
        }
      }
      return ret / weightSum;
    }

    private MeshGeometry3D getMeshFromModelDataV4(SlrrLib.Model.BinaryFaceDefV4 faceData, int uvindex)
    {
      var meshData = scxv4Mesh.VertexDataOfFaceDef(faceData);
      MeshGeometry3D obj = new MeshGeometry3D();
      obj.Positions = new Point3DCollection(meshData.Positions.Select(x => new Point3D(x.X, x.Y, x.Z)));
      obj.TriangleIndices = new Int32Collection(faceData.GetShorts.Select(x => (int)x));
      obj.Normals = new Vector3DCollection(meshData.Normals.Select(x => new Vector3D(x.X, x.Y, x.Z)));
      if (uvindex == 3 && meshData.IsUV3Defined)
        obj.TextureCoordinates = new PointCollection(meshData.UV3s.Select(x => new Point(x.U, x.V)));
      if (uvindex == 2 && meshData.IsUV2Defined)
        obj.TextureCoordinates = new PointCollection(meshData.UV2s.Select(x => new Point(x.U, x.V)));
      if (uvindex == 1 && meshData.IsUV1Defined)
        obj.TextureCoordinates = new PointCollection(meshData.UV1s.Select(x => new Point(x.U, x.V)));
      else
        obj.TextureCoordinates = new PointCollection(meshData.Positions.Select(x => new Point(0, 0)));
      return obj;
    }
    private GeometryModel3D getModelFromModelDataV4(SlrrLib.Model.BinaryFaceDefV4 meshData, int uvindex,bool selected = true)
    {
      ImageBrush imgBrush = new ImageBrush();

      if(selected)
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
      model.Geometry = getMeshFromModelDataV4(meshData, uvindex);
      model.Material = material;
      model.BackMaterial = backMaterial;
      return model;
    }
  }
}
