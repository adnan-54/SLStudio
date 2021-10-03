using SlrrLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
    public class WalkGeometry : RpkSpatialGeometry
    {
        private DynamicResEntry walkDef;
        private List<TrcWalk> highLightedDynamicNamedSpatialDatas = new List<TrcWalk>();
        private const byte nonSelectedAlpha = 100;
        private SolidColorBrush unresolvedColor = new SolidColorBrush(Color.FromArgb(nonSelectedAlpha, Colors.Green.R, Colors.Green.G, Colors.Green.B));
        private SolidColorBrush unresolvedColor_HighLight = Brushes.Red;
        private DiffuseMaterial unresolved_HighLight = new DiffuseMaterial();
        private DiffuseMaterial unresolvedMaterial = new DiffuseMaterial();
        private DiffuseMaterial topHighLightMaterial = new DiffuseMaterial();
        private float walkDummySize = 0.5f;

        public Dictionary<TrcWalk, GeometryModel3D> TrcWalkToGeom
        {
            get;
            private set;
        } = new Dictionary<TrcWalk, GeometryModel3D>();

        public TrcWalkManager WalkMngr
        {
            get;
            private set;
        }

        public WalkGeometry(DynamicRpk rpkDat, string rpkFilename)
        : base(rpkDat, rpkFilename)
        {
            unresolved_HighLight.Brush = unresolvedColor_HighLight;
            unresolvedMaterial.Brush = unresolvedColor;
            topHighLightMaterial.Brush = Brushes.Magenta;
            setWalkDefFromRpk();
            WalkMngr = new TrcWalkManager();
            WalkMngr.Rpk = Rpk;
            WalkMngr.FillWalksFromRpk();
        }

        public WalkGeometry(string rpkFilename)
        : base(rpkFilename)
        {
            unresolved_HighLight.Brush = unresolvedColor_HighLight;
            unresolvedMaterial.Brush = unresolvedColor;
            topHighLightMaterial.Brush = Brushes.Magenta;
            setWalkDefFromRpk();
            WalkMngr = new TrcWalkManager();
            WalkMngr.Rpk = Rpk;
            WalkMngr.FillWalksFromRpk();
        }

        public void SetTransformsOf(TrcWalk named)
        {
            Transform3DGroup transform3DGroup = new Transform3DGroup();
            transform3DGroup.Children.Add(new TranslateTransform3D(named.SpatialPosition));
            TrcWalkToGeom[named].Transform = transform3DGroup;
        }

        public override void GenerateVisuals()
        {
            TrcWalkToGeom.Clear();
            foreach (var walk in WalkMngr.Walks)
            {
                TrcWalkToGeom[walk] = GetGeomFromWalk(walk);
                SetTransformsOf(walk);
            }
        }

        public void UpdateVisuals(TrcWalk wlk)
        {
            if (!TrcWalkToGeom.ContainsKey(wlk))
                return;
            var newGeom = GetGeomFromWalk(wlk);
            TrcWalkToGeom[wlk].Geometry = newGeom.Geometry;
            SetTransformsOf(wlk);
        }

        public void UpdateRelatedVisuals(TrcWalk wlk)
        {
            UpdateVisuals(wlk);
            foreach (var related in wlk.AdjacencyDirections.Where(x => x.Direction.OtherWalk != null))
            {
                UpdateVisuals(related.Direction.OtherWalk);
                if (!related.Direction.OtherWalk.HasType23Adjacency())
                {
                    updateDeepRelatedVisuals(related.Direction.OtherWalk, wlk);
                }
            }
        }

        public void ClearHighLight()
        {
            foreach (var named in highLightedDynamicNamedSpatialDatas)
            {
                if (!TrcWalkToGeom.ContainsKey(named))
                    continue;
                TrcWalkToGeom[named].Material = unresolvedMaterial;
                TrcWalkToGeom[named].BackMaterial = unresolvedMaterial;
            }
            highLightedDynamicNamedSpatialDatas.Clear();
        }

        public void TopHighLightDynamicNamedSpatialData(TrcWalk walk)
        {
            if (walk == null)
                return;
            foreach (var related in walk.RelatedWalks)
                subHighLihgtSingleNamed(related);
            foreach (var segment in walk.AdjacencyDirections)
                highLihgtSingleNamed(segment.Direction.OtherWalk);
            foreach (var intersect in walk.AdjacencyDirections.Select(x => x.Direction.OtherWalk).Intersect(walk.RelatedWalks))
            {
                middleHighLihgtSingleNamed(intersect);
            }
            highLihgtSingleNamed(walk);
            TrcWalkToGeom[walk].BackMaterial = topHighLightMaterial;
            TrcWalkToGeom[walk].Material = topHighLightMaterial;
        }

        public void ReenforceHighlights()
        {
            var lstCpy = highLightedDynamicNamedSpatialDatas.ToList();
            highLightedDynamicNamedSpatialDatas.Clear();
            foreach (var obj in lstCpy)
            {
                highLihgtSingleNamed(obj);
            }
        }

        public TrcWalk GetClosestEntity(Vector3D pos)
        {
            float minDist = float.MaxValue;
            TrcWalk ret = null;
            float dist = float.MaxValue;
            foreach (var objLst in TrcWalkToGeom)
            {
                var named = objLst.Key;
                dist = (float)(named.SpatialPosition - pos).Length;
                if (dist < minDist)
                {
                    minDist = dist;
                    ret = objLst.Key;
                }
            }
            return ret;
        }

        public List<GeometryModel3D> GetCloseVisuals(float distance, Vector3D pos)
        {
            List<GeometryModel3D> ret = new List<GeometryModel3D>();
            foreach (var entry in TrcWalkToGeom)
            {
                if ((entry.Key.SpatialPosition - pos).Length < distance)
                    ret.Add(entry.Value);
            }
            return ret;
        }

        public GeometryModel3D GetGeomFromWalk(TrcWalk walk)
        {
            GeometryModel3D ret = new GeometryModel3D();
            MeshGeometry3D mesh = new MeshGeometry3D();

            Vector3D diffPos = new Vector3D(walkDummySize / 2.0, 0, 0);

            var frst = walk.AdjacencyDirections[0];
            var second = walk.AdjacencyDirections[1];
            Vector3D vec1 = frst.Direction.Pos1 - frst.Direction.Pos2;
            Vector3D vec2 = second.Direction.Pos2 - second.Direction.Pos1;
            Vector3D up = Vector3D.CrossProduct(vec1, vec2);
            up.Normalize();
            up *= 3.0;
            up += walk.GetGlobalSourcePosition(frst);
            var down = Vector3D.CrossProduct(vec2, vec1);
            down.Normalize();
            down *= 3.0;
            down += walk.GetGlobalSourcePosition(frst);
            //AddSegment(mesh, walk.GetGlobalSourcePosition(frst) - walk.SpatialPosition, up - walk.SpatialPosition, new Vector3D(1, 1, 0),0.3f,0.01);
            addSegment(mesh, walk.GetGlobalSourcePosition(frst) - walk.SpatialPosition, down - walk.SpatialPosition, new Vector3D(1, 1, 0), 0.3f, 0.01);
            bool globalPositions = walk.HasType23Adjacency();
            if (globalPositions)
            {
                addUShape(mesh, -diffPos, diffPos, walkDummySize, walkDummySize, walkDummySize / 3.0f);
            }
            else
            {
                Vector3D pavementPos = walk.GlobalPavementPos - walk.SpatialPosition;
                addUShape(mesh, pavementPos - diffPos, pavementPos + diffPos, walkDummySize, walkDummySize, walkDummySize);
            }
            int adjInd = 0;
            foreach (var adjacent in walk.AdjacencyDirections)
            {
                adjInd++;
                if (globalPositions)
                {
                    if (adjacent.PrettyType == TrcWalkSegmentTypeDescription.SimpleOtherConnectingSegmentWithNoOtherWalkRef)
                    {
                        Vector3D adjPos1 = new Vector3D(adjacent.Direction.X1, walk.GetNextSplineAdjacency(adjacent).ControlPoints.Last().Position.Y, adjacent.Direction.Y1) - walk.SpatialPosition;
                        Vector3D adjPos2 = new Vector3D(adjacent.Direction.X2, walk.GetPrevSplineAdjacency(adjacent).ControlPoints.First().Position.Y, adjacent.Direction.Y2) - walk.SpatialPosition;
                        addSegment(mesh, adjPos1, adjPos2, new Vector3D(1, 1, 0), 0.3f, 0.01);
                    }
                    if (adjacent.PrettyType == TrcWalkSegmentTypeDescription.SimpleSelfConnectingSegmentWithOtherWalkRef)
                    {
                        Vector3D adjPos1 = new Vector3D(adjacent.Direction.X1, walk.GetNextSplineAdjacency(adjacent).ControlPoints.Last().Position.Y, adjacent.Direction.Y1) - walk.SpatialPosition;
                        Vector3D adjPos2 = new Vector3D(adjacent.Direction.X2, walk.GetPrevSplineAdjacency(adjacent).ControlPoints.First().Position.Y, adjacent.Direction.Y2) - walk.SpatialPosition;
                        addSegment(mesh, adjPos1, adjPos2, new Vector3D(1, 1, 0), 0.3f, 0.01);
                    }
                    if (adjacent.PrettyType == TrcWalkSegmentTypeDescription.SplineOtherConnectingSegmentWithNoOtherWalkRef)
                    {
                        Vector3D startPos = adjacent.ControlPoints.First().Position - walk.SpatialPosition;
                        Vector3D startNormal = adjacent.ControlPoints.First().Normal;
                        Vector3D endPos = new Vector3D();
                        Vector3D endNormal = new Vector3D();
                        for (int i = 1; i < adjacent.ControlPoints.Count; i++)
                        {
                            endPos = adjacent.ControlPoints[i].Position - walk.SpatialPosition;
                            endNormal = adjacent.ControlPoints[i].Normal;
                            addSplineSegmentToMesh(mesh, endPos, startPos, -endNormal, startNormal);
                            startPos = endPos;
                            startNormal = endNormal;
                        }
                    }
                }
                else
                {
                    Vector3D LocalPavementPos = walk.GlobalPavementPos - walk.SpatialPosition;
                    if (adjacent.PrettyType == TrcWalkSegmentTypeDescription.SimpleOtherConnectingSegmentWithNoOtherWalkRef)
                    {
                        Vector3D adjPos1 = new Vector3D(adjacent.Direction.X1, 0, adjacent.Direction.Y1) + LocalPavementPos;
                        Vector3D adjPos2 = new Vector3D(adjacent.Direction.X2, 0, adjacent.Direction.Y2) + LocalPavementPos;
                        addSegment(mesh, adjPos1, adjPos2, new Vector3D(1, 1, 0), 0.3f, 0.01);
                    }
                    if (adjacent.PrettyType == TrcWalkSegmentTypeDescription.SimpleSelfConnectingSegmentWithOtherWalkRef)
                    {
                        Vector3D adjPos1 = new Vector3D(adjacent.Direction.X1, 0, adjacent.Direction.Y1) + LocalPavementPos;
                        Vector3D adjPos2 = new Vector3D(adjacent.Direction.X2, 0, adjacent.Direction.Y2) + LocalPavementPos;
                        addSegment(mesh, adjPos1, adjPos2, new Vector3D(1, 1, 0), 0.3f, 0.01);
                    }
                }
            }

            ret.Geometry = mesh;
            ret.BackMaterial = unresolvedMaterial;
            ret.Material = unresolvedMaterial;
            return ret;
        }

        public void DeleteWalk(TrcWalk walk)
        {
            WalkMngr.Walks.Remove(walk);
            TrcWalkToGeom.Remove(walk);
        }

        private bool matchingNativeWalk(string rsdStr)
        {
            var spl = rsdStr.Split(new char[] { '\r', '\n', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (spl.Length >= 2)
            {
                return spl[0].ToLower() == "native" && spl[1].ToLower() == "walk";
            }
            return false;
        }

        private void setWalkDefFromRpk()
        {
            walkDef = Rpk.Entries.FirstOrDefault(x => x.RSD.InnerEntries.Count == 1
                                                 && x.RSD.InnerEntries.First() is DynamicStringInnerEntry
                                                 && matchingNativeWalk((x.RSD.InnerEntries.First() as DynamicStringInnerEntry).StringData));
            if (walkDef == null && Rpk != null)
            {
                walkDef = new DynamicResEntry();
                int freeTypeID = Rpk.GetFirstFreeTypeIDIncludingHiddenEntries();
                if (freeTypeID == -1)
                    throw new Exception("There is no more room in the Rpk to add the needed native walk ref");
                walkDef.SuperID = Rpk.GetOrAddExternalRefIndexOfRPK("system.Rpk") << 16 | 0x00000015;
                walkDef.TypeID = freeTypeID;
                walkDef.Alias = "walk";
                walkDef.AdditionalType = 0;
                walkDef.IsParentCompatible = 1.0f;
                walkDef.TypeOfEntry = 8;
                walkDef.RSD = new DynamicRsdEntry();
                walkDef.RSD.InnerEntries.Add(new DynamicStringInnerEntry("native walk"));
                Rpk.Entries.Add(walkDef);
            }
        }

        private void subHighLihgtSingleNamed(TrcWalk walk)
        {
            if (walk == null)
                return;
            if (!TrcWalkToGeom.ContainsKey(walk))
                return;
            highLightedDynamicNamedSpatialDatas.Add(walk);
            DiffuseMaterial material = new DiffuseMaterial();
            material.Brush = Brushes.White;
            TrcWalkToGeom[walk].Material = material;
            TrcWalkToGeom[walk].BackMaterial = material;
        }

        private void middleHighLihgtSingleNamed(TrcWalk walk)
        {
            if (walk == null)
                return;
            if (!TrcWalkToGeom.ContainsKey(walk))
                return;
            highLightedDynamicNamedSpatialDatas.Add(walk);
            DiffuseMaterial material = new DiffuseMaterial();
            material.Brush = Brushes.Blue;
            TrcWalkToGeom[walk].Material = material;
            TrcWalkToGeom[walk].BackMaterial = material;
        }

        private void highLihgtSingleNamed(TrcWalk walk)
        {
            if (walk == null)
                return;
            if (!TrcWalkToGeom.ContainsKey(walk))
                return;
            highLightedDynamicNamedSpatialDatas.Add(walk);
            TrcWalkToGeom[walk].Material = unresolved_HighLight;
            TrcWalkToGeom[walk].BackMaterial = unresolved_HighLight;
        }

        private GeometryModel3D unresolvedBoxAtPos()
        {
            GeometryModel3D ret = new GeometryModel3D();
            MeshGeometry3D mesh = new MeshGeometry3D();
            addUShape(mesh, -boxDiff, boxDiff, boxWidth, boxWidth, (float)boxWidth);
            ret.Material = unresolvedMaterial;
            ret.Geometry = mesh;
            return ret;
        }

        private MeshGeometry3D unresolvedBoxMeshAtPos()
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            addUShape(mesh, -boxDiff, boxDiff, boxWidth, boxWidth, (float)boxWidth);
            return mesh;
        }

        private void updateDeepRelatedVisuals(TrcWalk wlk, TrcWalk prevContext)
        {
            foreach (var related in wlk.AdjacencyDirections.Where(x => x.Direction.OtherWalk != null))
            {
                if (related.Direction.OtherWalk == prevContext)
                    continue;
                UpdateVisuals(related.Direction.OtherWalk);
                if (!related.Direction.OtherWalk.HasType23Adjacency())
                {
                    updateDeepRelatedVisuals(related.Direction.OtherWalk, wlk);
                }
            }
        }

        private void addSplineSegmentToMesh(MeshGeometry3D obj, Vector3D p1v, Vector3D p2v,
                                            Vector3D n1, Vector3D n2, float width = 0.3f, bool useLengthOfNormals = false)
        {
            //Interpolate some
            Vector3D m1 = n1 + p1v;
            Vector3D m2 = n2 + p2v;
            if (!useLengthOfNormals)
            {
                n1.Normalize();
                n2.Normalize();
                m1 = (n1 * (p1v - p2v).Length / 3.14) + p1v;
                m2 = (n2 * (p1v - p2v).Length / 3.14) + p2v;
            }
            Vector3D lastPos = p1v;
            for (float t = 0.1f; t <= 1.09f; t += 0.1f)
            {
                Vector3D curPos = Math.Pow(1 - t, 3) * p1v +
                                  3 * Math.Pow(1 - t, 2) * t * m1 +
                                  3 * (1 - t) * t * t * m2 +
                                  t * t * t * p2v;
                addSegment(obj, lastPos,
                           curPos, new Vector3D(1, 1, 0),
                           0.01, width);
                lastPos = curPos;
            }
        }
    }
}