using System;
using System.Linq;

namespace SlrrLib.Model
{
    public class BinaryFaceDefV4 : FileEntry
    {
        private static readonly int typeDeferOffset = 0;
        private static readonly int sizeOffset = 4;
        private static readonly int numberOfShortsOffset = 8;

        private ushort[] shortsRetChache = null;

        public ushort[] GetShorts
        {
            get
            {
                if (shortsRetChache != null)
                    return shortsRetChache;
                var bytes = Cache.GetFileData().Skip(Offset + 12).Take(NumberOfShorts * 2).ToArray();
                shortsRetChache = new ushort[bytes.Length / 2];
                for (int byte_i = 0; byte_i != bytes.Length; byte_i += 2)
                {
                    shortsRetChache[byte_i / 2] = BitConverter.ToUInt16(bytes, byte_i);
                }
                return shortsRetChache;
            }
        }

        public int NumberOfShorts
        {
            get
            {
                return GetIntFromFile(numberOfShortsOffset);
            }
            set
            {
                int realSet = value + (value % 3);
                SetIntInFile(realSet, numberOfShortsOffset);
                SetIntInFile((NumberOfShorts * 2) + 12, sizeOffset);
            }
        }

        public int TypeDefer
        {
            get
            {
                return GetIntFromFile(typeDeferOffset);
            }
            set
            {
                SetIntInFile(value, typeDeferOffset);
            }
        }

        public override int Size
        {
            get
            {
                return GetIntFromFile(sizeOffset, true);
            }
            set
            {
                int inShorts = (value - 12) / 2;
                inShorts += inShorts % 3;
                int realSet = (inShorts * 2) + 12;
                SetIntInFile(realSet, sizeOffset);
                SetIntInFile((Size - 12) / 2, numberOfShortsOffset);
            }
        }

        public BinaryFaceDefV4(int offset, FileCacheHolder fileCache, bool cache = false)
        : base(fileCache, offset, cache)
        {
        }
    }
}