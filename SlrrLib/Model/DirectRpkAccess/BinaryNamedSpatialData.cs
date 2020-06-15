using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlrrLib.Model
{
    public class BinaryNamedSpatialData : BinaryInnerRsdEntry
    {
        protected static readonly int unkownInt1Offset = 0;
        protected static readonly int unkownInt2Offset = 4;
        protected static readonly int unkownShort1Offset = 8;
        protected static readonly int unkownInt3Offset = 10;
        protected static readonly int unkownInt4Offset = 14;
        protected static readonly int unkownInt5Offset = 18;
        protected static readonly int nameLengthByteOffset = 22;
        protected static readonly int nameOffset = 23;
        protected static readonly int unkownFloat1Offset = 0;
        protected static readonly int unkownFloat2Offset = 4;
        protected static readonly int unkownFloat3Offset = 8;
        protected static readonly int unkownFloat4Offset = 12;
        protected static readonly int unkownFloat5Offset = 16;
        protected static readonly int unkownFloat6Offset = 20;
        protected static readonly int unkownFloat7Offset = 24;
        protected static readonly int unkownFloat8Offset = 28;
        protected static readonly int unkownFloat9Offset = 32;
        protected static readonly int unkownFloat10Offset = 36;
        protected static readonly int unkownFloat11Offset = 40;
        protected static readonly int unkownFloat12Offset = 44;

        public int UnkownTypeID//can be a typeID
        {
            get
            {
                return GetIntFromFile(unkownInt1Offset);
            }
            set
            {
                SetIntInFile(value, unkownInt1Offset);
            }
        }

        public int UnkownSuperID//can be a superID
        {
            get
            {
                return GetIntFromFile(unkownInt2Offset);
            }
            set
            {
                SetIntInFile(value, unkownInt2Offset);
            }
        }

        public short UnkownShort1//can be type and additional type
        {
            get
            {
                return GetShortFromFile(unkownShort1Offset);
            }
            set
            {
                SetShortInFile(value, unkownShort1Offset);
            }
        }

        public float UnkownIsParentCompatible
        {
            get
            {
                return GetFloatFromFile(unkownInt3Offset);
            }
            set
            {
                SetFloatInFile(value, unkownInt3Offset);
            }
        }

        public int UnkownOffset//can be an offset
        {
            get
            {
                return GetIntFromFile(unkownInt4Offset);
            }
            set
            {
                SetIntInFile(value, unkownInt4Offset);
            }
        }

        public int UnkownSizeAtOffset//can be a length
        {
            get
            {
                return GetIntFromFile(unkownInt5Offset);
            }
            set
            {
                SetIntInFile(value, unkownInt5Offset);
            }
        }

        public byte NameLengthByte
        {
            get
            {
                return GetByteFromFile(nameLengthByteOffset, true);
            }
            set
            {
                SetByteInFile(value, nameLengthByteOffset);
            }
        }

        public string Name
        {
            get
            {
                return GetFixLengthString(nameOffset, NameLengthByte);
            }
            set
            {
                var byts = ASCIIEncoding.ASCII.GetBytes(value);
                if (byts.Length > byte.MaxValue)
                    byts = byts.Take(byte.MaxValue).ToArray();
                LengthChangingReplace(byts, 0, byts.Length, nameOffset, NameLengthByte);
                NameLengthByte = (byte)byts.Length;
            }
        }

        public float BoundingBoxX//in range -50000 .. 50000
        {
            get
            {
                return GetFloatFromFile(unkownFloatsOffset + unkownFloat1Offset);
            }
            set
            {
                SetFloatInFile(value, unkownFloatsOffset + unkownFloat1Offset);
            }
        }

        public float BoundingBoxY//in range -50000 .. 50000
        {
            get
            {
                return GetFloatFromFile(unkownFloatsOffset + unkownFloat2Offset);
            }
            set
            {
                SetFloatInFile(value, unkownFloatsOffset + unkownFloat2Offset);
            }
        }

        public float BoundingBoxZ//in range -50000 .. 50000
        {
            get
            {
                return GetFloatFromFile(unkownFloatsOffset + unkownFloat3Offset);
            }
            set
            {
                SetFloatInFile(value, unkownFloatsOffset + unkownFloat3Offset);
            }
        }

        public float BoundingBoxHalfWidthX//in range 0 .. 50000 generally small values
        {
            get
            {
                return GetFloatFromFile(unkownFloatsOffset + unkownFloat4Offset);
            }
            set
            {
                SetFloatInFile(value, unkownFloatsOffset + unkownFloat4Offset);
            }
        }

        public float BoundingBoxHalfWidthY//in range 0 .. 50000 generally small values
        {
            get
            {
                return GetFloatFromFile(unkownFloatsOffset + unkownFloat5Offset);
            }
            set
            {
                SetFloatInFile(value, unkownFloatsOffset + unkownFloat5Offset);
            }
        }

        public float BoundingBoxHalfWidthZ//in range 0 .. 50000 generally small values
        {
            get
            {
                return GetFloatFromFile(unkownFloatsOffset + unkownFloat6Offset);
            }
            set
            {
                SetFloatInFile(value, unkownFloatsOffset + unkownFloat6Offset);
            }
        }

        public float UnkownData7// only 0 seen
        {
            get
            {
                return GetFloatFromFile(unkownFloatsOffset + unkownFloat7Offset);
            }
            set
            {
                SetFloatInFile(value, unkownFloatsOffset + unkownFloat7Offset);
            }
        }

        public float UnkownData8// only 0 seen
        {
            get
            {
                return GetFloatFromFile(unkownFloatsOffset + unkownFloat8Offset);
            }
            set
            {
                SetFloatInFile(value, unkownFloatsOffset + unkownFloat8Offset);
            }
        }

        public float UnkownData9// only 0 seen
        {
            get
            {
                return GetFloatFromFile(unkownFloatsOffset + unkownFloat9Offset);
            }
            set
            {
                SetFloatInFile(value, unkownFloatsOffset + unkownFloat9Offset);
            }
        }

        public float UnkownData10// only 0 seen
        {
            get
            {
                return GetFloatFromFile(unkownFloatsOffset + unkownFloat10Offset);
            }
            set
            {
                SetFloatInFile(value, unkownFloatsOffset + unkownFloat10Offset);
            }
        }

        public float UnkownData11// only 0 seen
        {
            get
            {
                return GetFloatFromFile(unkownFloatsOffset + unkownFloat11Offset);
            }
            set
            {
                SetFloatInFile(value, unkownFloatsOffset + unkownFloat11Offset);
            }
        }

        public float UnkownData12// only 0 seen
        {
            get
            {
                return GetFloatFromFile(unkownFloatsOffset + unkownFloat12Offset);
            }
            set
            {
                SetFloatInFile(value, unkownFloatsOffset + unkownFloat12Offset);
            }
        }

        public IEnumerable<FileEntry> OwnedEntries
        {
            get;
            set;
        } = null;

        public override int Size
        {
            get
            {
                return 71 + NameLengthByte;
            }
            set
            {
                throw new Exception("Only the name length can change.");
            }
        }

        private int unkownFloatsOffset
        {
            get
            {
                return GetByteFromFile(nameLengthByteOffset, true) + nameLengthByteOffset + 1;
            }
        }

        public BinaryNamedSpatialData(FileCacheHolder file, int offset, bool cache = false)
        : base(0, file, offset, cache)
        {
        }

        public bool IsPointInsideBound(float x, float y, float z)
        {
            x -= BoundingBoxX;
            y -= BoundingBoxY;
            z -= BoundingBoxZ;
            return x < BoundingBoxHalfWidthX && x > -BoundingBoxHalfWidthX &&
                   y < BoundingBoxHalfWidthY && y > -BoundingBoxHalfWidthY &&
                   z < BoundingBoxHalfWidthZ && z > -BoundingBoxHalfWidthZ;
        }
    }
}