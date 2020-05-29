using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
  public class VRML97GeomExport
  {
    private StringBuilder shapesSB = new StringBuilder();
    private const string shapePattern = "Shape\r\n{\r\nappearance Appearance\r\n{\r\nmaterial Material\r\n{\r\ndiffuseColor 0.5882 0.5882 0.5882\r\nambientIntensity 1.0\r\nspecularColor 0 0 0\r\nshininess 0.145\r\ntransparency 0\r\n}\r\n}\r\ngeometry IndexedFaceSet\r\n{\r\nccw TRUE\r\nsolid TRUE\r\ncoord Coordinate\r\n{\r\npoint\r\n[\r\n#COORDS#\r\n]\r\n}\r\nnormal Normal\r\n{\r\nvector\r\n[\r\n#NORMALS\r\n]\r\n}\r\nnormalPerVertex TRUE\r\ntexCoord TextureCoordinate\r\n{\r\npoint\r\n[\r\n#UVPOINTS\r\n]\r\n}\r\ncoordIndex\r\n[\r\n#INDICES#\r\n]\r\n}\r\n}";
    private const string shapePatternSimple = "Shape {\r\nappearance Appearance {\r\nmaterial Material {\r\ndiffuseColor 0.5882 0.5882 0.5882\r\nambientIntensity 1.0\r\nspecularColor 0 0 0\r\nshininess 0.145\r\ntransparency 0\r\n}\r\n}\r\ngeometry DEF #NAME# IndexedFaceSet {\r\nccw TRUE\r\nsolid TRUE\r\ncoord DEF RandomName1-COORD Coordinate { point [#COORDS#]\r\n}\r\ncoordIndex [#INDICES#]\r\n}\r\n}";
    private const string transformPattern = "#VRML V2.0 utf8\r\n\r\n# Produced by 3D Studio MAX VRML97 exporter, Version 5.01, Revision 0.33\r\n# Date: Thu Aug 24 01:07:32 2017\r\n\r\nDEF RandomName1 Transform {\r\ntranslation 0 0 0\r\nchildren [\r\n#SHAPES#\r\n]\r\n}";

    public VRML97GeomExport()
    {

    }

    public void PushModel(NamedModel model)
    {
      if(model == null)
        return;
      Transform3DGroup transform3DGroup = new Transform3DGroup();
      transform3DGroup.Children.Add(new YprRotation3D(model.Ypr).TransformValues);
      transform3DGroup.Children.Add(new TranslateTransform3D(model.Translate));
      if (model is NamedScxModel scxv3Model && scxv3Model.Scxv3Source != null)
      {
        var transCoords = scxv3Model.Scxv3Source.Meshes[scxv3Model.MeshIndex].VertexDatas.Select(x => transform3DGroup.Transform(new Vector3D(x.VertexCoordX, x.VertexCoordY, x.VertexCoordZ))+new Vector3D(transform3DGroup.Value.OffsetX,transform3DGroup.Value.OffsetY,transform3DGroup.Value.OffsetZ));
        StringBuilder coords = new StringBuilder();
        foreach (var x in transCoords)
        {
          coords.AppendLine(", ");
          coords.Append(x.X.ToString("F8") + " " + x.Y.ToString("F8") + " " + x.Z.ToString("F8"));
        }
        coords.Remove(0, 2);
        StringBuilder normals = new StringBuilder();
        foreach (var x in scxv3Model.Scxv3Source.Meshes[scxv3Model.MeshIndex].VertexDatas)
        {
          normals.AppendLine(", ");
          normals.Append(x.VertexNormalX.ToString("F8") + " " + x.VertexNormalY.ToString("F8") + " " + x.VertexNormalZ.ToString("F8"));
        }
        normals.Remove(0, 2);
        StringBuilder uvs = new StringBuilder();
        foreach (var x in scxv3Model.Scxv3Source.Meshes[scxv3Model.MeshIndex].VertexDatas)
        {
          uvs.AppendLine(", ");
          uvs.Append(x.UVChannel1X.ToString("F8") + " " + x.UVChannel1Y.ToString("F8"));
        }
        uvs.Remove(0, 2);
        StringBuilder indices = new StringBuilder();
        for (int i = 0; i != scxv3Model.Scxv3Source.Meshes[scxv3Model.MeshIndex].VertexIndices.Count; i++ )
        {
          indices.Append(", "+scxv3Model.Scxv3Source.Meshes[scxv3Model.MeshIndex].VertexIndices[i].ToString("D"));
          if (((i + 1) % 3) == 0)
          {
            indices.AppendLine(", -1");
            while (shouldSkippNexTriangle(scxv3Model.Scxv3Source.Meshes[scxv3Model.MeshIndex].VertexIndices,
                                          i + 1, scxv3Model.Scxv3Source.Meshes[scxv3Model.MeshIndex].VertexDatas))
            {
              i += 3;
            }
          }
        }
        indices = indices.Remove(0, 2);
        string modelName = model.Name.Replace(":", "").Replace(")", "").Replace("(", "");
        if (modelName.Contains(" "))
          modelName = modelName.Remove(modelName.IndexOf(" "));
        shapesSB.Append(shapePattern.Replace("#NAME#", modelName)
                        .Replace("#COORDS#", coords.ToString())
                        .Replace("#INDICES#", indices.ToString())
                        .Replace("#UVPOINTS", uvs.ToString())
                        .Replace("#NORMALS", normals.ToString()));
      }
      if (model is NamedScxModel scxv4Model && scxv4Model.Scxv4Source != null)
      {
        var transCoords = scxv4Model.Scxv4Source.Meshes[scxv4Model.MeshIndex].VertexData.VertexDataList.Select(x => transform3DGroup.Transform(new Vector3D(x.Position.X, x.Position.Y, x.Position.Z)) + new Vector3D(transform3DGroup.Value.OffsetX, transform3DGroup.Value.OffsetY, transform3DGroup.Value.OffsetZ));
        StringBuilder coords = new StringBuilder();
        foreach(var x in transCoords)
        {
          coords.AppendLine(", ");
          coords.Append(x.X.ToString("F8") + " " + x.Y.ToString("F8") + " " + x.Z.ToString("F8"));
        }
        coords.Remove(0, 2);
        StringBuilder normals = new StringBuilder();
        foreach (var x in scxv4Model.Scxv4Source.Meshes[scxv4Model.MeshIndex].VertexData.VertexDataList)
        {
          normals.AppendLine(", ");
          normals.Append(x.Normal.X.ToString("F8") + " " + x.Normal.Y.ToString("F8") + " " + x.Normal.Z.ToString("F8"));
        }
        normals.Remove(0, 2);
        StringBuilder uvs = new StringBuilder();
        foreach (var x in scxv4Model.Scxv4Source.Meshes[scxv4Model.MeshIndex].VertexData.VertexDataList)
        {
          uvs.AppendLine(", ");
          uvs.Append(x.Uv1.U.ToString("F8") + " " + x.Uv1.V.ToString("F8"));
        }
        uvs.Remove(0, 2);
        StringBuilder indices = new StringBuilder();
        for (int i = 0; i != scxv4Model.Scxv4Source.Meshes[scxv4Model.MeshIndex].FaceDef.Indices.Count; i++)
        {
          indices.Append(", " + scxv4Model.Scxv4Source.Meshes[scxv4Model.MeshIndex].FaceDef.Indices[i].ToString("D"));
          if (((i+1) % 3) == 0)
          {
            indices.AppendLine(", -1");
            while (shouldSkippNexTriangle(scxv4Model.Scxv4Source.Meshes[scxv4Model.MeshIndex].FaceDef.Indices,
                                          i + 1, scxv4Model.Scxv4Source.Meshes[scxv4Model.MeshIndex].VertexData.VertexDataList))
            {
              i += 3;
            }
          }
        }
        indices = indices.Remove(0, 2);
        string modelName = model.Name.Replace(":", "").Replace(")", "").Replace("(", "");
        if (modelName.Contains(" "))
          modelName = modelName.Remove(modelName.IndexOf(" "));
        shapesSB.Append(shapePattern.Replace("#NAME#", modelName)
                        .Replace("#COORDS#", coords.ToString())
                        .Replace("#INDICES#", indices.ToString())
                        .Replace("#UVPOINTS", uvs.ToString())
                        .Replace("#NORMALS", normals.ToString()));
      }
      if (model is NamedPhysModel physModel && physModel.PhysSource != null)
      {
        var transCoords = physModel.PhysSource.Vetices.Select(x => transform3DGroup.Transform(new Vector3D(x.VertexX, x.VertexY, x.VertexZ)) + new Vector3D(transform3DGroup.Value.OffsetX, transform3DGroup.Value.OffsetY, transform3DGroup.Value.OffsetZ));
        StringBuilder coords = new StringBuilder();
        foreach (var x in transCoords)
        {
          coords.Append(", ");
          coords.Append(x.X.ToString("F8") + " " + x.Y.ToString("F8") + " " + x.Z.ToString("F8"));
        }
        coords.Remove(0, 2);
        StringBuilder indices = new StringBuilder();
        foreach (var firstCh in physModel.PhysSource.FacingProperties)
        {
          indices.Append("," + firstCh.TriIndex0.ToString() +
                         "," + firstCh.TriIndex1.ToString() +
                         "," + firstCh.TriIndex2.ToString() +
                         ", -1");
        }
        indices = indices.Remove(0, 2);
        string modelName = physModel.Name.Replace(":", "").Replace(")", "").Replace("(", "");
        if (modelName.Contains(" "))
          modelName = modelName.Remove(modelName.IndexOf(" "));
        shapesSB.Append(shapePatternSimple.Replace("#NAME#", modelName).Replace("#COORDS#", coords.ToString()).Replace("#INDICES#", indices.ToString()));
      }
      if (model is NamedPolyModel polyModel && polyModel.PolySource != null)
      {
        var transCoords = polyModel.PolySource.Meshes.ElementAt(polyModel.MeshIndex).Vertices.Select(x => transform3DGroup.Transform(new Vector3D(x.VertexCoordX, x.VertexCoordY, x.VertexCoordZ)) + new Vector3D(transform3DGroup.Value.OffsetX, transform3DGroup.Value.OffsetY, transform3DGroup.Value.OffsetZ));
        StringBuilder coords = new StringBuilder();
        foreach (var x in transCoords)
        {
          coords.Append(", ");
          coords.Append(x.X.ToString("F8") + " " + x.Y.ToString("F8") + " " + x.Z.ToString("F8"));
        }
        coords.Remove(0, 2);
        StringBuilder indices = new StringBuilder();
        var indexLst = polyModel.PolySource.Meshes.ElementAt(polyModel.MeshIndex).Indices.ToList();
        int indexCount = indexLst.Count;
        for (int i = 0; i != indexCount; i++)
        {
          indices.Append(", " + indexLst[i].ToString("D"));
          if (((i + 1) % 3) == 0)
          {
            indices.Append(", -1");
          }
        }
        indices = indices.Remove(0, 2);
        string modelName = polyModel.Name.Replace(":","").Replace(")", "").Replace("(", "");
        if (modelName.Contains(" "))
          modelName = modelName.Remove(modelName.IndexOf(" "));
        shapesSB.Append(shapePatternSimple.Replace("#NAME#", modelName).Replace("#COORDS#", coords.ToString()).Replace("#INDICES#", indices.ToString()));
      }
    }
    public void WriteFullExport(string fnam)
    {
      File.WriteAllText(fnam, transformPattern.Replace("#SHAPES#", shapesSB.ToString()));
    }

    private bool shouldSkippNexTriangle(List<int> indices, int currentPos,List<SlrrLib.Model.DynamicVertexV3> vertices)
    {
      if (indices.Count <= currentPos + 3)
        return false;
      for (int i = 0; i < currentPos; i+=3)
      {
        int matchFound = 0;
        for (int tri_i = 0; tri_i != 3; tri_i++)
        {
          for (int tri_j = 0; tri_j != 3; tri_j++)
          {
            float xDiff = Math.Abs(vertices[indices[i + tri_i]].VertexCoordX - vertices[indices[currentPos + tri_j]].VertexCoordX);
            float yDiff = Math.Abs(vertices[indices[i + tri_i]].VertexCoordY - vertices[indices[currentPos + tri_j]].VertexCoordY);
            float zDiff = Math.Abs(vertices[indices[i + tri_i]].VertexCoordZ - vertices[indices[currentPos + tri_j]].VertexCoordZ);
            if (xDiff < 0.001f &&
                yDiff < 0.001f &&
                zDiff < 0.001f)
            {
              matchFound++;
              break;
            }
          }
          if (matchFound==3)
            return true;
        }
      }
      return false;
    }
    private bool shouldSkippNexTriangle(List<ushort> indices, int currentPos,List<SlrrLib.Model.DynamicCompleteVertexDataV4> vertices)
    {
      if (indices.Count <= currentPos + 3)
        return false;
      for (int i = 0; i < currentPos; i+=3)
      {
        int matchFound = 0;
        for (int tri_i = 0; tri_i != 3; tri_i++)
        {
          for (int tri_j = 0; tri_j != 3; tri_j++)
          {
            float xDiff = Math.Abs(vertices[indices[i + tri_i]].Position.X - vertices[indices[currentPos + tri_j]].Position.X);
            float yDiff = Math.Abs(vertices[indices[i + tri_i]].Position.Y - vertices[indices[currentPos + tri_j]].Position.Y);
            float zDiff = Math.Abs(vertices[indices[i + tri_i]].Position.Z - vertices[indices[currentPos + tri_j]].Position.Z);
            if (xDiff < 0.001f &&
                yDiff < 0.001f &&
                zDiff < 0.001f)
            {
              matchFound++;
              break;
            }
          }
          if (matchFound==3)
            return true;
        }
      }
      return false;
    }
  }
}
