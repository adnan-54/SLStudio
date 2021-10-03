using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlrrLib.Model
{
    public class DynamicNamedSpatialData : DynamicRSDInnerEntryBase
    {
        public int SuperID
        {
            get;
            set;
        }

        public int TypeID
        {
            get;
            set;
        }

        public short UnkownShort1
        {
            get;
            set;
        }

        public float UnkownIsParentCompatible
        {
            get;
            set;
        }

        public int UnkownOffset
        {
            get;
            set;
        }

        public int UnkownSizeAtOffset
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public float BoundingBoxX
        {
            get;
            set;
        }

        public float BoundingBoxY
        {
            get;
            set;
        }

        public float BoundingBoxZ
        {
            get;
            set;
        }

        public float BoundingBoxHalfWidthX
        {
            get;
            set;
        }

        public float BoundingBoxHalfWidthY
        {
            get;
            set;
        }

        public float BoundingBoxHalfWidthZ
        {
            get;
            set;
        }

        public float UnkownData7
        {
            get;
            set;
        }

        public float UnkownData8
        {
            get;
            set;
        }

        public float UnkownData9
        {
            get;
            set;
        }

        public float UnkownData10
        {
            get;
            set;
        }

        public float UnkownData11
        {
            get;
            set;
        }

        public float UnkownData12
        {
            get;
            set;
        }

        public List<DynamicRSDInnerEntryBase> OwnedEntries
        {
            get;
            set;
        } = new List<DynamicRSDInnerEntryBase>();

        public DynamicNamedSpatialData()
        : this(null)
        {
        }

        public DynamicNamedSpatialData(BinaryNamedSpatialData from = null)
        {
            if (from == null)
                return;
            SuperID = from.UnkownTypeID;
            TypeID = from.UnkownSuperID;
            UnkownShort1 = from.UnkownShort1;
            UnkownIsParentCompatible = from.UnkownIsParentCompatible;
            UnkownOffset = from.UnkownOffset;
            UnkownSizeAtOffset = from.UnkownSizeAtOffset;
            Name = from.Name;
            BoundingBoxX = from.BoundingBoxX;
            BoundingBoxY = from.BoundingBoxY;
            BoundingBoxZ = from.BoundingBoxZ;
            BoundingBoxHalfWidthX = from.BoundingBoxHalfWidthX;
            BoundingBoxHalfWidthY = from.BoundingBoxHalfWidthY;
            BoundingBoxHalfWidthZ = from.BoundingBoxHalfWidthZ;
            UnkownData7 = from.UnkownData7;
            UnkownData8 = from.UnkownData8;
            UnkownData9 = from.UnkownData9;
            UnkownData10 = from.UnkownData10;
            UnkownData11 = from.UnkownData11;
            UnkownData12 = from.UnkownData12;

            foreach (var ownedEntry in from.OwnedEntries)
            {
                if (ownedEntry is BinarySpatialNode)
                    OwnedEntries.Add(new DynamicSpatialNode(ownedEntry as BinarySpatialNode));
                if (ownedEntry is BinaryInnerPhysEntry)
                    OwnedEntries.Add(new DynamicInnerPhysEntry(ownedEntry as BinaryInnerPhysEntry));
                if (ownedEntry is BinaryInnerPolyEntry)
                    OwnedEntries.Add(new DynamicInnerPolyEntry(ownedEntry as BinaryInnerPolyEntry));
                if (ownedEntry is BinaryRSDInnerEntry)
                    OwnedEntries.Add(new DynamicRSDInnerEntry(ownedEntry as BinaryRSDInnerEntry));
                if (ownedEntry is BinaryEXTPInnerEntry)
                    OwnedEntries.Add(new DynamicEXTPInnerEntry(ownedEntry as BinaryEXTPInnerEntry));
            }
        }

        public bool IsOtherPositionInBound(DynamicNamedSpatialData other)
        {
            float x = other.BoundingBoxX - BoundingBoxX;
            float y = other.BoundingBoxY - BoundingBoxY;
            float z = other.BoundingBoxZ - BoundingBoxZ;
            return x < BoundingBoxHalfWidthX && x > -BoundingBoxHalfWidthX &&
                   y < BoundingBoxHalfWidthY && y > -BoundingBoxHalfWidthY &&
                   z < BoundingBoxHalfWidthZ && z > -BoundingBoxHalfWidthZ;
        }

        public bool IsOtherContainedWithinBounds(DynamicNamedSpatialData other)
        {
            float x = other.BoundingBoxX - BoundingBoxX;
            float y = other.BoundingBoxY - BoundingBoxY;
            float z = other.BoundingBoxZ - BoundingBoxZ;
            return isInBound(x + other.BoundingBoxHalfWidthX, y + other.BoundingBoxHalfWidthY, z + other.BoundingBoxHalfWidthZ) &&
                   isInBound(x - other.BoundingBoxHalfWidthX, y + other.BoundingBoxHalfWidthY, z + other.BoundingBoxHalfWidthZ) &&
                   isInBound(x + other.BoundingBoxHalfWidthX, y - other.BoundingBoxHalfWidthY, z + other.BoundingBoxHalfWidthZ) &&
                   isInBound(x + other.BoundingBoxHalfWidthX, y + other.BoundingBoxHalfWidthY, z - other.BoundingBoxHalfWidthZ) &&
                   isInBound(x - other.BoundingBoxHalfWidthX, y - other.BoundingBoxHalfWidthY, z + other.BoundingBoxHalfWidthZ) &&
                   isInBound(x - other.BoundingBoxHalfWidthX, y + other.BoundingBoxHalfWidthY, z - other.BoundingBoxHalfWidthZ) &&
                   isInBound(x + other.BoundingBoxHalfWidthX, y - other.BoundingBoxHalfWidthY, z - other.BoundingBoxHalfWidthZ) &&
                   isInBound(x - other.BoundingBoxHalfWidthX, y - other.BoundingBoxHalfWidthY, z - other.BoundingBoxHalfWidthZ);
        }

        public bool IsOtherPositionContainedWithin(DynamicNamedSpatialData other)
        {
            float x = other.BoundingBoxX - BoundingBoxX;
            float y = other.BoundingBoxY - BoundingBoxY;
            float z = other.BoundingBoxZ - BoundingBoxZ;
            return x < BoundingBoxHalfWidthX && x > -BoundingBoxHalfWidthX &&
                   y < BoundingBoxHalfWidthY && y > -BoundingBoxHalfWidthY &&
                   z < BoundingBoxHalfWidthZ && z > -BoundingBoxHalfWidthZ;
        }

        public void IncludeOtherWithinBounds(DynamicNamedSpatialData other, float margin = 0.01f)
        {
            System.Windows.Media.Media3D.Rect3D mine = new System.Windows.Media.Media3D.Rect3D
            {
                SizeX = BoundingBoxHalfWidthX * 2.0,
                SizeY = BoundingBoxHalfWidthY * 2.0,
                SizeZ = BoundingBoxHalfWidthZ * 2.0,
                X = BoundingBoxX - BoundingBoxHalfWidthX,
                Y = BoundingBoxY - BoundingBoxHalfWidthY,
                Z = BoundingBoxZ - BoundingBoxHalfWidthZ
            };
            System.Windows.Media.Media3D.Rect3D others = new System.Windows.Media.Media3D.Rect3D
            {
                SizeX = other.BoundingBoxHalfWidthX * 2.0,
                SizeY = other.BoundingBoxHalfWidthY * 2.0,
                SizeZ = other.BoundingBoxHalfWidthZ * 2.0,
                X = other.BoundingBoxX - other.BoundingBoxHalfWidthX,
                Y = other.BoundingBoxY - other.BoundingBoxHalfWidthY,
                Z = other.BoundingBoxZ - other.BoundingBoxHalfWidthZ
            };
            mine.Union(others);
            BoundingBoxHalfWidthX = ((float)mine.SizeX / 2.0f) + margin;
            BoundingBoxHalfWidthY = ((float)mine.SizeY / 2.0f) + margin;
            BoundingBoxHalfWidthZ = ((float)mine.SizeZ / 2.0f) + margin;
            BoundingBoxX = (float)mine.X + BoundingBoxHalfWidthX + margin / 2.0f;
            BoundingBoxY = (float)mine.Y + BoundingBoxHalfWidthY + margin / 2.0f;
            BoundingBoxZ = (float)mine.Z + BoundingBoxHalfWidthZ + margin / 2.0f;
        }

        public float VolumeOfBound()
        {
            return 2.0f * BoundingBoxHalfWidthX * BoundingBoxHalfWidthY * BoundingBoxHalfWidthZ;
        }

        public void RemoveDirectChild(DynamicNamedSpatialData child)
        {
            OwnedEntries.Remove(child);
            foreach (var entr in OwnedEntries)
            {
                if (entr is DynamicSpatialNode datArr)
                    datArr.DataArray.Remove(child);
            }
        }

        public void FixName()
        {
            if (Name.Length > byte.MaxValue)
            {
                Name = Name.Substring(0, byte.MaxValue - 1);
            }
            Name += (Name == "" || Name.Last() == '\0' ? "" : "\0");
        }

        public void VisitAllDecendantNamedDatas(Action<DynamicNamedSpatialData, DynamicNamedSpatialData> visiter)
        {
            foreach (var ownedDat in OwnedEntries.ToList())
            {
                if (ownedDat is DynamicNamedSpatialData)
                {
                    visiter(this, ownedDat as DynamicNamedSpatialData);
                    (ownedDat as DynamicNamedSpatialData).VisitAllDecendantNamedDatas(visiter);
                }
                if (ownedDat is DynamicSpatialNode)
                {
                    (ownedDat as DynamicSpatialNode).VisitAllDecendantNamedDatas(visiter, this);
                }
            }
        }

        public void VisitAllDecendantNamedDatas(Action<DynamicNamedSpatialData, DynamicNamedSpatialData, int> visiter, int startDepth)
        {
            foreach (var ownedDat in OwnedEntries.ToList())
            {
                if (ownedDat is DynamicNamedSpatialData)
                {
                    visiter(this, ownedDat as DynamicNamedSpatialData, startDepth);
                    (ownedDat as DynamicNamedSpatialData).VisitAllDecendantNamedDatas(visiter, startDepth + 1);
                }
                if (ownedDat is DynamicSpatialNode)
                {
                    (ownedDat as DynamicSpatialNode).VisitAllDecendantNamedDatas(visiter, this, startDepth + 1);
                }
            }
        }

        public void VisitAllDecendantNamedDatas(Func<DynamicNamedSpatialData, DynamicNamedSpatialData, bool> visiter)
        {
            foreach (var ownedDat in OwnedEntries.ToList())
            {
                if (ownedDat is DynamicNamedSpatialData)
                {
                    if (!visiter(this, ownedDat as DynamicNamedSpatialData))
                    {
                        break;
                    }
                  (ownedDat as DynamicNamedSpatialData).VisitAllDecendantNamedDatas(visiter);
                }
                if (ownedDat is DynamicSpatialNode)
                {
                    (ownedDat as DynamicSpatialNode).VisitAllDecendantNamedDatas(visiter, this);
                }
            }
        }

        public DynamicSpatialNode GetFirstNamedDataArray()
        {
            return OwnedEntries.FirstOrDefault(x => x is DynamicSpatialNode) as DynamicSpatialNode;
        }

        public IEnumerable<DynamicRSDInnerEntry> GetRSDEntries()
        {
            return OwnedEntries.Where(x => x is DynamicRSDInnerEntry).Select(x => x as DynamicRSDInnerEntry);
        }

        public DynamicRSDInnerEntry GetFirstRSDEntry()
        {
            return GetRSDEntries().FirstOrDefault();
        }

        public int GetIntHexValueFromLineNameInFirstRSD(string lineName)
        {
            var strValue = GetStringValueFromLineNameInFirstRSD(lineName);
            if (strValue != null)
            {
                try
                {
                    return int.Parse(strValue.Substring(2), System.Globalization.NumberStyles.HexNumber);
                }
                catch (Exception) { }//doesn't really matter drop to return -1
            }
            return -1;
        }

        public void SetIntHexValueFromLineNameInFirstRSD(string lineName, int value)
        {
            SetStringValueFromLineNameInFirstRSD(lineName, "0x" + value.ToString("X8"));
        }

        public System.Windows.Media.Media3D.Vector3D GetPositionFromParamsLineFromFirstRSD()
        {
            var prms = GetStringValueFromLineNameInFirstRSD("params");
            if (prms == null)
                return new System.Windows.Media.Media3D.Vector3D(BoundingBoxX, BoundingBoxY, BoundingBoxZ);
            var spl = prms.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            if (spl.Length < 3)
                return new System.Windows.Media.Media3D.Vector3D(BoundingBoxX, BoundingBoxY, BoundingBoxZ);
            var ret = new System.Windows.Media.Media3D.Vector3D();
            try
            {
                ret.X = double.Parse(spl[0], System.Globalization.CultureInfo.InvariantCulture);
                ret.Y = double.Parse(spl[1], System.Globalization.CultureInfo.InvariantCulture);
                ret.Z = double.Parse(spl[2], System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                return new System.Windows.Media.Media3D.Vector3D(BoundingBoxX, BoundingBoxY, BoundingBoxZ);
            }
            return ret;
        }

        public System.Windows.Media.Media3D.Vector3D GetYawPitchRollFromParamsLineFromFirstRSD()
        {
            var prms = GetStringValueFromLineNameInFirstRSD("params");
            if (prms == null)
                return new System.Windows.Media.Media3D.Vector3D();
            var spl = prms.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            if (spl.Length < 6)
                return new System.Windows.Media.Media3D.Vector3D();
            var ret = new System.Windows.Media.Media3D.Vector3D();
            try
            {
                ret.X = double.Parse(spl[3], System.Globalization.CultureInfo.InvariantCulture);
                ret.Y = double.Parse(spl[4], System.Globalization.CultureInfo.InvariantCulture);
                ret.Z = double.Parse(spl[5], System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception) { }
            return ret;
        }

        public void SetPositionFromParamsLineFromFirstRSD(System.Windows.Media.Media3D.Vector3D pos)
        {
            var prms = GetStringValueFromLineNameInFirstRSD("params");
            if (prms == null)
                return;
            var spl = prms.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            spl[0] = pos.X.ToString("F3");
            spl[1] = pos.Y.ToString("F3");
            spl[2] = pos.Z.ToString("F3");
            SetStringValueFromLineNameInFirstRSD("params", spl.Aggregate((x, y) => x + ',' + y));
        }

        public void SetYawPitchRollFromParamsLineFromFirstRSD(System.Windows.Media.Media3D.Vector3D YawPicthRoll)
        {
            var prms = GetStringValueFromLineNameInFirstRSD("params");
            if (prms == null)
                return;
            var spl = prms.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();
            spl[3] = YawPicthRoll.X.ToString("F3");
            spl[4] = YawPicthRoll.Y.ToString("F3");
            spl[5] = YawPicthRoll.Z.ToString("F3");
            SetStringValueFromLineNameInFirstRSD("params", spl.Aggregate((x, y) => x + ',' + y));
        }

        public string GetStringValueFromLineNameInFirstRSD(string lineName)
        {
            var rsd = GetFirstRSDEntry();
            if (rsd == null)
                return null;
            var lookUp = rsd.GetStringDataAsDict();
            if (lookUp.Any(x => x.Key.ToLower() == lineName))
            {
                var gameTypeLine = lookUp.First(x => x.Key.ToLower() == lineName);
                return gameTypeLine.Value;
            }
            return null;
        }

        public void SetStringValueFromLineNameInFirstRSD(string lineName, string lineValue)
        {
            var rsd = GetFirstRSDEntry();
            if (rsd == null)
                return;
            var lookUp = rsd.GetStringDataAsDict();
            if (lookUp.Any(x => x.Key.ToLower() == lineName))
            {
                var gameTypeLine = lookUp.First(x => x.Key.ToLower() == lineName);
                gameTypeLine.Value = lineValue;
            }
            rsd.SetStringDataAsDict(lookUp);
        }

        public override int GetSize()
        {
            FixName();
            var byts = ASCIIEncoding.ASCII.GetBytes(Name);
            return 71 + (byts.Length);
        }

        public override void Save(BinaryWriter bw)
        {
            bw.Write(SuperID);
            bw.Write(TypeID);
            bw.Write(UnkownShort1);
            bw.Write(UnkownIsParentCompatible);
            bw.Write(unkownOffset());
            bw.Write(OwnedEntries.Sum(x => x.GetSize()));
            var byts = ASCIIEncoding.ASCII.GetBytes(Name);
            bw.Write((byte)byts.Length);
            bw.Write(byts);
            bw.Write(BoundingBoxX);
            bw.Write(BoundingBoxY);
            bw.Write(BoundingBoxZ);
            bw.Write(BoundingBoxHalfWidthX);
            bw.Write(BoundingBoxHalfWidthY);
            bw.Write(BoundingBoxHalfWidthZ);
            bw.Write(UnkownData7);
            bw.Write(UnkownData8);
            bw.Write(UnkownData9);
            bw.Write(UnkownData10);
            bw.Write(UnkownData11);
            bw.Write(UnkownData12);
        }

        private int unkownOffset()
        {
            if (OwnedEntries.FirstOrDefault(x => x is DynamicSpatialNode) is DynamicSpatialNode ownEntry)
                return ownEntry.PrecalculatedOffset;
            return -1;
        }

        private bool isInBound(float x, float y, float z)
        {
            return x < BoundingBoxHalfWidthX && x > -BoundingBoxHalfWidthX &&
                   y < BoundingBoxHalfWidthY && y > -BoundingBoxHalfWidthY &&
                   z < BoundingBoxHalfWidthZ && z > -BoundingBoxHalfWidthZ;
        }
    }
}