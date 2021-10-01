using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Vec3 = System.Windows.Media.Media3D.Vector3D;

namespace SlrrLib.Model
{
    public class Trc
    {
        private class TrcPseudoselfDescription
        {
            public TrcLane referd;
            public float minx;
            public float maxx;
            public float maxxTrue;
        }

        private class TrcSplPosition
        {
            public Vec3 pos;
            public Vec3 normal;
            public float velocity;
        }

        internal static readonly bool loadPrevVersion = false;
        public static readonly double DividerForBezier = 3.14159;

        public int HeaderFirstInt
        {
            get;
            set;
        }

        public List<TrcIntersection> IntersectionData1
        {
            get;
            set;
        } = new List<TrcIntersection>(); //should be count1 long

        public List<TrcIntersection> IntersectionData2
        {
            get;
            set;
        } = new List<TrcIntersection>(); //should be count2 long

        public List<TrcNavigatorNode> RaceNodeDescriptors
        {
            get;
            set;
        } = new List<TrcNavigatorNode>();   //should be count3 long

        public List<TrcLane> LaneData
        {
            get;
            set;
        } = new List<TrcLane>();   //should be count4 long

        public static Trc LoadXML(string fnam)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Trc));
            StreamReader reader = new StreamReader(fnam);
            Trc ret = (Trc)serializer.Deserialize(reader);
            reader.Close();
            return ret;
        }

        public Trc()
        {
        }

        public TrcObject GetFromFlattenedIndex(int index)
        {
            List<TrcObject> flattenedList = new List<TrcObject>();
            flattenedList.Add(new TrcObject());//index 0 is nothing
            flattenedList.AddRange(IntersectionData1);
            flattenedList.AddRange(IntersectionData2);
            flattenedList.AddRange(RaceNodeDescriptors);
            flattenedList.AddRange(LaneData);
            return flattenedList[index];
        }

        public static Trc Load(string fnam)
        {
            Trc ret = new Trc();
            BinaryReader br = new BinaryReader(File.OpenRead(fnam));
            ret.HeaderFirstInt = br.ReadInt32();
            int count1 = br.ReadInt32();
            int count2 = br.ReadInt32();
            int count3 = br.ReadInt32();
            int count4 = br.ReadInt32();
            ret.IntersectionData1 = new List<TrcIntersection>();
            ret.IntersectionData2 = new List<TrcIntersection>();
            ret.RaceNodeDescriptors = new List<TrcNavigatorNode>();
            ret.LaneData = new List<TrcLane>();
            for (int i = 0; i != count1; ++i)
            {
                ret.IntersectionData1.Add(TrcIntersection.Load(br));
            }
            for (int i = 0; i != count2; ++i)
            {
                ret.IntersectionData2.Add(TrcIntersection.Load(br));
            }
            for (int i = 0; i != count3; ++i)
            {
                ret.RaceNodeDescriptors.Add(TrcNavigatorNode.Load(br));
            }
            for (int i = 0; i != count4; ++i)
            {
                ret.LaneData.Add(TrcLane.Load(br));
            }
            br.Close();
            ret.fillFlattenedRefrences();
            return ret;
        }

        public void Write(string fnam, bool bak = true, string BakSignature = "_BAK_trc_")
        {
            if (bak && File.Exists(fnam))
            {
                int bakInd = 0;
                while (File.Exists(fnam + BakSignature + bakInd.ToString()))
                    bakInd++;
                File.Copy(fnam, fnam + BakSignature + bakInd.ToString());
            }
            if (!File.Exists(fnam))
                File.Create(fnam).Close();
            var lst = LaneData.Select(x => x.PseudoSelfIndexInLaneArray).OrderBy(x => x).ToList();
            fillFlattenedIndsFromReferences(false);
            File.Delete(fnam);
            BinaryWriter bw = new BinaryWriter(File.OpenWrite(fnam));
            HeaderFirstInt = 1;
            bw.Write(HeaderFirstInt);
            bw.Write(IntersectionData1.Count);
            bw.Write(IntersectionData2.Count);
            bw.Write(RaceNodeDescriptors.Count);
            bw.Write(LaneData.Count);
            foreach (var dat in IntersectionData1)
            {
                dat.Save(bw);
            }
            foreach (var dat in IntersectionData2)
            {
                dat.Save(bw);
            }
            foreach (var dat in RaceNodeDescriptors)
            {
                dat.Save(bw);
            }
            foreach (var dat in LaneData)
            {
                dat.Save(bw);
            }
            bw.Close();
        }

        public void WriteXML(string fnam)
        {
            XmlSerializer x = new XmlSerializer(typeof(Trc));
            TextWriter writer = new StreamWriter(fnam);
            x.Serialize(writer, this);
            writer.Close();
        }

        public DynamicNamedSpatialData GenNamedDataForIntersection(TrcIntersection cross, DynamicResEntry nativeRoad, int freeTypeID, float margin = 10.0f)
        {
            DynamicNamedSpatialData ret = new DynamicNamedSpatialData();
            ret.TypeID = freeTypeID;
            DynamicSpatialNode emptyArr = new DynamicSpatialNode();
            DynamicRSDInnerEntry rsd = new DynamicRSDInnerEntry();
            ret.OwnedEntries.Add(emptyArr);
            ret.OwnedEntries.Add(rsd);
            if (IntersectionData1.Contains(cross))
            {
                ret.Name = "crossType1";
                rsd.StringData = "gametype 0x" + nativeRoad.TypeID.ToString("X8") + "\r\n" + "params 2;1," + IntersectionData1.IndexOf(cross).ToString();
            }
            else if (IntersectionData2.Contains(cross))
            {
                ret.Name = "crossType2";
                rsd.StringData = "gametype 0x" + nativeRoad.TypeID.ToString("X8") + "\r\n" + "params 3;1," + IntersectionData2.IndexOf(cross).ToString();
            }
            else
            {
                return null;
            }
            cross.SetPositionToAvgFromFloatDatas();
            Vec3 MaxVec = maxFromFloatData(cross.GetAllFloatDatas()) + new Vec3(margin, margin / 10.0, margin) - new Vec3(cross.PositionXData, cross.PositionYData, cross.PositionZData);
            Vec3 MinVec = minFromFloatData(cross.GetAllFloatDatas()) - new Vec3(margin, margin / 10.0, margin) - new Vec3(cross.PositionXData, cross.PositionYData, cross.PositionZData);
            ret.BoundingBoxX = cross.PositionXData;
            ret.BoundingBoxY = cross.PositionYData;
            ret.BoundingBoxZ = cross.PositionZData;
            MaxVec -= MinVec;
            MaxVec *= 0.5;
            ret.BoundingBoxHalfWidthX = (float)MaxVec.X;
            ret.BoundingBoxHalfWidthY = (float)MaxVec.Y;
            ret.BoundingBoxHalfWidthZ = (float)MaxVec.Z;
            ret.UnkownShort1 = 0x101;
            ret.UnkownIsParentCompatible = 0.0f;
            return ret;
        }

        public DynamicNamedSpatialData GenNamedDataForRaceNode(TrcNavigatorNode race, DynamicResEntry nativeRoad, int freeTypeID, float margin = 10.0f)
        {
            DynamicNamedSpatialData ret = new DynamicNamedSpatialData();
            ret.TypeID = freeTypeID;
            DynamicSpatialNode emptyArr = new DynamicSpatialNode();
            DynamicRSDInnerEntry rsd = new DynamicRSDInnerEntry();
            ret.OwnedEntries.Add(emptyArr);
            ret.OwnedEntries.Add(rsd);
            if (RaceNodeDescriptors.Contains(race))
            {
                ret.Name = "road";
                rsd.StringData = "gametype 0x" + nativeRoad.TypeID.ToString("X8") + "\r\n" + "params 1;1," + RaceNodeDescriptors.IndexOf(race).ToString();
            }
            else
            {
                return null;
            }
            race.SetPositionToAvgFromFloatDatas();
            Vec3 MaxVec = maxFromFloatData(race.Spline) + new Vec3(margin, margin / 10.0, margin);
            Vec3 MinVec = minFromFloatData(race.Spline) - new Vec3(margin, margin / 10.0, margin);
            Vec3 avgPos = (MaxVec + MinVec) * 0.5;
            MaxVec -= avgPos;
            MinVec -= avgPos;
            ret.BoundingBoxX = (float)avgPos.X;
            ret.BoundingBoxY = (float)avgPos.Y;
            ret.BoundingBoxZ = (float)avgPos.Z;
            MaxVec -= MinVec;
            MaxVec *= 0.5;
            ret.BoundingBoxHalfWidthX = (float)MaxVec.X;
            ret.BoundingBoxHalfWidthY = (float)MaxVec.Y;
            ret.BoundingBoxHalfWidthZ = (float)MaxVec.Z;
            ret.UnkownShort1 = 0x101;
            ret.UnkownIsParentCompatible = 0.0f;
            return ret;
        }

        public void FillSplineLengthsInRegardToMaxSpeed()
        {
            foreach (var lane in LaneData)
            {
                foreach (var spl in lane.Spline)
                {
                    float newLength = spl.GetLengthOfSpline(100) / lane.GetSpeedDistortionDivider();
                    spl.LengthOfSplineAtSource = newLength;
                    spl.LengthOfSplineAtTarget = newLength;
                }
                lane.SetPositionToAvgFromFloatDatas();
            }
            foreach (var cross in IntersectionData1.Union(IntersectionData2))
            {
                foreach (var crossing in cross.Crossings)
                {
                    var fromLane = cross.LaneRefs[crossing.IndOfFROMLaneInParnet].LaneInFlattened_1;
                    var toLane = cross.LaneRefs[crossing.IndOfTOLaneInParnet].LaneInFlattened_1;
                    crossing.LaneShape.LengthOfSplineAtSource = crossing.LaneShape.GetLengthOfSpline(100) / fromLane.GetSpeedDistortionDivider();
                    crossing.LaneShape.LengthOfSplineAtTarget = crossing.LaneShape.GetLengthOfSpline(100) / toLane.GetSpeedDistortionDivider();
                }
                cross.SetPositionToAvgFromFloatDatas();
            }
        }

        public void RecalcConnectionsForLane(TrcLane lane)
        {
            foreach (var cross in IntersectionData1.Union(IntersectionData2))
            {
                var refed = cross.LaneRefs.FirstOrDefault(x => x.LaneInFlattened_1 == lane);
                if (refed != null)
                {
                    bool found = false;
                    foreach (var crossLane in cross.Crossings)
                    {
                        if ((crossLane.LaneShape.SourceLanePosition - lane.EndPoint).Length < 0.1)
                        {
                            found = true;
                            refed.IsInboundLane = 1;
                            lane.EndPoint = crossLane.LaneShape.SourceLanePosition;
                            var vecCpy = crossLane.LaneShape.SourceLaneMinusDeltaControlP;
                            vecCpy.Normalize();
                            lane.EndNormal = vecCpy * lane.EndNormal.Length;
                        }
                        if ((crossLane.LaneShape.TargetLanePosition - lane.StartPoint).Length < 0.1)
                        {
                            found = true;
                            refed.IsInboundLane = 0;
                            lane.StartPoint = crossLane.LaneShape.TargetLanePosition;
                            var vecCpy = crossLane.LaneShape.TargetLaneDeltaControlPoint;
                            vecCpy.Normalize();
                            lane.StartNormal = vecCpy * lane.StartNormal.Length;
                        }
                    }
                    if (!found)
                        cross.LaneRefs.Remove(refed);
                }
                else
                {
                    refed = new TrcIntersectionLaneReference();
                    refed.LaneInFlattened_1 = lane;
                    bool found = false;
                    foreach (var crossLane in cross.Crossings)
                    {
                        if ((crossLane.LaneShape.SourceLanePosition - lane.EndPoint).Length < 0.1)
                        {
                            found = true;
                            refed.IsInboundLane = 1;
                            lane.EndPoint = crossLane.LaneShape.SourceLanePosition;
                            var vecCpy = crossLane.LaneShape.SourceLaneMinusDeltaControlP;
                            vecCpy.Normalize();
                            lane.EndNormal = vecCpy * lane.EndNormal.Length;
                        }
                        if ((crossLane.LaneShape.TargetLanePosition - lane.StartPoint).Length < 0.1)
                        {
                            found = true;
                            refed.IsInboundLane = 0;
                            lane.StartPoint = crossLane.LaneShape.TargetLanePosition;
                            var vecCpy = crossLane.LaneShape.TargetLaneDeltaControlPoint;
                            vecCpy.Normalize();
                            lane.StartNormal = vecCpy * lane.StartNormal.Length;
                        }
                    }
                    if (found)
                    {
                        if (!cross.LaneRefs.Any(x => x.LaneInFlattened_1 == refed.LaneInFlattened_1))
                            cross.LaneRefs.Add(refed);
                    }
                }
            }
        }

        public void RecalcConnectionsForIntersection(TrcIntersection cross)
        {
            cross.LaneRefs.Clear();
            foreach (var lane in LaneData)
            {
                foreach (var crossLane in cross.Crossings)
                {
                    if ((crossLane.LaneShape.SourceLanePosition - lane.EndPoint).Length < 0.1)
                    {
                        var refed = new TrcIntersectionLaneReference();
                        refed.LaneInFlattened_1 = lane;
                        refed.IsInboundLane = 1;
                        lane.EndPoint = crossLane.LaneShape.SourceLanePosition;
                        var vecCpy = crossLane.LaneShape.SourceLaneMinusDeltaControlP;
                        vecCpy.Normalize();
                        lane.EndNormal = vecCpy * lane.EndNormal.Length;
                        if (!cross.LaneRefs.Any(x => x.LaneInFlattened_1 == refed.LaneInFlattened_1))
                            cross.LaneRefs.Add(refed);
                        break;
                    }
                    if ((crossLane.LaneShape.TargetLanePosition - lane.StartPoint).Length < 0.1)
                    {
                        var refed = new TrcIntersectionLaneReference();
                        refed.LaneInFlattened_1 = lane;
                        refed.IsInboundLane = 0;
                        lane.StartPoint = crossLane.LaneShape.TargetLanePosition;
                        var vecCpy = crossLane.LaneShape.TargetLaneDeltaControlPoint;
                        vecCpy.Normalize();
                        lane.StartNormal = vecCpy * lane.StartNormal.Length;
                        if (!cross.LaneRefs.Any(x => x.LaneInFlattened_1 == refed.LaneInFlattened_1))
                            cross.LaneRefs.Add(refed);
                        break;
                    }
                }
            }
        }

        public void RemoveLane(TrcLane lane)
        {
            LaneData.Remove(lane);
            foreach (var cross in IntersectionData1.Union(IntersectionData2))
            {
                foreach (var laneRef in cross.LaneRefs.ToList())
                {
                    if (laneRef.LaneInFlattened_1 == lane)
                    {
                        cross.LaneRefs.Remove(laneRef);
                    }
                }
            }
            foreach (var other in LaneData)
            {
                if (other.LaneInFlattened_1 == lane)
                    other.LaneInFlattened_1 = null;
                if (other.LeftAdjacentLane == lane)
                    other.LeftAdjacentLane = null;
                if (other.RightAdjacentLane == lane)
                    other.RightAdjacentLane = null;
            }
            foreach (var race in RaceNodeDescriptors)
            {
                if (race.LaneInFlattened_1 == lane)
                    race.LaneInFlattened_1 = null;
            }
        }

        public void GenerateAdjacentLanesForLane(TrcLane lane)
        {
            foreach (var cross1 in IntersectionData1.Union(IntersectionData2))
            {
                foreach (var cross2 in IntersectionData1.Union(IntersectionData2))
                {
                    if (cross1 == cross2)
                        continue;
                    if (cross1.LaneRefs.Any(x => x.LaneInFlattened_1 == lane) && cross2.LaneRefs.Any(x => x.LaneInFlattened_1 == lane))
                    {
                        var startingCross = cross1;
                        if (cross2.LaneRefs.First(x => x.LaneInFlattened_1 == lane).IsInboundLane == 0)
                            startingCross = cross2;
                        List<TrcLane> mutualOutLanes = cross1.LaneRefs.Select(x => x.LaneInFlattened_1)
                                                       .Intersect(cross2.LaneRefs.Select(x => x.LaneInFlattened_1))
                                                       .Where(x => startingCross.LaneRefs.Any(y => y.IsInboundLane == 0 && y.LaneInFlattened_1 == x)).ToList();
                        if (mutualOutLanes.Count == 1)
                        {
                            mutualOutLanes[0].LeftAdjacentLane = null;
                            mutualOutLanes[0].RightAdjacentLane = null;
                            return;
                        }
                        if (mutualOutLanes.Count == 2)
                        {
                            mutualOutLanes[0].LeftAdjacentLane = mutualOutLanes[1];
                            mutualOutLanes[0].RightAdjacentLane = null;
                            mutualOutLanes[1].LeftAdjacentLane = mutualOutLanes[0];
                            mutualOutLanes[1].RightAdjacentLane = null;
                            mutualOutLanes[0].SetProperAdjacencyOrder();
                            mutualOutLanes[1].SetProperAdjacencyOrder();
                            return;
                        }
                        List<TrcLane> maxDistLanes = new List<TrcLane>();
                        double maxDist = -1;
                        for (int i = 0; i != mutualOutLanes.Count; ++i)
                        {
                            for (int j = i + 1; j < mutualOutLanes.Count; ++j)
                            {
                                double porDist = (mutualOutLanes[i].StartPoint - mutualOutLanes[j].StartPoint).Length;
                                if (porDist > maxDist)
                                {
                                    maxDistLanes.Clear();
                                    maxDistLanes.Add(mutualOutLanes[i]);
                                    maxDistLanes.Add(mutualOutLanes[j]);
                                    maxDist = porDist;
                                }
                            }
                        }
                        foreach (var l in mutualOutLanes)
                        {
                            l.LeftAdjacentLane = null;
                            l.RightAdjacentLane = null;
                            var closest = closestTwoByStartPoint(mutualOutLanes, l);
                            if ((closest[0].StartPoint - l.StartPoint).Length < maxDist - 0.1)
                                l.LeftAdjacentLane = closest[0];
                            if ((closest[1].StartPoint - l.StartPoint).Length < maxDist - 0.1)
                            {
                                if (l.LeftAdjacentLane == null)
                                    l.LeftAdjacentLane = closest[1];
                                else
                                    l.RightAdjacentLane = closest[1];
                            }
                            l.SetProperAdjacencyOrder();
                        }
                        return;
                    }
                }
            }
        }

        public List<TrcIntersection> GetRelatedIntersections(TrcLane l)
        {
            List<TrcIntersection> ret = new List<TrcIntersection>();
            foreach (var cross1 in IntersectionData1.Union(IntersectionData2))
            {
                if (cross1.LaneRefs.Any(x => x.LaneInFlattened_1 == l))
                    ret.Add(cross1);
            }
            return ret;
        }

        public void RegenerateRaceEntries()
        {
            RaceNodeDescriptors.Clear();
            foreach (var lane in LaneData)
            {
                lane.RaceNodeInFlattened_1 = null;
            }
            var connectionLookUp = new Dictionary<TrcIntersection, Dictionary<TrcIntersection, HashSet<TrcLane>>>();
            foreach (var cross1 in IntersectionData1.Union(IntersectionData2))
            {
                connectionLookUp[cross1] = new Dictionary<TrcIntersection, HashSet<TrcLane>>();
                foreach (var cross2 in IntersectionData1.Union(IntersectionData2))
                {
                    connectionLookUp[cross1][cross2] = new HashSet<TrcLane>();
                    int laneref1_i = 0;
                    foreach (var laneref1 in cross1.LaneRefs)
                    {
                        int laneref2_i = 0;
                        foreach (var laneref2 in cross2.LaneRefs)
                        {
                            if (laneref1.LaneInFlattened_1 == laneref2.LaneInFlattened_1)
                            {
                                if (laneref1.IsInboundLane == 0 && laneref2.IsInboundLane == 1)
                                {
                                    connectionLookUp[cross1][cross2].Add(laneref1.LaneInFlattened_1);
                                    laneref1.LaneInFlattened_1.IndexOfMeInParentIntersection_1 = laneref1_i;//intentionally switched the map-path drawing is better this way
                                    laneref1.LaneInFlattened_1.IndexOfMeInParentIntersection_2 = laneref2_i;
                                    laneref1.LaneInFlattened_1.AreIndicesRefsInReverseAsInTheRaceNode = 0;
                                }
                            }
                            laneref2_i++;
                        }
                        laneref1_i++;
                    }
                }
            }
            HashSet<TrcIntersection> processed = new HashSet<TrcIntersection>();
            foreach (var cross1 in IntersectionData1.Union(IntersectionData2))
            {
                foreach (var cross2 in IntersectionData1.Union(IntersectionData2))
                {
                    if (processed.Contains(cross2))
                        continue;
                    var connectingLanesTo = connectionLookUp[cross1][cross2];
                    var connectingLanesFrom = connectionLookUp[cross2][cross1];
                    if (connectingLanesTo.Any() || connectingLanesFrom.Any())
                    {
                        var connectingLanesFromNormalizeDirection = connectingLanesFrom.Select(x => x.Copy()).ToList();
                        foreach (var toRev in connectingLanesFromNormalizeDirection)
                            toRev.ReverseLaneInPlace();
                        var normalizedDirectionLanes = connectingLanesTo.Union(connectingLanesFromNormalizeDirection).ToList();
                        //lets make the race node cross1->cross2
                        int minNumOfNodes = normalizedDirectionLanes.Min(x => x.Spline.Count);
                        TrcNavigatorNode avgRaceNode = new TrcNavigatorNode();
                        cross1.SetPositionToAvgFromFloatDatas();
                        cross2.SetPositionToAvgFromFloatDatas();
                        avgRaceNode.IntersectionInFlattened_1 = cross1;//intentionally switched the map-path drawing is better this way
                        avgRaceNode.IntersectionInFlattened_2 = cross2;
                        //build hierarchy to get the lanes loaded
                        var allLanes = connectingLanesTo.Union(connectingLanesFrom).ToList();
                        avgRaceNode.LaneInFlattened_1 = allLanes[0];
                        for (int i = 1; i < allLanes.Count; ++i)
                        {
                            allLanes[i - 1].LaneInFlattened_1 = allLanes[i];
                            allLanes[i].LaneInFlattened_1 = null;
                        }
                        int b = 0;
                        List<TrcSplPosition> fromBeginning = new List<TrcSplPosition>();
                        List<TrcSplPosition> fromEnd = new List<TrcSplPosition>();
                        if (minNumOfNodes == 1)
                        {
                            TrcSplPosition beg1 = new TrcSplPosition();
                            TrcSplPosition beg2 = new TrcSplPosition();
                            var filteredLanes = normalizedDirectionLanes.Where(x => x.Spline.Count == 1).ToList();
                            foreach (var partLane in filteredLanes)
                            {
                                beg1.pos += partLane.Spline[0].SourceLanePosition;
                                beg1.velocity += partLane.Spline[0].LengthOfSplineAtSource;
                                beg1.normal += partLane.Spline[0].SourceLaneMinusDeltaControlP;
                                beg2.pos += partLane.Spline[0].TargetLanePosition;
                                beg2.velocity += partLane.Spline[0].LengthOfSplineAtTarget;
                                beg2.normal += partLane.Spline[0].TargetLaneDeltaControlPoint;
                            }
                            beg1.pos /= (float)filteredLanes.Count;
                            beg1.normal /= (float)filteredLanes.Count;
                            beg1.velocity /= (float)filteredLanes.Count;
                            beg2.pos /= (float)filteredLanes.Count;
                            beg2.normal /= (float)filteredLanes.Count;
                            beg2.velocity /= (float)filteredLanes.Count;
                            fromBeginning.Add(beg1);
                            fromBeginning.Add(beg2);
                        }
                        while (b < minNumOfNodes - 1 - b)
                        {
                            TrcSplPosition beg1 = new TrcSplPosition();
                            TrcSplPosition end1 = new TrcSplPosition();
                            TrcSplPosition beg2 = new TrcSplPosition();
                            TrcSplPosition end2 = new TrcSplPosition();
                            foreach (var partLane in normalizedDirectionLanes)
                            {
                                end1.pos += partLane.Spline[b].TargetLanePosition;
                                end1.velocity += partLane.Spline[b].LengthOfSplineAtTarget;
                                end1.normal += partLane.Spline[b].TargetLaneDeltaControlPoint;
                                end2.pos += partLane.Spline[b].SourceLanePosition;
                                end2.velocity += partLane.Spline[b].LengthOfSplineAtSource;
                                end2.normal += partLane.Spline[b].SourceLaneMinusDeltaControlP;
                            }
                            foreach (var partLane in normalizedDirectionLanes)
                            {
                                beg1.pos += partLane.Spline[partLane.Spline.Count - 1 - b].SourceLanePosition;
                                beg1.velocity += partLane.Spline[partLane.Spline.Count - 1 - b].LengthOfSplineAtSource;
                                beg1.normal += partLane.Spline[partLane.Spline.Count - 1 - b].SourceLaneMinusDeltaControlP;
                                beg2.pos += partLane.Spline[partLane.Spline.Count - 1 - b].TargetLanePosition;
                                beg2.velocity += partLane.Spline[partLane.Spline.Count - 1 - b].LengthOfSplineAtTarget;
                                beg2.normal += partLane.Spline[partLane.Spline.Count - 1 - b].TargetLaneDeltaControlPoint;
                            }
                            beg1.pos /= (float)normalizedDirectionLanes.Count;
                            beg1.normal /= (float)normalizedDirectionLanes.Count;
                            beg1.velocity /= (float)normalizedDirectionLanes.Count;
                            beg2.pos /= (float)normalizedDirectionLanes.Count;
                            beg2.normal /= (float)normalizedDirectionLanes.Count;
                            beg2.velocity /= (float)normalizedDirectionLanes.Count;
                            fromBeginning.Add(beg1);
                            fromBeginning.Add(beg2);
                            end1.pos /= (float)normalizedDirectionLanes.Count;
                            end1.normal /= (float)normalizedDirectionLanes.Count;
                            end1.velocity /= (float)normalizedDirectionLanes.Count;
                            end2.pos /= (float)normalizedDirectionLanes.Count;
                            end2.normal /= (float)normalizedDirectionLanes.Count;
                            end2.velocity /= (float)normalizedDirectionLanes.Count;
                            fromEnd.Add(end1);
                            fromEnd.Add(end2);
                            if ((b + 1) >= minNumOfNodes - b - 2 && minNumOfNodes % 2 == 1)
                            {
                                end1 = new TrcSplPosition();
                                end2 = new TrcSplPosition();
                                foreach (var partLane in normalizedDirectionLanes)
                                {
                                    end1.pos += partLane.Spline[b + 1].TargetLanePosition;
                                    end1.velocity += partLane.Spline[b + 1].LengthOfSplineAtTarget;
                                    end1.normal += partLane.Spline[b + 1].TargetLaneDeltaControlPoint;
                                    end2.pos += partLane.Spline[b + 1].SourceLanePosition;
                                    end2.velocity += partLane.Spline[b + 1].LengthOfSplineAtSource;
                                    end2.normal += partLane.Spline[b + 1].SourceLaneMinusDeltaControlP;
                                }
                                end1.pos /= (float)normalizedDirectionLanes.Count;
                                end1.normal /= (float)normalizedDirectionLanes.Count;
                                end1.velocity /= (float)normalizedDirectionLanes.Count;
                                end2.pos /= (float)normalizedDirectionLanes.Count;
                                end2.normal /= (float)normalizedDirectionLanes.Count;
                                end2.velocity /= (float)normalizedDirectionLanes.Count;
                                fromEnd.Add(end1);
                                fromEnd.Add(end2);
                            }
                            b++;
                        }
                        fromEnd.Reverse();
                        fromBeginning.AddRange(fromEnd);
                        fromBeginning.Reverse();
                        for (int i = 0; i != fromBeginning.Count - 1; ++i)
                        {
                            if ((fromBeginning[i].pos - fromBeginning[i + 1].pos).Length < 1.0)
                                continue;
                            TrcSplineSegment spl = new TrcSplineSegment();
                            spl.SourceLanePosition = fromBeginning[i + 1].pos;
                            spl.TargetLanePosition = fromBeginning[i].pos;
                            spl.SourceLaneMinusDeltaControlP = fromBeginning[i + 1].normal;
                            spl.TargetLaneDeltaControlPoint = fromBeginning[i].normal;
                            spl.LengthOfSplineAtSource = fromBeginning[i + 1].velocity;
                            spl.LengthOfSplineAtTarget = fromBeginning[i].velocity;
                            spl.UnkByte_1 = 1;
                            avgRaceNode.Spline.Add(spl);
                        }
                        avgRaceNode.CheckFlow();
                        avgRaceNode.SetPositionToAvgFromFloatDatas();
                        foreach (var partLane in allLanes)
                        {
                            partLane.RaceNodeInFlattened_1 = avgRaceNode;
                        }
                        RaceNodeDescriptors.Add(avgRaceNode);
                        foreach (var toReverse in connectingLanesFrom)
                        {
                            toReverse.AreIndicesRefsInReverseAsInTheRaceNode = 1;
                        }
                    }
                }
                processed.Add(cross1);
            }
        }

        public void FillIntersectionRefIndices()
        {
            foreach (var inter in IntersectionData1.Union(IntersectionData2))
            {
                foreach (var crossing in inter.Crossings)
                {
                    int indOfClosestLaneEnd = -1;
                    double minDist = double.MaxValue;
                    for (int i = 0; i != inter.LaneRefs.Count; ++i)
                    {
                        double cand = (crossing.LaneShape.SourceLanePosition - inter.LaneRefs[i].LaneInFlattened_1.EndPoint).Length;
                        if (cand < minDist)
                        {
                            indOfClosestLaneEnd = i;
                            minDist = cand;
                        }
                    }
                    if (indOfClosestLaneEnd == -1)
                        throw new Exception("Could not find source point for intersection lane");
                    if (crossing.IndOfFROMLaneInParnet != (byte)indOfClosestLaneEnd)
                    {
                        crossing.IndOfFROMLaneInParnet = (byte)indOfClosestLaneEnd;
                        crossing.LaneShape.SourceLanePosition = inter.LaneRefs[indOfClosestLaneEnd].LaneInFlattened_1.EndPoint;
                    }
                    if (inter.LaneRefs[indOfClosestLaneEnd].IsInboundLane == 0)
                        inter.LaneRefs[indOfClosestLaneEnd].IsInboundLane = 1;
                }
                foreach (var crossing in inter.Crossings)
                {
                    int indOfClosestLaneEnd = -1;
                    double minDist = double.MaxValue;
                    for (int i = 0; i != inter.LaneRefs.Count; ++i)
                    {
                        double cand = (crossing.LaneShape.TargetLanePosition - inter.LaneRefs[i].LaneInFlattened_1.StartPoint).Length;
                        if (cand < minDist)
                        {
                            indOfClosestLaneEnd = i;
                            minDist = cand;
                        }
                    }
                    if (indOfClosestLaneEnd == -1)
                        throw new Exception("Could not find source point for intersection lane");
                    if (crossing.IndOfTOLaneInParnet != (byte)indOfClosestLaneEnd)
                    {
                        crossing.IndOfTOLaneInParnet = (byte)indOfClosestLaneEnd;
                        crossing.LaneShape.TargetLanePosition = inter.LaneRefs[indOfClosestLaneEnd].LaneInFlattened_1.StartPoint;
                    }
                    if (inter.LaneRefs[indOfClosestLaneEnd].IsInboundLane == 1)
                        inter.LaneRefs[indOfClosestLaneEnd].IsInboundLane = 0;
                }
                Dictionary<TrcLane, TrcIntersection> theOtherIntersection = new Dictionary<TrcLane, TrcIntersection>();
                List<TrcIntersection> othersOrder = new List<TrcIntersection>();
                foreach (var crossLane in inter.Crossings)
                {
                    var allRealted = GetRelatedIntersections(inter.LaneRefs[crossLane.IndOfFROMLaneInParnet].LaneInFlattened_1);
                    var other = allRealted.FirstOrDefault(x => x != inter);
                    theOtherIntersection[inter.LaneRefs[crossLane.IndOfFROMLaneInParnet].LaneInFlattened_1] = other;
                    if (!othersOrder.Contains(other))
                        othersOrder.Add(other);
                }
                inter.Crossings = inter.Crossings.OrderBy(x => x.IndOfFROMLaneInParnet).ToList();
                inter.Crossings = inter.Crossings.OrderBy(x => othersOrder.IndexOf(theOtherIntersection[inter.LaneRefs[x.IndOfFROMLaneInParnet].LaneInFlattened_1])).ToList();
            }
        }

        private Vec3 maxFromFloatData(IEnumerable<TrcSplineSegment> coll)
        {
            Vec3 max = new Vec3(float.MinValue, float.MinValue, float.MinValue);
            if (!coll.Any())
                return max;
            Vec3 tmp;
            foreach (var vec in coll)
            {
                tmp = vec.GetMaxPosition();
                if (max.X < tmp.X)
                    max.X = tmp.X;
                if (max.Y < tmp.Y)
                    max.Y = tmp.Y;
                if (max.Z < tmp.Z)
                    max.Z = tmp.Z;
            }
            return max;
        }

        private Vec3 minFromFloatData(IEnumerable<TrcSplineSegment> coll)
        {
            Vec3 min = new Vec3(float.MaxValue, float.MaxValue, float.MaxValue);
            if (!coll.Any())
                return min;
            Vec3 tmp;
            foreach (var vec in coll)
            {
                tmp = vec.GetMinPosition();
                if (min.X > tmp.X)
                    min.X = tmp.X;
                if (min.Y > tmp.Y)
                    min.Y = tmp.Y;
                if (min.Z > tmp.Z)
                    min.Z = tmp.Z;
            }
            return min;
        }

        private Vec3 posAtParam(TrcSplineSegment spl, float t)
        {
            Vec3 p1 = spl.SourceLanePosition - (spl.SourceLaneMinusDeltaControlP / DividerForBezier);
            Vec3 p2 = spl.TargetLanePosition - (spl.TargetLaneDeltaControlPoint / DividerForBezier);
            return Math.Pow(1 - t, 3) * spl.SourceLanePosition +
                   3 * Math.Pow(1 - t, 2) * t * p1 +
                   3 * (1 - t) * t * t * p2 +
                   t * t * t * spl.TargetLanePosition;
        }

        private Vec3 middlePoint(IEnumerable<TrcSplineSegment> coll)
        {
            int count = coll.Count();
            if (count % 2 == 1)
            {
                TrcSplineSegment middleelem = null;
                if (count == 1)
                    middleelem = coll.Last();
                else
                    middleelem = coll.ElementAt((count + 1) / 2);
                return posAtParam(middleelem, 0.5f);
            }
            return coll.ElementAt((count / 2)).TargetLanePosition;
        }

        private void fillFlattenedRefrences()
        {
            List<TrcObject> flattenedArray = new List<TrcObject>();
            flattenedArray.Add(null);
            flattenedArray.AddRange(IntersectionData1);
            flattenedArray.AddRange(IntersectionData2);
            flattenedArray.AddRange(RaceNodeDescriptors);
            flattenedArray.AddRange(LaneData);
            foreach (var inters in IntersectionData1)
            {
                foreach (var data in inters.LaneRefs)
                {
                    if (flattenedArray[data.LaneIndInFlattened] is TrcLane)
                        data.LaneInFlattened_1 = flattenedArray[data.LaneIndInFlattened] as TrcLane;
                    else
                        throw new Exception("Non Lane entry refed in WeightedTRCDat");
                }
            }
            foreach (var inters in IntersectionData2)
            {
                foreach (var data in inters.LaneRefs)
                {
                    if (flattenedArray[data.LaneIndInFlattened] is TrcLane)
                        data.LaneInFlattened_1 = flattenedArray[data.LaneIndInFlattened] as TrcLane;
                    else
                        throw new Exception("Non Lane entry refed in WeightedTRCDat");
                }
            }
            foreach (var lane in LaneData)
            {
                if (flattenedArray[lane.ZeroableLaneIndInFlattened_1] == null || flattenedArray[lane.ZeroableLaneIndInFlattened_1] is TrcLane)
                    lane.LaneInFlattened_1 = flattenedArray[lane.ZeroableLaneIndInFlattened_1] as TrcLane;
                else
                    throw new Exception("Non Lane entry refed in Lane ZeroableLaneIndInFlattened_2");
                if (flattenedArray[lane.ZeroableLaneIndInFlattened_2] == null || flattenedArray[lane.ZeroableLaneIndInFlattened_2] is TrcLane)
                    lane.LeftAdjacentLane = flattenedArray[lane.ZeroableLaneIndInFlattened_2] as TrcLane;
                else
                    throw new Exception("Non Lane entry refed in Lane ZeroableLaneIndInFlattened_3");
                if (flattenedArray[lane.ZeroableLaneIndInFlattened_3] == null || flattenedArray[lane.ZeroableLaneIndInFlattened_3] is TrcLane)
                    lane.RightAdjacentLane = flattenedArray[lane.ZeroableLaneIndInFlattened_3] as TrcLane;
                else
                    throw new Exception("Non Lane entry refed in Lane ZeroableLaneIndInFlattened_3");

                lane.InFlattened_1 = flattenedArray[lane.ZeroableIndInFlattened_1];
                lane.InFlattened_2 = flattenedArray[lane.ZeroableIndInFlattened_2];

                if (flattenedArray[lane.RaceNodeIndInFlattened] is TrcNavigatorNode)
                    lane.RaceNodeInFlattened_1 = flattenedArray[lane.RaceNodeIndInFlattened] as TrcNavigatorNode;
                else
                    throw new Exception("Non RaceNode entry refed in Lane RaceNodeIndInFlattened");
            }
            foreach (var race in RaceNodeDescriptors)
            {
                if (flattenedArray[race.LaneIndInFlattened] is TrcLane)
                    race.LaneInFlattened_1 = flattenedArray[race.LaneIndInFlattened] as TrcLane;
                else
                    throw new Exception("Non Lane entry refed in Lane LaneIndInFlattened");

                if (flattenedArray[race.IntersectionIndInFlattened_1] is TrcIntersection)
                    race.IntersectionInFlattened_1 = flattenedArray[race.IntersectionIndInFlattened_1] as TrcIntersection;
                else
                    throw new Exception("Non Intersection entry refed in Lane IntersectionIndInFlattened_1");
                if (flattenedArray[race.IntersectionIndInFlattened_2] is TrcIntersection)
                    race.IntersectionInFlattened_2 = flattenedArray[race.IntersectionIndInFlattened_2] as TrcIntersection;
                else
                    throw new Exception("Non Intersection entry refed in Lane IntersectionIndInFlattened_2");
            }
        }

        private void fillFlattenedIndsFromReferences(bool addMissingButReferencedObjs = true)
        {
            List<object> flattenedArray = new List<object>();
            bool foundEntryOutsideOfArray = true;
            while (foundEntryOutsideOfArray)
            {
                foundEntryOutsideOfArray = false;
                flattenedArray = new List<object>();
                flattenedArray.Add(null);
                flattenedArray.AddRange(IntersectionData1);
                flattenedArray.AddRange(IntersectionData2);
                flattenedArray.AddRange(RaceNodeDescriptors);
                flattenedArray.AddRange(LaneData);
                foreach (var inters in IntersectionData1)
                {
                    foreach (var data in inters.LaneRefs.ToList())
                    {
                        if (data.LaneInFlattened_1 == null)
                        {
                            inters.LaneRefs.Remove(data);
                        }
                        if (!flattenedArray.Contains(data.LaneInFlattened_1))
                        {
                            if (addMissingButReferencedObjs)
                            {
                                foundEntryOutsideOfArray = true;
                                LaneData.Add(data.LaneInFlattened_1);
                            }
                            else
                            {
                                inters.LaneRefs.Remove(data);
                            }
                        }
                    }
                }
                foreach (var inters in IntersectionData2)
                {
                    foreach (var data in inters.LaneRefs.ToList())
                    {
                        if (data.LaneInFlattened_1 == null)
                        {
                            inters.LaneRefs.Remove(data);
                        }
                        if (!flattenedArray.Contains(data.LaneInFlattened_1))
                        {
                            if (addMissingButReferencedObjs)
                            {
                                foundEntryOutsideOfArray = true;
                                LaneData.Add(data.LaneInFlattened_1);
                            }
                            else
                            {
                                inters.LaneRefs.Remove(data);
                            }
                        }
                    }
                }
                foreach (var lane in LaneData.ToList())
                {
                    //this should have a better implementation
                    if (!flattenedArray.Contains(lane.LaneInFlattened_1))
                    {
                        if (addMissingButReferencedObjs)
                        {
                            foundEntryOutsideOfArray = true;
                            LaneData.Add(lane.LaneInFlattened_1);
                        }
                        else
                        {
                            lane.LaneInFlattened_1 = null;
                        }
                    }
                    if (!flattenedArray.Contains(lane.LeftAdjacentLane))
                    {
                        if (addMissingButReferencedObjs)
                        {
                            foundEntryOutsideOfArray = true;
                            LaneData.Add(lane.LeftAdjacentLane);
                        }
                        else
                        {
                            lane.LeftAdjacentLane = null;
                        }
                    }
                    if (!flattenedArray.Contains(lane.RightAdjacentLane))
                    {
                        if (addMissingButReferencedObjs)
                        {
                            foundEntryOutsideOfArray = true;
                            LaneData.Add(lane.RightAdjacentLane);
                        }
                        else
                        {
                            lane.RightAdjacentLane = null;
                        }
                    }

                    //no clue what to do with these no example in city.trc but will assume they are lane refs
                    //lane.InFlattened_1 = flattenedArray[lane.ZeroableIndInFlattened_1];
                    //lane.InFlattened_2 = flattenedArray[lane.ZeroableIndInFlattened_2];
                    if (!flattenedArray.Contains(lane.InFlattened_1))
                    {
                        if (addMissingButReferencedObjs)
                        {
                            foundEntryOutsideOfArray = true;
                            LaneData.Add(lane.RightAdjacentLane);
                        }
                        else
                        {
                            lane.RightAdjacentLane = null;
                        }
                    }
                    if (!flattenedArray.Contains(lane.InFlattened_2))
                    {
                        if (addMissingButReferencedObjs)
                        {
                            foundEntryOutsideOfArray = true;
                            LaneData.Add(lane.RightAdjacentLane);
                        }
                        else
                        {
                            lane.RightAdjacentLane = null;
                        }
                    }

                    if (lane.RaceNodeInFlattened_1 == null)
                        throw new Exception("Can not be null");
                    if (!flattenedArray.Contains(lane.RaceNodeInFlattened_1))
                    {
                        if (addMissingButReferencedObjs)
                        {
                            foundEntryOutsideOfArray = true;
                            RaceNodeDescriptors.Add(lane.RaceNodeInFlattened_1);
                        }
                        else
                        {
                            throw new Exception("Lane refed Race node not in flattened array");
                        }
                    }

                    //test race node validity
                    if (lane.AreIndicesRefsInReverseAsInTheRaceNode == 1)
                    {
                        if (lane.RaceNodeInFlattened_1.IntersectionInFlattened_2.LaneRefs[lane.IndexOfMeInParentIntersection_1].LaneInFlattened_1 != lane)
                            throw new Exception("Race node index refs are invalid (IndexOfMeInParentIntersection_1)");
                        if (lane.RaceNodeInFlattened_1.IntersectionInFlattened_1.LaneRefs[lane.IndexOfMeInParentIntersection_2].LaneInFlattened_1 != lane)
                            throw new Exception("Race node index refs are invalid (IndexOfMeInParentIntersection_2)");
                    }
                    else
                    {
                        if (lane.RaceNodeInFlattened_1.IntersectionInFlattened_1.LaneRefs[lane.IndexOfMeInParentIntersection_1].LaneInFlattened_1 != lane)
                            throw new Exception("Race node index refs are invalid (IndexOfMeInParentIntersection_1)");
                        if (lane.RaceNodeInFlattened_1.IntersectionInFlattened_2.LaneRefs[lane.IndexOfMeInParentIntersection_2].LaneInFlattened_1 != lane)
                            throw new Exception("Race node index refs are invalid (IndexOfMeInParentIntersection_2)");
                    }
                }
                foreach (var race in RaceNodeDescriptors)
                {
                    if (race.LaneInFlattened_1 == null)
                        throw new Exception("Can not be null");
                    if (race.IntersectionInFlattened_1 == null)
                        throw new Exception("Can not be null");
                    if (race.IntersectionInFlattened_2 == null)
                        throw new Exception("Can not be null");
                    if (!flattenedArray.Contains(race.LaneInFlattened_1))
                    {
                        if (addMissingButReferencedObjs)
                        {
                            foundEntryOutsideOfArray = true;
                            LaneData.Add(race.LaneInFlattened_1);
                        }
                        else
                        {
                            throw new Exception("Race lane refed not in flattened array");
                        }
                    }
                    if (!flattenedArray.Contains(race.IntersectionInFlattened_1))
                    {
                        if (addMissingButReferencedObjs)
                        {
                            foundEntryOutsideOfArray = true;
                            IntersectionData2.Add(race.IntersectionInFlattened_1);
                        }
                        else
                        {
                            throw new Exception("Race refed intersection1 not in flatten array");
                        }
                    }
                    if (!flattenedArray.Contains(race.IntersectionInFlattened_2))
                    {
                        if (addMissingButReferencedObjs)
                        {
                            foundEntryOutsideOfArray = true;
                            IntersectionData2.Add(race.IntersectionInFlattened_2);
                        }
                        else
                        {
                            throw new Exception("Race refed intersection2 not in flatten array");
                        }
                    }
                }
            }
            foreach (var inters in IntersectionData1)
            {
                foreach (var data in inters.LaneRefs)
                {
                    data.LaneIndInFlattened = flattenedArray.IndexOf(data.LaneInFlattened_1);
                }
            }
            foreach (var inters in IntersectionData2)
            {
                foreach (var data in inters.LaneRefs)
                {
                    data.LaneIndInFlattened = flattenedArray.IndexOf(data.LaneInFlattened_1);
                }
            }
            foreach (var lane in LaneData)
            {
                lane.ZeroableLaneIndInFlattened_1 = flattenedArray.IndexOf(lane.LaneInFlattened_1);
                lane.ZeroableLaneIndInFlattened_2 = flattenedArray.IndexOf(lane.LeftAdjacentLane);
                lane.ZeroableLaneIndInFlattened_3 = flattenedArray.IndexOf(lane.RightAdjacentLane);
                lane.ZeroableIndInFlattened_1 = flattenedArray.IndexOf(lane.InFlattened_1);
                lane.ZeroableIndInFlattened_2 = flattenedArray.IndexOf(lane.InFlattened_2);
                lane.RaceNodeIndInFlattened = flattenedArray.IndexOf(lane.RaceNodeInFlattened_1);
            }
            foreach (var race in RaceNodeDescriptors)
            {
                race.LaneIndInFlattened = flattenedArray.IndexOf(race.LaneInFlattened_1);
                race.IntersectionIndInFlattened_1 = flattenedArray.IndexOf(race.IntersectionInFlattened_1);
                race.IntersectionIndInFlattened_2 = flattenedArray.IndexOf(race.IntersectionInFlattened_2);
            }
            fillPseudoSelfsAndBoundsOfLanes(5.0f);
        }

        private void fillPseudoSelfsAndBoundsOfLanes(float margin = 3.0f)
        {
            List<TrcPseudoselfDescription> pseudSelves = new List<TrcPseudoselfDescription>();
            foreach (var lane in LaneData)
            {
                TrcPseudoselfDescription toad = new TrcPseudoselfDescription();
                toad.referd = lane;
                toad.maxx = float.MinValue;
                toad.minx = float.MaxValue;
                foreach (var spl in lane.Spline)
                {
                    for (float t = 0.01f; t <= 1.0f; t += 0.01f)
                    {
                        var evaled = spl.EvaluateAtParam(t);
                        if (evaled.X > toad.maxx)
                            toad.maxx = (float)evaled.X;
                        if (evaled.X < toad.minx)
                            toad.minx = (float)evaled.X;
                    }
                }
                toad.maxxTrue = toad.maxx;
                pseudSelves.Add(toad);
            }
            pseudSelves = pseudSelves.OrderBy(x => x.minx).ToList();
            for (int i = 0; i != pseudSelves.Count - 1; ++i)
            {
                for (int j = i + 1; j < pseudSelves.Count; ++j)
                {
                    if (pseudSelves[i].maxx > pseudSelves[j].maxx)
                        pseudSelves[j].maxx = pseudSelves[i].maxx;
                }
            }
            for (int i = 0; i != pseudSelves.Count; ++i)
            {
                LaneData[i].MaxXOfPseudoSelf = pseudSelves[i].maxx + margin;
                LaneData[i].MinXOfPseudoSelf = pseudSelves[i].minx - margin;
                LaneData[i].MaxxTrueOfPseudoSelf = pseudSelves[i].maxxTrue + margin;
                LaneData[i].PseudoSelfIndexInLaneArray = LaneData.IndexOf(pseudSelves[i].referd);
            }
        }

        private List<TrcLane> closestTwoByStartPoint(List<TrcLane> possible, TrcLane toThis)
        {
            double minDist = double.MaxValue;
            TrcLane found1 = null;
            foreach (var l in possible)
            {
                if (l == toThis)
                    continue;
                double potDist = (toThis.StartPoint - l.StartPoint).Length;
                if (potDist < minDist)
                {
                    minDist = potDist;
                    found1 = l;
                }
            }
            TrcLane found2 = null;
            minDist = double.MaxValue;
            foreach (var l in possible)
            {
                if (l == toThis)
                    continue;
                if (l == found1)
                    continue;
                double potDist = (toThis.StartPoint - l.StartPoint).Length;
                if (potDist < minDist)
                {
                    minDist = potDist;
                    found2 = l;
                }
            }
            List<TrcLane> ret = new List<TrcLane>();
            ret.Add(found1);
            ret.Add(found2);
            return ret;
        }
    }
}