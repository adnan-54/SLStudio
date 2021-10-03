using System;
using System.Collections.Generic;
using System.Text;

namespace SlrrLib.Model
{
    public class BinaryMaterialV4Entry : FileEntry
    {
        public static readonly Dictionary<ushort, ushort> TypeToSize = new Dictionary<ushort, ushort>
    {
      {2048,32},//char[32]
      {0,4},//byte RGBA
      {256,4},//float intensity
      {1536,28},//map definition
      {1280,4}//int
    };

        private static readonly int unkownFlagOffset = 0;
        private static readonly int typeOffset = 2;
        private static readonly int dataOffset = 4;

        public static byte[] GetDefaultEntry(ushort unkownID, ushort typeBackwards, int size)
        {
            ushort type = (ushort)((typeBackwards >> 8) | (typeBackwards << 8));
            var ret = new byte[size];
            Array.Copy(BitConverter.GetBytes(unkownID), 0, ret, 0, 2);
            Array.Copy(BitConverter.GetBytes(type), 0, ret, 2, 2);
            return ret;
        }

        public static BinaryMaterialV4Entry ConstructMaterialEntry(FileCacheHolder fileCache, int offset, bool cache = false)
        {
            BinaryMaterialV4Entry ret = new BinaryMaterialV4Entry(fileCache, offset, cache);
            if (ret.Type == 1536)
                return new BinaryMaterialV4MapDefinition(fileCache, offset, cache);
            return ret;
        }

        public ushort UnknownFlag
        {
            get
            {
                return (ushort)GetShortFromFile(unkownFlagOffset);
            }
            set
            {
                SetShortInFile((short)value, unkownFlagOffset);
            }
        }

        public ushort Type
        {
            get
            {
                return (ushort)GetShortFromFile(typeOffset, true);
            }
            set
            {
                SetShortInFile((short)value, typeOffset);
            }
        }

        public ushort TypeSwapped
        {
            get
            {
                return (ushort)((Type >> 8) | (Type << 8));
            }
            set
            {
                Type = (ushort)((value >> 8) | (value << 8));
            }
        }

        public int MaterialRelativeOffset
        {
            get
            {
                return Offset;
            }
        }

        public override int Size
        {
            get
            {
                return TypeToSize[Type] + 4;//4 header;
            }
            set
            {
            }
        }

        public string PrettyName
        {
            get
            {
                if (UnknownFlag == 0 && TypeSwapped == 0)
                    return "Diffuse Color";
                if (UnknownFlag == 2 && TypeSwapped == 0)
                    return "Illumination Color";
                if (UnknownFlag == 1 && TypeSwapped == 0)
                    return "Specular Color";
                if (UnknownFlag == 0 && TypeSwapped == 1)
                    return "Glossiness";
                if (UnknownFlag == 1 && TypeSwapped == 1)
                    return "Reflection Percent";
                if (UnknownFlag == 2 && TypeSwapped == 1)
                    return "Bump Percent";
                if (UnknownFlag == 0 && TypeSwapped == 6)
                    return "Diffuse Map";
                if (UnknownFlag == 1 && TypeSwapped == 6)
                    return "Diffuse Mix Second";
                if (UnknownFlag == 2 && TypeSwapped == 6)
                    return "Bump Map";
                if (UnknownFlag == 3 && TypeSwapped == 6)
                    return "Reflection Map";
                if (UnknownFlag == 4 && TypeSwapped == 6)
                    return "Illumination Map";
                if (UnknownFlag == 0 && TypeSwapped == 8)
                    return "Material Name";
                return "Unkown";
            }
        }

        public float DataAsFloat
        {
            get
            {
                return GetFloatFromFile(dataOffset, true);
            }
            set
            {
                SetFloatInFile(value, dataOffset);
            }
        }

        public int DataAsInt
        {
            get
            {
                return GetIntFromFile(dataOffset, true);
            }
            set
            {
                SetIntInFile(value, dataOffset);
            }
        }

        public byte DataRAsByte
        {
            get
            {
                return GetByteFromFile(dataOffset, true);
            }
            set
            {
                SetByteInFile(value, dataOffset);
            }
        }

        public byte DataGAsByte
        {
            get
            {
                return GetByteFromFile(dataOffset + 1, true);
            }
            set
            {
                SetByteInFile(value, dataOffset + 1);
            }
        }

        public byte DataBAsByte
        {
            get
            {
                return GetByteFromFile(dataOffset + 2, true);
            }
            set
            {
                SetByteInFile(value, dataOffset + 2);
            }
        }

        public byte DataAAsByte
        {
            get
            {
                return GetByteFromFile(dataOffset + 3, true);
            }
            set
            {
                SetByteInFile(value, dataOffset + 3);
            }
        }

        public string DataAsString
        {
            get
            {
                if (UnknownFlag == 0 && TypeSwapped == 8)
                {
                    return ASCIIEncoding.ASCII.GetString(Cache.GetFileData(), Offset + 4, 32);
                }
                return "";
            }
        }

        public BinaryMaterialV4Entry(FileCacheHolder fileCache, int offset, bool cache = false)
        : base(fileCache, offset, cache)
        {
        }

        public override string ToString()
        {
            return Offset.ToString() + ":" + PrettyName;
        }
    }
}