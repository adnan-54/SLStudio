using System;

namespace SlrrLib.Model
{
    public class BinaryEXTPInnerEntry : BinaryInnerRsdEntry
    {
        protected static readonly int offsetSignature = 0;
        protected static readonly int offsetLengthOfData = 4;
        protected static readonly int offsetEXTPint = 8;

        public string Signature
        {
            get
            {
                return GetFixLengthString(offsetSignature, 4);
            }
            set
            {
                SetFixLengthString(value, 4, offsetSignature);
            }
        }

        public int LengthOfData
        {
            get
            {
                return GetIntFromFile(offsetLengthOfData);
            }
            set
            {
                SetIntInFile(value, offsetLengthOfData);
            }
        }

        public int EXTPint
        {
            get
            {
                return GetIntFromFile(offsetEXTPint);
            }
            set
            {
                SetIntInFile(value, offsetEXTPint);
            }
        }

        public override int Size
        {
            get
            {
                return GetIntFromFile(offsetLengthOfData, true) + 8;
            }
            set
            {
                throw new Exception("EXTPInnerEntry size should always be LengthOfData+8");
            }
        }

        public BinaryEXTPInnerEntry(FileCacheHolder file, int offset, bool cache = false)
        : base(0, file, offset, cache)
        {
        }

        public override string ToString()
        {
            return "EXTP: " + EXTPint.ToString();
        }
    }
}