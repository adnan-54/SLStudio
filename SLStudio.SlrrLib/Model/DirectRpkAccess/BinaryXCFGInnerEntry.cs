using System.Linq;

namespace SlrrLib.Model
{
    public class BinaryXCFGInnerEntry : BinaryICFGInnerEntry
    {
        public string CfgRefrenceKey
        {
            get
            {
                if (DataList.Any())
                    return new string(DataList.First().TakeWhile(x => x != ' ').ToArray()).TrimEnd(' ');
                return "";
            }
            set
            {
                DataList = new string[] { value + " " + CfgReferenceValue };
            }
        }

        public string CfgReferenceValue
        {
            get
            {
                if (DataList.Any())
                    return new string(DataList.First().SkipWhile(x => x != ' ').Skip(1).ToArray());
                return "";
            }
            set
            {
                if (value.Last() == '\0')
                    DataList = new string[] { CfgRefrenceKey + " " + value };
                else
                    DataList = new string[] { CfgRefrenceKey + " " + value + "\0" };
            }
        }

        public BinaryXCFGInnerEntry(FileCacheHolder file, int offset, bool cache = false)
        : base(file, offset, cache)
        {
        }

        public override string ToString()
        {
            return CfgRefrenceKey + " : " + CfgReferenceValue;
        }
    }
}