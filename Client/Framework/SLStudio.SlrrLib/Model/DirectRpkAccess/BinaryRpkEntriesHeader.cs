using System;

namespace SlrrLib.Model
{
    public class BinaryRpkEntriesHeader : FileEntry
    {
        protected static readonly int offsetEntriesSize = 0;
        protected static readonly int offsetEntriesCount = 4;
        protected static readonly int offsetEntriesType1Count = 8;
        protected static readonly int offsetEntriesNonType1Count = 12;

        public int EntriesSize
        {
            get
            {
                return GetIntFromFile(offsetEntriesSize);
            }
            set
            {
                SetIntInFile(value, offsetEntriesSize);
            }
        }

        public int EntriesCount
        {
            get
            {
                return GetIntFromFile(offsetEntriesCount);
            }
            set
            {
                SetIntInFile(value, offsetEntriesCount);
            }
        }

        public int EntriesType1Count
        {
            get
            {
                return GetIntFromFile(offsetEntriesType1Count);
            }
            set
            {
                SetIntInFile(value, offsetEntriesType1Count);
            }
        }

        public int EntriesNonType1Count
        {
            get
            {
                return GetIntFromFile(offsetEntriesNonType1Count);
            }
            set
            {
                SetIntInFile(value, offsetEntriesNonType1Count);
            }
        }

        public override int Size
        {
            get
            {
                return 16;
            }
            set
            {
                if (value != 16)
                    throw new Exception("RpkEntriesHeader must be 16 long");
            }
        }

        public BinaryRpkEntriesHeader(FileCacheHolder file, int offset, bool cache = false)
        : base(file, offset, cache)
        {
        }
    }
}