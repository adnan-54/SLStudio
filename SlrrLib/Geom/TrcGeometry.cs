using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SlrrLib.Geom
{
    public class TrcGeometry : GeometryBuilder
    {
        private List<SlrrLib.Model.TrcObject> highLightedObjects = new List<SlrrLib.Model.TrcObject>();
        private const byte nonSelectedAlpha = 70;
        private const byte nonSelectedAlpha_Race = 150;

        public SlrrLib.Model.Trc Trc
        {
            get;
            private set;
        }

        public Dictionary<SlrrLib.Model.TrcObject, GeometryModel3D> TrcStructToGeom
        {
            get;
            private set;
        } = new Dictionary<SlrrLib.Model.TrcObject, GeometryModel3D>();

        public SolidColorBrush Intersection1Color
        {
            get;
            private set;
        } = new SolidColorBrush(Color.FromArgb(nonSelectedAlpha, Colors.DarkRed.R, Colors.DarkRed.G, Colors.DarkRed.B));

        public SolidColorBrush Intersection2Color
        {
            get;
            private set;
        } = new SolidColorBrush(Color.FromArgb(nonSelectedAlpha, Colors.DarkGreen.R, Colors.DarkGreen.G, Colors.DarkGreen.B));

        public SolidColorBrush LaneColor
        {
            get;
            private set;
        } = new SolidColorBrush(Color.FromArgb(nonSelectedAlpha, Colors.DarkBlue.R, Colors.DarkBlue.G, Colors.DarkBlue.B));

        public SolidColorBrush RaceColor
        {
            get;
            private set;
        } = new SolidColorBrush(Color.FromArgb(nonSelectedAlpha_Race, Colors.DarkGray.R, Colors.DarkGray.G, Colors.DarkGray.B));

        public SolidColorBrush Intersection1ColorHighLight
        {
            get;
            private set;
        } = Brushes.Red;

        public SolidColorBrush Intersection2ColorHighLight
        {
            get;
            private set;
        } = Brushes.Green;

        public SolidColorBrush LaneColorHighLight
        {
            get;
            private set;
        } = Brushes.Blue;

        public SolidColorBrush RaceColorHighLight
        {
            get;
            private set;
        } = Brushes.White;

        public EmissiveMaterial Intersection1MaterialHighLight
        {
            get;
            private set;
        } = new EmissiveMaterial();

        public EmissiveMaterial Intersection2MaterialHighLight
        {
            get;
            private set;
        } = new EmissiveMaterial();

        public EmissiveMaterial LaneMaterialHighLight
        {
            get;
            private set;
        } = new EmissiveMaterial();

        public EmissiveMaterial RaceMaterialHighLight
        {
            get;
            private set;
        } = new EmissiveMaterial();

        public EmissiveMaterial Intersection1Material
        {
            get;
            private set;
        } = new EmissiveMaterial();

        public EmissiveMaterial Intersection2Material
        {
            get;
            private set;
        } = new EmissiveMaterial();

        public EmissiveMaterial LaneMaterial
        {
            get;
            private set;
        } = new EmissiveMaterial();

        public EmissiveMaterial RaceMaterial
        {
            get;
            private set;
        } = new EmissiveMaterial();

        public EmissiveMaterial TopHighLightMaterial
        {
            get;
            private set;
        } = new EmissiveMaterial();

        public EmissiveMaterial MiddleHighLightMaterial
        {
            get;
            private set;
        } = new EmissiveMaterial();

        public float CrossThickness
        {
            get;
            private set;
        } = 0.5f;

        public float RaceThickness
        {
            get;
            private set;
        } = 0.5f;

        public float LaneThickness
        {
            get;
            private set;
        } = 0.5f;

        public TrcGeometry(SlrrLib.Model.Trc model)
        {
            Trc = model;
            Intersection1MaterialHighLight.Brush = Intersection1ColorHighLight;
            Intersection2MaterialHighLight.Brush = Intersection2ColorHighLight;
            LaneMaterialHighLight.Brush = LaneColorHighLight;
            RaceMaterialHighLight.Brush = RaceColorHighLight;
            Intersection1Material.Brush = Intersection1Color;
            Intersection2Material.Brush = Intersection2Color;
            LaneMaterial.Brush = LaneColor;
            RaceMaterial.Brush = RaceColor;
            TopHighLightMaterial.Brush = Brushes.Magenta;
            MiddleHighLightMaterial.Brush = Brushes.Gold;
        }

        public void ClearHighLight()
        {
            foreach (var trcStruct in highLightedObjects)
            {
                if (!TrcStructToGeom.ContainsKey(trcStruct))
                    continue;
                if (trcStruct is SlrrLib.Model.TrcIntersection)
                {
                    if (Trc.IntersectionData1.Contains(trcStruct))
                    {
                        TrcStructToGeom[trcStruct].BackMaterial = Intersection1Material;
                        TrcStructToGeom[trcStruct].Material = Intersection1Material;
                    }
                    else
                    {
                        TrcStructToGeom[trcStruct].BackMaterial = Intersection2Material;
                        TrcStructToGeom[trcStruct].Material = Intersection2Material;
                    }
                }
                else if (trcStruct is SlrrLib.Model.TrcLane)
                {
                    TrcStructToGeom[trcStruct].BackMaterial = LaneMaterial;
                    TrcStructToGeom[trcStruct].Material = LaneMaterial;
                }
                else if (trcStruct is SlrrLib.Model.TrcNavigatorNode)
                {
                    TrcStructToGeom[trcStruct].BackMaterial = RaceMaterial;
                    TrcStructToGeom[trcStruct].Material = RaceMaterial;
                }
            }
            highLightedObjects.Clear();
        }

        public void TopHighLightObject(SlrrLib.Model.TrcObject trcStruct)
        {
            if (trcStruct == null)
                return;
            if (!TrcStructToGeom.ContainsKey(trcStruct))
                return;
            highLihgtSingleTrcStruct(trcStruct);
            TrcStructToGeom[trcStruct].BackMaterial = TopHighLightMaterial;
            TrcStructToGeom[trcStruct].Material = TopHighLightMaterial;
        }

        public void MiddleighLightObject(SlrrLib.Model.TrcObject trcStruct)
        {
            if (trcStruct == null)
                return;
            if (!TrcStructToGeom.ContainsKey(trcStruct))
                return;
            highLihgtSingleTrcStruct(trcStruct);
            TrcStructToGeom[trcStruct].BackMaterial = MiddleHighLightMaterial;
            TrcStructToGeom[trcStruct].Material = MiddleHighLightMaterial;
        }

        public void HighLightChildrenEntititesClearBefore(SlrrLib.Model.TrcObject trcStruct)
        {
            ClearHighLight();
            if (trcStruct == null)
                return;
            highLightChildrenEntitites(trcStruct);
            TrcStructToGeom[trcStruct].BackMaterial = TopHighLightMaterial;
            TrcStructToGeom[trcStruct].Material = TopHighLightMaterial;
        }

        public void ReenforceHighlights()
        {
            var lstCpy = highLightedObjects.ToList();
            highLightedObjects.Clear();
            foreach (var obj in lstCpy)
            {
                highLihgtSingleTrcStruct(obj);
            }
        }

        public void RemoveTrcObject(SlrrLib.Model.TrcObject trcStruct)
        {
            if (trcStruct is SlrrLib.Model.TrcIntersection)
            {
                var interSect = trcStruct as SlrrLib.Model.TrcIntersection;
                Trc.IntersectionData1.Remove(interSect);
                Trc.IntersectionData2.Remove(interSect);
            }
            else if (trcStruct is SlrrLib.Model.TrcLane)
            {
                var lane = trcStruct as SlrrLib.Model.TrcLane;
                Trc.RemoveLane(lane);
            }
            else if (trcStruct is SlrrLib.Model.TrcNavigatorNode)
            {
                var race = trcStruct as SlrrLib.Model.TrcNavigatorNode;
                Trc.RaceNodeDescriptors.Remove(race);
            }
            TrcStructToGeom.Remove(trcStruct);
        }

        public void HighLightParentEntitiesClearBefore(SlrrLib.Model.TrcObject trcStruct)
        {
            ClearHighLight();
            highLihgtSingleTrcStruct(trcStruct);
            foreach (var obj in TrcStructToGeom)
            {
                if (obj.Key is SlrrLib.Model.TrcIntersection)
                {
                    var interSect = obj.Key as SlrrLib.Model.TrcIntersection;
                    foreach (var dat in interSect.LaneRefs)
                    {
                        if (dat.LaneInFlattened_1 == trcStruct)
                            highLihgtSingleTrcStruct(interSect);
                    }
                }
                else if (obj.Key is SlrrLib.Model.TrcLane)
                {
                    var lane = obj.Key as SlrrLib.Model.TrcLane;
                    if (lane.InFlattened_1 == trcStruct || lane.InFlattened_2 == trcStruct ||
                        lane.LaneInFlattened_1 == trcStruct || lane.LeftAdjacentLane == trcStruct ||
                        lane.RightAdjacentLane == trcStruct || lane.RaceNodeInFlattened_1 == trcStruct)
                        highLihgtSingleTrcStruct(lane);
                }
                else if (obj.Key is SlrrLib.Model.TrcNavigatorNode)
                {
                    var race = obj.Key as SlrrLib.Model.TrcNavigatorNode;
                    if (race.IntersectionInFlattened_1 == trcStruct || race.IntersectionInFlattened_2 == trcStruct ||
                        race.LaneInFlattened_1 == trcStruct)
                        highLihgtSingleTrcStruct(race);
                }
            }
            TrcStructToGeom[trcStruct].BackMaterial = TopHighLightMaterial;
            TrcStructToGeom[trcStruct].Material = TopHighLightMaterial;
        }

        public SlrrLib.Model.TrcObject GetClosestEntity(Vector3D pos)
        {
            float minDist = float.MaxValue;
            SlrrLib.Model.TrcObject ret = null;
            float dist = float.MaxValue;
            foreach (var objLst in TrcStructToGeom)
            {
                var trcStruct = objLst.Key;
                if (trcStruct is SlrrLib.Model.TrcIntersection)
                {
                    var cross = trcStruct as SlrrLib.Model.TrcIntersection;
                    cross.SetPositionToAvgFromFloatDatas();
                    dist = (float)(pos - new Vector3D(cross.PositionXData, cross.PositionYData, cross.PositionZData)).Length;
                }
                else if (trcStruct is SlrrLib.Model.TrcLane)
                {
                    var lane = trcStruct as SlrrLib.Model.TrcLane;
                    float dist1 = (float)(pos - lane.EndPoint).Length;
                    float dist2 = (float)(pos - lane.StartPoint).Length;
                    dist = Math.Min(dist1, dist2);
                }
                else if (trcStruct is SlrrLib.Model.TrcNavigatorNode)
                {
                    var race = trcStruct as SlrrLib.Model.TrcNavigatorNode;
                    race.SetPositionToAvgFromFloatDatas();
                    float dist1 = (float)(pos - race.GetEndPoint()).Length;
                    float dist2 = (float)(pos - race.GetStartPoint()).Length;
                    dist = Math.Min(dist1, dist2);
                }
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
            foreach (var entry in TrcStructToGeom)
            {
                if ((getVec3(entry.Value.Bounds.Location) - pos).Length - max3(entry.Value.Bounds.Size) < distance)
                    ret.Add(entry.Value);
            }
            return ret;
        }

        public override void GenerateVisuals()
        {
            TrcStructToGeom.Clear();
            foreach (var int1 in Trc.IntersectionData1)
            {
                var toad = MeshFromIntersection(int1, Intersection1Color, CrossThickness);
                TrcStructToGeom[int1] = toad;
            }
            foreach (var int2 in Trc.IntersectionData2)
            {
                var toad = MeshFromIntersection(int2, Intersection2Color, CrossThickness);
                TrcStructToGeom[int2] = toad;
            }
            foreach (var lane in Trc.LaneData)
            {
                var toad = MeshFromLane(lane, LaneColor, LaneThickness);
                TrcStructToGeom[lane] = toad;
            }
            foreach (var raceNode in Trc.RaceNodeDescriptors)
            {
                var toad = MeshFromRaceNode(raceNode, RaceColor, RaceThickness);
                TrcStructToGeom[raceNode] = toad;
            }
        }

        public GeometryModel3D MeshFromIntersection(SlrrLib.Model.TrcIntersection data, SolidColorBrush ColorBrush, float thickness = 3.0f, bool visualizeNormals = false)
        {
            MeshGeometry3D obj = new MeshGeometry3D();
            foreach (var floatDat in data.Crossings)
            {
                addSegementFromFloatData(obj, floatDat, thickness, visualizeNormals);
            }
            EmissiveMaterial material = new EmissiveMaterial();
            material.Brush = ColorBrush;
            EmissiveMaterial backMaterial = new EmissiveMaterial();
            backMaterial.Brush = ColorBrush;
            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = obj;
            model.Material = material;
            model.BackMaterial = backMaterial;
            return model;
        }

        public GeometryModel3D MeshFromLane(SlrrLib.Model.TrcLane data, SolidColorBrush ColorBrush, float thickness = 3.0f, bool visualizeNormals = false, bool addBounds = false)
        {
            MeshGeometry3D obj = new MeshGeometry3D();
            foreach (var floatDat in data.Spline)
            {
                addSegementFromFloatData(obj, floatDat, thickness, visualizeNormals);
            }
            if (addBounds)
            {
                int myInd = Trc.LaneData.IndexOf(data);
                var entryWithMyIndex = Trc.LaneData.FirstOrDefault(x => x.PseudoSelfIndexInLaneArray == myInd);
                if (entryWithMyIndex != null)
                {
                    var height = data.Pos1Y * 0.5 + data.Pos2Y * 0.5;
                    addSegment(obj, new Vector3D(entryWithMyIndex.MinXOfPseudoSelf, height, -10000),
                               new Vector3D(entryWithMyIndex.MinXOfPseudoSelf, height, 10000),
                               new Vector3D(1, 1, 0), 0.5, 0.5);
                    addSegment(obj, new Vector3D(entryWithMyIndex.MaxXOfPseudoSelf, height, -10000),
                               new Vector3D(entryWithMyIndex.MaxXOfPseudoSelf, height, 10000),
                               new Vector3D(1, 1, 0), 0.5, 0.5);
                    addSegment(obj, new Vector3D(entryWithMyIndex.MaxxTrueOfPseudoSelf, height, -10000),
                               new Vector3D(entryWithMyIndex.MaxxTrueOfPseudoSelf, height, 10000),
                               new Vector3D(1, 1, 0), 0.5, 0.5);
                }
            }
            //var pos = data.StartPoint;
            //AddUShape(obj, pos, pos + new Vector3D(1, 0, 0), 1, 1, 100);
            EmissiveMaterial material = new EmissiveMaterial();
            material.Brush = ColorBrush;
            EmissiveMaterial backMaterial = new EmissiveMaterial();
            backMaterial.Brush = ColorBrush;
            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = obj;
            model.Material = material;
            model.BackMaterial = backMaterial;
            return model;
        }

        public GeometryModel3D MeshFromRaceNode(SlrrLib.Model.TrcNavigatorNode data, SolidColorBrush ColorBrush, float thickness = 3.0f, bool visualizeNormals = false)
        {
            MeshGeometry3D obj = new MeshGeometry3D();
            foreach (var floatDat in data.Spline)
            {
                addSegementFromFloatData(obj, floatDat, thickness / 2.0f, visualizeNormals, 0);
            }
            EmissiveMaterial material = new EmissiveMaterial();
            material.Brush = ColorBrush;
            EmissiveMaterial backMaterial = new EmissiveMaterial();
            backMaterial.Brush = ColorBrush;
            GeometryModel3D model = new GeometryModel3D();
            model.Geometry = obj;
            model.Material = material;
            model.BackMaterial = backMaterial;
            return model;
        }

        public void UpdateGeneratedVisualsOf(SlrrLib.Model.TrcObject obj)
        {
            if (TrcStructToGeom.ContainsKey(obj))
            {
                if (obj is SlrrLib.Model.TrcIntersection)
                {
                    var geom = MeshFromIntersection(obj as SlrrLib.Model.TrcIntersection, Intersection1Color, CrossThickness);
                    TrcStructToGeom[obj].Geometry = geom.Geometry;
                }
                else if (obj is SlrrLib.Model.TrcLane)
                {
                    var geom = MeshFromLane(obj as SlrrLib.Model.TrcLane, LaneColor, LaneThickness);
                    TrcStructToGeom[obj].Geometry = geom.Geometry;
                }
                else if (obj is SlrrLib.Model.TrcNavigatorNode)
                {
                    var geom = MeshFromRaceNode(obj as SlrrLib.Model.TrcNavigatorNode, RaceColor, RaceThickness);
                    TrcStructToGeom[obj].Geometry = geom.Geometry;
                }
                ReenforceHighlights();
            }
        }

        private void highLightChildrenEntitites(SlrrLib.Model.TrcObject trcStruct)
        {
            if (trcStruct == null)
                return;
            if (highLightedObjects.Contains(trcStruct))
                return;
            highLihgtSingleTrcStruct(trcStruct);
            if (trcStruct is SlrrLib.Model.TrcIntersection)
            {
                var interSect = trcStruct as SlrrLib.Model.TrcIntersection;
                EmissiveMaterial trueMaterail = new EmissiveMaterial();
                EmissiveMaterial falseMaterail = new EmissiveMaterial();
                trueMaterail.Brush = Brushes.White;
                falseMaterail.Brush = Brushes.Black;
                foreach (var dat in interSect.LaneRefs)
                {
                    if (dat.IsInboundLane == 1)
                        highLihgtSingleTrcStruct(dat.LaneInFlattened_1);
                    else
                        MiddleighLightObject(dat.LaneInFlattened_1);
                }
            }
            else if (trcStruct is SlrrLib.Model.TrcLane)
            {
                var lane = trcStruct as SlrrLib.Model.TrcLane;
                highLihgtSingleTrcStruct(lane.InFlattened_1);
                highLihgtSingleTrcStruct(lane.InFlattened_2);
                //HighLihgtSingleTrcStruct(lane.LaneInFlattened_1);
                highLihgtSingleTrcStruct(lane.LeftAdjacentLane);
                highLihgtSingleTrcStruct(lane.RightAdjacentLane);
                highLihgtSingleTrcStruct(lane.RaceNodeInFlattened_1);
            }
            else if (trcStruct is SlrrLib.Model.TrcNavigatorNode)
            {
                var race = trcStruct as SlrrLib.Model.TrcNavigatorNode;
                highLihgtSingleTrcStruct(race.IntersectionInFlattened_1);
                highLihgtSingleTrcStruct(race.IntersectionInFlattened_2);
                highLihgtSingleTrcStruct(race.LaneInFlattened_1);
            }
        }

        private void highLihgtSingleTrcStruct(SlrrLib.Model.TrcObject trcStruct)
        {
            if (trcStruct == null)
                return;
            if (!TrcStructToGeom.ContainsKey(trcStruct))
                return;
            highLightedObjects.Add(trcStruct);
            if (trcStruct is SlrrLib.Model.TrcIntersection)
            {
                if (Trc.IntersectionData1.Contains(trcStruct))
                {
                    TrcStructToGeom[trcStruct].BackMaterial = Intersection1MaterialHighLight;
                    TrcStructToGeom[trcStruct].Material = Intersection1MaterialHighLight;
                }
                else
                {
                    TrcStructToGeom[trcStruct].BackMaterial = Intersection2MaterialHighLight;
                    TrcStructToGeom[trcStruct].Material = Intersection2MaterialHighLight;
                }
            }
            else if (trcStruct is SlrrLib.Model.TrcLane)
            {
                TrcStructToGeom[trcStruct].BackMaterial = LaneMaterialHighLight;
                TrcStructToGeom[trcStruct].Material = LaneMaterialHighLight;
            }
            else if (trcStruct is SlrrLib.Model.TrcNavigatorNode)
            {
                TrcStructToGeom[trcStruct].BackMaterial = RaceMaterialHighLight;
                TrcStructToGeom[trcStruct].Material = RaceMaterialHighLight;
            }
        }

        private void addSegementFromFloatData(MeshGeometry3D obj, SlrrLib.Model.TrcIntersectionLane floatDat,
                                              float thickness = 3.0f, bool visualizeNormals = false, float HeightPlus = 0.0f)
        {
            addSegementFromFloatData(obj, floatDat.LaneShape, thickness, visualizeNormals, HeightPlus);
        }

        private void addSegementFromFloatData(MeshGeometry3D obj, SlrrLib.Model.TrcSplineSegment floatDat,
                                              float thickness = 3.0f, bool visualizeNormals = false, float HeightPlus = 0.0f, bool addCenter = false)
        {
            Vector3D srcLane = new Vector3D(floatDat.SourceLanePosition.X,
                                            floatDat.SourceLanePosition.Y,
                                            floatDat.SourceLanePosition.Z);
            Vector3D srcLaneDelta = new Vector3D(floatDat.SourceLaneMinusDeltaControlP.X,
                                                 floatDat.SourceLaneMinusDeltaControlP.Y,
                                                 floatDat.SourceLaneMinusDeltaControlP.Z);
            Vector3D trgLane = new Vector3D(floatDat.TargetLanePosition.X,
                                            floatDat.TargetLanePosition.Y,
                                            floatDat.TargetLanePosition.Z);
            Vector3D trgLaneDelta = new Vector3D(floatDat.TargetLaneDeltaControlPoint.X,
                                                 floatDat.TargetLaneDeltaControlPoint.Y,
                                                 floatDat.TargetLaneDeltaControlPoint.Z);

            //srcLaneDelta.Normalize();
            //trgLaneDelta.Normalize();
            addSegmentToMesh(obj, srcLane, trgLane, thickness * 4.0f, 0.1f + HeightPlus
                             , 0.1f, 0.1f + HeightPlus,
                             srcLane - (srcLaneDelta / SlrrLib.Model.Trc.DividerForBezier),
                             trgLane + (trgLaneDelta / SlrrLib.Model.Trc.DividerForBezier), addCenter);
        }
    }
}