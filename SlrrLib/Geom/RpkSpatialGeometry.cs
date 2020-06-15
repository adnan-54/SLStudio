using SlrrLib.Model;
using SlrrLib.Model.HighLevel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
    public abstract class RpkSpatialGeometry : GeometryBuilder
    {
        protected const double boxWidth = 1.0;
        protected Vector3D boxDiff = new Vector3D(boxWidth / 2.0, 0, 0);
        protected string rpkFilename;
        protected StringBuilder spatialDepthDescription = new StringBuilder();

        public string SlrrRoot
        {
            get;
            protected set;
        }

        public int LastSpatialTraceDepth
        {
            get;
            protected set;
        } = 0;

        public DynamicRpk Rpk
        {
            get;
            protected set;
        }

        protected RpkSpatialGeometry(DynamicRpk rpkDat, string rpkFilename)
        {
            Rpk = rpkDat;
            if (Rpk == null)
                Rpk = new DynamicRpk();
            this.rpkFilename = rpkFilename;
            SlrrRoot = GameFileManager.GetSLRRRoot(rpkFilename);
        }

        protected RpkSpatialGeometry(string rpkFilename)
        {
            Rpk = new DynamicRpk(new BinaryRpk(rpkFilename, true));
            if (Rpk == null)
                Rpk = new DynamicRpk();
            this.rpkFilename = rpkFilename;
            SlrrRoot = GameFileManager.GetSLRRRoot(rpkFilename);
        }

        public IEnumerable<GeometryModel3D> SpatialRepresentationForObject(DynamicNamedSpatialData target)
        {
            spatialDepthDescription = new StringBuilder();
            List<GeometryModel3D> ret = new List<GeometryModel3D>();
            Dictionary<DynamicNamedSpatialData, DynamicNamedSpatialData> childToParent =
              new Dictionary<DynamicNamedSpatialData, DynamicNamedSpatialData>();
            bool found = false;
            foreach (var res in Rpk.Entries)
            {
                foreach (var rsd in res.RSD.InnerEntries)
                {
                    if (rsd is SlrrLib.Model.DynamicSpatialNode)
                    {
                        var unkDat = rsd as SlrrLib.Model.DynamicSpatialNode;
                        foreach (var futureCh in unkDat.DataArray)
                        {
                            childToParent[futureCh] = null;
                        }
                        List<SlrrLib.Model.DynamicSpatialNode> toProc = new List<SlrrLib.Model.DynamicSpatialNode>();
                        toProc.Add(unkDat);
                        while (toProc.Any())
                        {
                            unkDat = toProc.Last();
                            toProc.RemoveAt(toProc.Count - 1);
                            foreach (var named in unkDat.DataArray)
                            {
                                foreach (var child in named.OwnedEntries)
                                {
                                    if (child is SlrrLib.Model.DynamicSpatialNode)
                                    {
                                        var arr = child as SlrrLib.Model.DynamicSpatialNode;
                                        toProc.Add(arr);
                                        foreach (var futureCh in arr.DataArray)
                                        {
                                            childToParent[futureCh] = named;
                                        }
                                    }
                                }
                                if (named == target)
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (found)
                        break;
                }
                if (found)
                    break;
            }
            if (found)
            {
                DynamicNamedSpatialData ch = target;
                int depth = 0;
                while (ch != null)
                {
                    spatialDepthDescription.Append(ch.Name);
                    spatialDepthDescription.Append(" | " + ch.TypeID.ToString("X8") + "->" + ch.SuperID.ToString("X8") + " T: " + ch.UnkownShort1.ToString("X4"));
                    spatialDepthDescription.AppendLine();
                    ret.Add(geomFromBound(ch, depth));
                    depth++;

                    ch = childToParent[ch];
                }
                LastSpatialTraceDepth = depth;
            }
            return ret;
        }

        public IEnumerable<GeometryModel3D> SpatialRepresentationForPoint(Vector3D pos)
        {
            List<GeometryModel3D> ret = new List<GeometryModel3D>();
            Dictionary<DynamicNamedSpatialData, DynamicNamedSpatialData> childToParent =
              new Dictionary<DynamicNamedSpatialData, DynamicNamedSpatialData>();
            bool found = false;
            DynamicNamedSpatialData dummy = new DynamicNamedSpatialData();
            dummy.BoundingBoxX = (float)pos.X;
            dummy.BoundingBoxY = (float)pos.Y;
            dummy.BoundingBoxZ = (float)pos.Z;
            var target = Rpk.SpatialStructs().First().GetDeepestContainingPosition(dummy);
            foreach (var res in Rpk.Entries)
            {
                foreach (var rsd in res.RSD.InnerEntries)
                {
                    if (rsd is DynamicSpatialNode)
                    {
                        var unkDat = rsd as DynamicSpatialNode;
                        foreach (var futureCh in unkDat.DataArray)
                        {
                            childToParent[futureCh] = null;
                        }
                        List<DynamicSpatialNode> toProc = new List<DynamicSpatialNode>();
                        toProc.Add(unkDat);
                        while (toProc.Any())
                        {
                            unkDat = toProc.Last();
                            toProc.RemoveAt(toProc.Count - 1);
                            foreach (var named in unkDat.DataArray)
                            {
                                foreach (var child in named.OwnedEntries)
                                {
                                    if (child is DynamicSpatialNode)
                                    {
                                        var arr = child as DynamicSpatialNode;
                                        toProc.Add(arr);
                                        foreach (var futureCh in arr.DataArray)
                                        {
                                            childToParent[futureCh] = named;
                                        }
                                    }
                                }
                                if (named == target)
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (found)
                        break;
                }
                if (found)
                    break;
            }
            if (found)
            {
                DynamicNamedSpatialData ch = target;
                int depth = 0;
                while (ch != null)
                {
                    ret.Add(geomFromBound(ch, depth));
                    depth++;

                    ch = childToParent[ch];
                }
                LastSpatialTraceDepth = depth;
            }
            return ret;
        }

        protected GeometryModel3D geomFromBound(DynamicNamedSpatialData named, int depth = 0)
        {
            GeometryModel3D ret = new GeometryModel3D();
            DiffuseMaterial materail = new DiffuseMaterial();
            materail.Brush = new SolidColorBrush(Color.FromArgb(255, (byte)(Math.Min(depth * 20, 255)), 0, 0));
            materail.AmbientColor = Colors.White;
            MeshGeometry3D mesh = new MeshGeometry3D();
            var ppp = new Point3D(named.BoundingBoxX + named.BoundingBoxHalfWidthX,
                                  named.BoundingBoxY + named.BoundingBoxHalfWidthY,//
                                  named.BoundingBoxZ + named.BoundingBoxHalfWidthZ);
            var mpp = new Point3D(named.BoundingBoxX - named.BoundingBoxHalfWidthX,
                                  named.BoundingBoxY + named.BoundingBoxHalfWidthY,//
                                  named.BoundingBoxZ + named.BoundingBoxHalfWidthZ);
            var pmp = new Point3D(named.BoundingBoxX + named.BoundingBoxHalfWidthX,
                                  named.BoundingBoxY - named.BoundingBoxHalfWidthY,
                                  named.BoundingBoxZ + named.BoundingBoxHalfWidthZ);
            var ppm = new Point3D(named.BoundingBoxX + named.BoundingBoxHalfWidthX,
                                  named.BoundingBoxY + named.BoundingBoxHalfWidthY,//
                                  named.BoundingBoxZ - named.BoundingBoxHalfWidthZ);
            var mmp = new Point3D(named.BoundingBoxX - named.BoundingBoxHalfWidthX,
                                  named.BoundingBoxY - named.BoundingBoxHalfWidthY,
                                  named.BoundingBoxZ + named.BoundingBoxHalfWidthZ);
            var mpm = new Point3D(named.BoundingBoxX - named.BoundingBoxHalfWidthX,
                                  named.BoundingBoxY + named.BoundingBoxHalfWidthY,//
                                  named.BoundingBoxZ - named.BoundingBoxHalfWidthZ);
            var pmm = new Point3D(named.BoundingBoxX + named.BoundingBoxHalfWidthX,
                                  named.BoundingBoxY - named.BoundingBoxHalfWidthY,
                                  named.BoundingBoxZ - named.BoundingBoxHalfWidthZ);
            var mmm = new Point3D(named.BoundingBoxX - named.BoundingBoxHalfWidthX,
                                  named.BoundingBoxY - named.BoundingBoxHalfWidthY,
                                  named.BoundingBoxZ - named.BoundingBoxHalfWidthZ);
            Vector3D up = new Vector3D(1, 1, 0);
            double thickness = new Vector3D(ppp.X + ppm.X,
                                            ppp.Y + ppm.Y,
                                            ppp.Z + ppm.Z).Length / 2000.0;
            addSegment(mesh, ppp, ppm, up, thickness);
            addSegment(mesh, ppp, pmp, up, thickness);
            addSegment(mesh, ppp, mpp, up, thickness);
            addSegment(mesh, ppm, mpm, up, thickness);
            addSegment(mesh, ppm, pmm, up, thickness);
            addSegment(mesh, pmp, mmp, up, thickness);
            addSegment(mesh, pmp, pmm, up, thickness);
            addSegment(mesh, mmp, mmm, up, thickness);
            addSegment(mesh, mpm, mmm, up, thickness);
            addSegment(mesh, mpm, mpp, up, thickness);
            addSegment(mesh, pmm, mmm, up, thickness);
            addSegment(mesh, mpp, mmp, up, thickness);
            ret.Geometry = mesh;
            ret.BackMaterial = materail;
            ret.Material = materail;
            return ret;
        }
    }
}