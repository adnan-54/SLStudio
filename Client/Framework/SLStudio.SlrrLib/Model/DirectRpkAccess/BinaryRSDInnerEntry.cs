using System;
using System.Text;

namespace SlrrLib.Model
{
    public class BinaryRSDInnerEntry : BinaryInnerRsdEntry
    {
        protected static readonly int offsetSignature = 0;
        protected static readonly int offsetLengthOfData = 4;
        protected static readonly int offsetData = 8;

        public string StringData
        {
            get
            {
                return ASCIIEncoding.ASCII.GetString(Cache.GetFileData(), offsetData + Offset, LengthOfData);
            }
            set
            {
                string toConvert = value;
                if (!toConvert.EndsWith("\r\n"))
                    toConvert += "\r\n";
                var bytes = ASCIIEncoding.ASCII.GetBytes(toConvert);
                LengthChangingReplace(bytes, 0, bytes.Length, offsetData - Offset, LengthOfData);
                Size = bytes.Length + 8;
            }
        }

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

        public override int Size
        {
            get
            {
                return GetIntFromFile(offsetLengthOfData, true) + 8;
            }
            set
            {
                throw new Exception("RSDInnerEntry size should always be LengthOfData+8");
            }
        }

        public BinaryRSDInnerEntry(FileCacheHolder file, int offset, bool cache = false)
        : base(0, file, offset, cache)
        {
        }

        public override string ToString()
        {
            return StringData;
        }
    }
}