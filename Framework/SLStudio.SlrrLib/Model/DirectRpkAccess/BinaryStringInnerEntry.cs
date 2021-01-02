namespace SlrrLib.Model
{
    public class BinaryStringInnerEntry : BinaryInnerRsdEntry
    {
        public string StringData
        {
            get
            {
                return base.innerRSDDataString;
            }
            set
            {
                string toConvert = value;
                if (!toConvert.EndsWith("\r\n"))
                    toConvert += "\r\n";
                base.innerRSDDataString = toConvert;
            }
        }

        public BinaryStringInnerEntry(int Size, FileCacheHolder file, int offset, bool cache = false)
        : base(Size, file, offset, cache)
        {
        }

        public override string ToString()
        {
            return StringData;
        }
    }
}