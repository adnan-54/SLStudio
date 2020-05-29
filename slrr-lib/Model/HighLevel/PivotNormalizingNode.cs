using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SlrrLib.Model.HighLevel
{
  public class PivotNormalizingNode
  {
    private Vector3D pivotCache = new Vector3D(0, 0, 0);
    private bool pivotCacheSet = false;
    private ScxCache cache;

    public string ScxFnam
    {
      get;
      set;
    }
    public HashSet<Cfg> Cfgs
    {
      get;
      set;
    }

    public PivotNormalizingNode(ScxCache scxCache)
    {
      cache = scxCache;
      Cfgs = new HashSet<Cfg>();
    }
    public PivotNormalizingNode(ScxCache scxCache,string ScxFnam)
    {
      cache = scxCache;
      Cfgs = new HashSet<Cfg>();
      this.ScxFnam = ScxFnam;
    }

    public Vector3D GetMedianPivot()
    {
      if(!pivotCacheSet)
      {
        pivotCache = recalcMedianPivot();
      }
      return pivotCache;
    }
    public void TranslateSCXsAndCorrespondingSlot()
    {
      Vector3D translate = -GetMedianPivot();
      var scx = cache.GetSCXFromFileName(ScxFnam);
      if (scx is BinaryScxV3)
      {
        var scxv3 = new DynamicScxV3(scx as BinaryScxV3);
        foreach (var mesh in scxv3.Meshes)
        {
          foreach (var vert in mesh.VertexDatas)
          {
            vert.VertexCoordX += (float)translate.X;
            vert.VertexCoordY += (float)translate.Y;
            vert.VertexCoordZ += (float)translate.Z;
          }
        }
        scxv3.SaveAs(scx.FileName);
      }
      else if (scx is BinaryScxV4)
      {
        var scxv4 = new DynamicScxV4(scx as BinaryScxV4);
        foreach (var mesh in scxv4.Meshes)
        {
          foreach (var vert in mesh.VertexData.VertexDataList)
          {
            vert.Position = new BasicXYZ
            {
              X = vert.Position.X +(float)translate.X,
              Y = vert.Position.Y +(float)translate.Y,
              Z = vert.Position.Z +(float)translate.Z
            };
          }
        }
        scxv4.SaveAs(scx.FileName);
      }
      foreach(var cfg in Cfgs)
      {
        foreach(var posLine in cfg.LinesWithPositionData)
        {
          posLine.LineX += (float)translate.X/100.0f;
          posLine.LineY += (float)translate.Y/100.0f;
          posLine.LineZ += (float)translate.Z/100.0f;
        }
        cfg.Save();
      }
    }
    private Vector3D recalcMedianPivot()
    {
      Vector3D ret = new Vector3D(0, 0, 0);
      Vector3D curAvg = new Vector3D(0, 0, 0);
      double count = 0;
      var scx = cache.GetSCXFromFileName(ScxFnam);
      if (scx is BinaryScxV3)
      {
        var scxv3 = scx as BinaryScxV3;
        foreach (var mesh in scxv3.Models)
        {
          count++;
          double vertNumNorm = mesh.VertexCount;
          foreach (var vert in mesh.VertexDatas)
          {
            curAvg.X += vert.VertexCoordX / vertNumNorm;
            curAvg.Y += vert.VertexCoordY / vertNumNorm;
            curAvg.Z += vert.VertexCoordZ / vertNumNorm;
          }
        }
      }
      else if (scx is BinaryScxV4)
      {
        var scxv4 = scx as BinaryScxV4;
        foreach (var mesh in scxv4.MeshDatas)
        {
          count++;
          double vertNumNorm = mesh.NumberOfVertices;
          foreach (var vert in mesh.FullVertexDataList)
          {
            curAvg.X += vert.Position.X / vertNumNorm;
            curAvg.Y += vert.Position.Y / vertNumNorm;
            curAvg.Z += vert.Position.Z / vertNumNorm;
          }
        }
      }
      curAvg.X /= count;
      curAvg.Y /= count;
      curAvg.Z /= count;
      return curAvg;
    }
  }
}
