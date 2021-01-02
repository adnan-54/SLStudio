using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SlrrLib.Model
{
    public class BinaryICFGInnerEntry : BinaryInnerRsdEntry
    {
        protected static readonly int offsetSignature = 0;
        protected static readonly int offsetLengthOfData = 4;
        protected static readonly int offsetData = 8;

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

        public IEnumerable<string> DataList
        {
            get
            {
                return rsdString.Split('\0');
            }
            set
            {
                string stringData = "";
                foreach (var str in value)
                {
                    stringData = str + "\0";
                }
                stringData = stringData.Trim('\0');
                rsdString = stringData;
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

        private string rsdString
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

        public BinaryICFGInnerEntry(FileCacheHolder file, int offset, bool cache = false)
        : base(0, file, offset, cache)
        {
        }

        public override string ToString()
        {
            return DataList.Aggregate((x, y) => x + " " + y);
        }
    }
}