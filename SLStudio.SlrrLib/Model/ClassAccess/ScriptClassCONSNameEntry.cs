using System;
using System.IO;
using System.Linq;
using System.Text;

namespace SlrrLib.Model
{
    public class ScriptClassCONSNameEntry : ScriptClassCONSConstantEntry
    {
        protected static readonly int stringLengthOffset = 4;
        protected static readonly int stringOffset = 8;

        public int StringLength
        {
            get
            {
                return GetIntFromFile(stringLengthOffset, true);
            }
            set
            {
                SetIntInFile(value, stringLengthOffset, true);
            }
        }

        public string DataAsString
        {
            get
            {
                return GetFixLengthString(stringOffset, StringLength, true);
            }
        }

        public ScriptClassCONSNameEntry(FileCacheHolder file, int offset, bool cache = false)
        : base(file, offset, null, cache)
        {
        }

        public ScriptClassCONSNameEntry(string Contents, ScriptClassCONSChunk parent = null)
        : base(getFileContentsFromString(Contents), 0, parent)
        {
            StringLength = ASCIIEncoding.ASCII.GetByteCount(Contents);
        }

        public void ReplaceString(string toad)
        {
            Cache = getFileContentsFromString(toad);
            Offset = 0;
            StringLength = ASCIIEncoding.ASCII.GetByteCount(toad);
        }

        public override int GetSizeInFile()
        {
            return 4 + 4 + 1 + StringLength;
        }

        public override string ToString()
        {
            return DataAsString;
        }

        public override void Save(BinaryWriter bw)
        {
            bw.Write(ID);
            bw.Write(ASCIIEncoding.ASCII.GetByteCount(DataAsString));
            bw.Write(ASCIIEncoding.ASCII.GetBytes(DataAsString));
            bw.Write((byte)0);
        }

        private static FileCacheHolder getFileContentsFromString(string data)
        {
            int strLength = ASCIIEncoding.ASCII.GetByteCount(data);
            var tmpLst = BitConverter.GetBytes(0).ToList();//ID
            tmpLst.AddRange(BitConverter.GetBytes(strLength));
            tmpLst.AddRange(ASCIIEncoding.ASCII.GetBytes(data));
            tmpLst.Add((byte)0);
            return new FileCacheHolder(tmpLst.ToArray());
        }
    }
}