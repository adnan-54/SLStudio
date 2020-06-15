using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlrrLib.Model
{
    public class DynamicICFGInnerEntry : DynamicRSDInnerEntryBase
    {
        public readonly static string Signature = "ICFG";

        public List<string> DataList
        {
            get;
            set;
        } = new List<string>();// \0 delimiter

        public DynamicICFGInnerEntry()
        : this(null)
        {
        }

        public DynamicICFGInnerEntry(BinaryICFGInnerEntry from = null)
        {
            if (from == null)
                return;
            foreach (var s in from.DataList)
                DataList.Add(s);
        }

        public override int GetSize()
        {
            return 4 + 4 + (DataList.Count - 1) + DataList.Sum(x => x.Length);
        }

        public override void Save(BinaryWriter bw)
        {
            bw.Write(ASCIIEncoding.ASCII.GetBytes(Signature));
            bw.Write(GetSize() - 8);//dataSize
            for (int i = 0; i != DataList.Count; i++)
            {
                bw.Write(ASCIIEncoding.ASCII.GetBytes(DataList[i]));
                if (i != DataList.Count - 1)
                    bw.Write((byte)0);
            }
        }
    }
}