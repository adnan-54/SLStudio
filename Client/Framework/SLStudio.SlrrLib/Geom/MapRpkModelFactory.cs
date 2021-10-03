using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
    public class MapRpkModelFactory : IModelFactory
    {
        private IEnumerable<string> modelGroupsCache = null;
        private bool loadPolys = true;
        private BoundBox3D bounds = BoundBox3D.MaxBound;

        public SlrrLib.Model.BinaryRpk RpkLoaded
        {
            get;
            private set;
        } = null;

        public string LoadedRpkName
        {
            get;
            private set;
        } = "";

        public bool LoadPhys
        {
            get;
            private set;
        } = false;

        public MapRpkModelFactory()
        {
        }

        public MapRpkModelFactory(string targetRPK, bool loadPhys, bool loadPoly)
        {
            modelGroupsCache = new List<string> { targetRPK };
            LoadPhys = loadPhys;
            loadPolys = loadPoly;
        }

        public MapRpkModelFactory(string targetRPK, bool loadPhys, bool loadPoly, BoundBox3D loadBound)
        {
            modelGroupsCache = new List<string> { targetRPK };
            LoadPhys = loadPhys;
            loadPolys = loadPoly;
            bounds = loadBound;
        }

        public IEnumerable<NamedModel> GenModels(string fnam)
        {
            List<NamedModel> ret = new List<NamedModel>();
            SlrrLib.Model.BinaryRpk rpk = new SlrrLib.Model.BinaryRpk(fnam, true);
            LoadedRpkName = fnam;
            int curInd = 0;
            foreach (var res in rpk.RESEntries)
            {
                foreach (var hidden in res.GetFlatHiddenEntries())
                {
                    if (hidden is SlrrLib.Model.BinaryInnerPolyEntry)
                    {
                        if (loadPolys == false)
                            continue;
                        var innerPoly = hidden as SlrrLib.Model.BinaryInnerPolyEntry;
                        InnerPolyGeometry geom = new InnerPolyGeometry(innerPoly);
                        if (!geom.HasVertexInBound(bounds.MaxX, bounds.MaxY, bounds.MaxZ, bounds.MinX, bounds.MinY, bounds.MinZ))
                            continue;
                        int ind = 0;
                        foreach (var partMEsh in geom.GetWpfModels())
                        {
                            Transform3DGroup transform3DGroup = new Transform3DGroup();
                            transform3DGroup.Children.Add(new TranslateTransform3D(new Vector3D(0, 0, 0)));
                            partMEsh.Transform = transform3DGroup;
                            ret.Add(new NamedPolyModel
                            {
                                Name = innerPoly.Siganture + "_" + curInd.ToString(),
                                SourceOfModel = NamedModelSource.Mesh,
                                MeshIndex = ind,
                                ModelGeom = partMEsh,
                                Translate = new Vector3D(0, 0, 0),
                                PolySource = innerPoly,
                            });
                            curInd++;
                            ind++;
                        }
                    }
                    if (hidden is SlrrLib.Model.BinaryInnerPhysEntry)
                    {
                        if (LoadPhys == false)
                            continue;
                        var innerPhys = hidden as SlrrLib.Model.BinaryInnerPhysEntry;
                        InnerPhysGeometry geom = new InnerPhysGeometry(innerPhys);
                        if (!geom.HasVertexInBound(bounds.MaxX, bounds.MaxY, bounds.MaxZ, bounds.MinX, bounds.MinY, bounds.MinZ))
                            continue;
                        foreach (var partMEsh in geom.WpfModels())
                        {
                            Transform3DGroup transform3DGroup = new Transform3DGroup();
                            transform3DGroup.Children.Add(new TranslateTransform3D(new Vector3D(0, 0, 0)));
                            partMEsh.Transform = transform3DGroup;
                            ret.Add(new NamedPhysModel
                            {
                                Name = innerPhys.Siganture + "_" + curInd.ToString(),
                                SourceOfModel = NamedModelSource.Mesh,
                                ModelGeom = partMEsh,
                                Translate = new Vector3D(0, 0, 0),
                                PhysSource = innerPhys,
                            });
                            curInd++;
                        }
                    }
                }
            }
            RpkLoaded = rpk;
            return ret;
        }

        public IEnumerable<string> GetModelGroups()
        {
            return modelGroupsCache;
        }

        public IEnumerable<GeometryModel3D> GetSpatialRepresentationForPoint(Vector3D p)
        {
            List<GeometryModel3D> ret = new List<GeometryModel3D>();
            foreach (var res in RpkLoaded.RESEntries)
            {
                foreach (var rsd in res.RSD.InnerEntries)
                {
                    if (rsd is SlrrLib.Model.BinarySpatialNode)
                    {
                        var unkDat = rsd as SlrrLib.Model.BinarySpatialNode;
                        List<SlrrLib.Model.BinarySpatialNode> toProc = new List<SlrrLib.Model.BinarySpatialNode>();
                        toProc.Add(unkDat);
                        while (toProc.Any())
                        {
                            unkDat = toProc.Last();
                            toProc.RemoveAt(toProc.Count - 1);
                            foreach (var named in unkDat.DataArray)
                            {
                                foreach (var child in named.OwnedEntries)
                                {
                                    if (child is SlrrLib.Model.BinarySpatialNode)
                                        toProc.Add(child as SlrrLib.Model.BinarySpatialNode);
                                }
                                if (named.IsPointInsideBound((float)p.X, (float)p.Y, (float)p.Z))
                                {
                                    ret.Add(getGeomFromBound(named));
                                }
                            }
                        }
                    }
                }
            }
            return ret;
        }

        public IEnumerable<GeometryModel3D> GetSpatialRepresentationForObject(SlrrLib.Model.BinaryNamedSpatialData target)
        {
            List<GeometryModel3D> ret = new List<GeometryModel3D>();
            Dictionary<SlrrLib.Model.BinaryNamedSpatialData, SlrrLib.Model.BinaryNamedSpatialData> childToParent =
              new Dictionary<SlrrLib.Model.BinaryNamedSpatialData, SlrrLib.Model.BinaryNamedSpatialData>();
            bool found = false;
            foreach (var res in RpkLoaded.RESEntries)
            {
                foreach (var rsd in res.RSD.InnerEntries)
                {
                    if (rsd is SlrrLib.Model.BinarySpatialNode)
                    {
                        var unkDat = rsd as SlrrLib.Model.BinarySpatialNode;
                        List<SlrrLib.Model.BinarySpatialNode> toProc = new List<SlrrLib.Model.BinarySpatialNode>();
                        toProc.Add(unkDat);
                        while (toProc.Any())
                        {
                            unkDat = toProc.Last();
                            toProc.RemoveAt(toProc.Count - 1);
                            foreach (var named in unkDat.DataArray)
                            {
                                foreach (var child in named.OwnedEntries)
                                {
                                    if (child is SlrrLib.Model.BinarySpatialNode)
                                    {
                                        var arr = child as SlrrLib.Model.BinarySpatialNode;
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
                SlrrLib.Model.BinaryNamedSpatialData ch = target;
                while (ch != null)
                {
                    ret.Add(getGeomFromBound(ch));
                    ch = childToParent[ch];
                }
            }
            return ret;
        }

        public string ResolveMapTextureIndex(int textindexInPoly)
        {
            try
            {
                var trackRes = RpkLoaded.RESEntries.FirstOrDefault(x => x.AdditionalType == 17 && x.TypeOfEntry == 1);
                if (trackRes == null)
                    return @"grid.png";
                var rsd = trackRes.RSD.InnerEntries.First(x => x is SlrrLib.Model.BinaryRSDInnerEntry) as SlrrLib.Model.BinaryRSDInnerEntry;
                var paramsLine = rsd.StringData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)[1];
                var typeID = paramsLine.Split(new char[] { ' ', ',', '\t' }, StringSplitOptions.RemoveEmptyEntries)[1];
                int renderTypeID = int.Parse(typeID.Remove(0, 2), System.Globalization.NumberStyles.HexNumber);
                var renderRes = RpkLoaded.RESEntries.FirstOrDefault(x => x.TypeID == renderTypeID);
                var textureList = (renderRes.RSD.InnerEntries.First() as SlrrLib.Model.BinaryStringInnerEntry)
                                  .StringData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Where(x => x.ToLower().StartsWith("texture"))
                                  .Select(x => int.Parse(x.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[1].Remove(0, 2), System.Globalization.NumberStyles.HexNumber)).ToList();
                var meshIDStr = (renderRes.RSD.InnerEntries.First() as SlrrLib.Model.BinaryStringInnerEntry)
                                .StringData.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).First(x => x.ToLower().StartsWith("mesh"));
                var meshID = int.Parse(meshIDStr.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries)[1].Remove(0, 2), System.Globalization.NumberStyles.HexNumber);
                var slrrRoot = SlrrLib.Model.HighLevel.GameFileManager.GetSLRRRoot(LoadedRpkName);
                var rpkWithMesh = RpkLoaded;
                if (meshID >> 16 != 0)
                {
                    int extrefInd = (meshID >> 16) - 1;
                    var rpkName = RpkLoaded.ExternalReferences.ElementAt(extrefInd).ReferenceString;
                    rpkWithMesh = new SlrrLib.Model.BinaryRpk(slrrRoot + "\\" + rpkName, true);
                }
                var meshRes = rpkWithMesh.RESEntries.First(x => x.TypeID == (meshID & 0x0000FFFF));
                var fnamMesh = (meshRes.RSD.InnerEntries.First(x => x is SlrrLib.Model.BinaryStringInnerEntry) as SlrrLib.Model.BinaryStringInnerEntry).StringData;
                fnamMesh = slrrRoot + "\\" + fnamMesh.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).First(x => x.ToLower().StartsWith("sourcefile"))
                           .Trim().Remove(0, 10).Trim();
                var mapScx = SlrrLib.Model.Scx.ConstructScx(fnamMesh, true);
                int textindex = textindexInPoly;
                if (mapScx.Version == 3)
                {
                    var mapv3Scx = mapScx as SlrrLib.Model.BinaryScxV3;
                    var refedmaterial = mapv3Scx.Materials[textindexInPoly];
                    textindex = refedmaterial.DiffuseMapIndex;
                }
                if (mapScx.Version == 4)
                {
                    var mapv4Scx = mapScx as SlrrLib.Model.BinaryScxV4;
                    var refedmaterial = mapv4Scx.Materials.ElementAt(textindexInPoly);
                    textindex = (refedmaterial.Entries.First(x => x.PrettyName == "Diffuse Map") as SlrrLib.Model.BinaryMaterialV4MapDefinition)
                                .TextureIndex;
                }

                var textureTypeID = textureList[textindex];
                var rpkWithTexture = RpkLoaded;
                if (textureTypeID >> 16 != 0)
                {
                    int extrefInd = (textureTypeID >> 16) - 1;
                    var rpkName = RpkLoaded.ExternalReferences.ElementAt(extrefInd).ReferenceString;
                    rpkWithTexture = new SlrrLib.Model.BinaryRpk(slrrRoot + "\\" + rpkName, true);
                }
                var textureRes = rpkWithTexture.RESEntries.First(x => x.TypeID == (textureTypeID & 0x0000FFFF));
                var fnam = (textureRes.RSD.InnerEntries.First(x => x is SlrrLib.Model.BinaryStringInnerEntry) as SlrrLib.Model.BinaryStringInnerEntry).StringData;
                fnam = slrrRoot + "\\" + fnam.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).First(x => x.ToLower().StartsWith("sourcefile"))
                       .Trim().Remove(0, 10).Trim();
                if (File.Exists(fnam))
                    return fnam;
            }
            catch (Exception)
            {
                return @"grid.png";
            }
            return @"grid.png";
        }

        private Vector3D scaleVector(Vector3D vector, double length)
        {
            double scale = length / vector.Length;
            return new Vector3D(
                     vector.X * scale,
                     vector.Y * scale,
                     vector.Z * scale);
        }

        private void addTriangle(MeshGeometry3D mesh, Point3D point1, Point3D point2, Point3D point3)
        {
            int index1 = mesh.Positions.Count;
            mesh.Positions.Add(point1);
            mesh.Positions.Add(point2);
            mesh.Positions.Add(point3);
            mesh.TriangleIndices.Add(index1++);
            mesh.TriangleIndices.Add(index1++);
            mesh.TriangleIndices.Add(index1);
        }

        private void addSegment(MeshGeometry3D mesh, Point3D point1, Point3D point2, Vector3D up, double thickness = 0.1)
        {
            Vector3D v = point2 - point1;
            Vector3D n1 = scaleVector(up, thickness / 2.0);
            Vector3D n2 = Vector3D.CrossProduct(v, n1);
            n2 = scaleVector(n2, thickness / 2.0);
            Point3D p1pp = point1 + n1 + n2;
            Point3D p1mp = point1 - n1 + n2;
            Point3D p1pm = point1 + n1 - n2;
            Point3D p1mm = point1 - n1 - n2;
            Point3D p2pp = point2 + n1 + n2;
            Point3D p2mp = point2 - n1 + n2;
            Point3D p2pm = point2 + n1 - n2;
            Point3D p2mm = point2 - n1 - n2;
            addTriangle(mesh, p1pp, p1mp, p2mp);
            addTriangle(mesh, p1pp, p2mp, p2pp);
            addTriangle(mesh, p1pp, p2pp, p2pm);
            addTriangle(mesh, p1pp, p2pm, p1pm);
            addTriangle(mesh, p1pm, p2pm, p2mm);
            addTriangle(mesh, p1pm, p2mm, p1mm);
            addTriangle(mesh, p1mm, p2mm, p2mp);
            addTriangle(mesh, p1mm, p2mp, p1mp);
            addTriangle(mesh, p1pp, p1pm, p1mm);
            addTriangle(mesh, p1pp, p1mm, p1mp);
            addTriangle(mesh, p2pp, p2mp, p2mm);
            addTriangle(mesh, p2pp, p2mm, p2pm);
        }

        private GeometryModel3D getGeomFromBound(SlrrLib.Model.BinaryNamedSpatialData named)
        {
            GeometryModel3D ret = new GeometryModel3D();
            DiffuseMaterial materail = new DiffuseMaterial();
            materail.Brush = Brushes.Red;
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
                                            ppp.Z + ppm.Z).Length / 1000.0;
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