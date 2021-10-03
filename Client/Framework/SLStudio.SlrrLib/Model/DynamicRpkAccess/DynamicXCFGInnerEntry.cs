using System.IO;
using System.Linq;
using System.Text;

namespace SlrrLib.Model
{
    public class DynamicXCFGInnerEntry : DynamicRSDInnerEntryBase
    {
        public static readonly string Signature = "XCFG";

        public string CfgRefrenceKey
        {
            get;
            set;
        }

        public string CfgReferenceValue
        {
            get;
            set;
        }

        public DynamicXCFGInnerEntry()
        : this(null)
        {
        }

        public DynamicXCFGInnerEntry(BinaryXCFGInnerEntry from = null)
        {
            if (from == null)
                return;
            CfgReferenceValue = from.CfgReferenceValue;
            CfgRefrenceKey = from.CfgRefrenceKey;
        }

        public override int GetSize()
        {
            if (CfgReferenceValue.Last() != '\0')
                return 4 + 4 + 1 + CfgRefrenceKey.Length + CfgReferenceValue.Length + 1;
            return 4 + 4 + 1 + CfgRefrenceKey.Length + CfgReferenceValue.Length;
        }

        public override void Save(BinaryWriter bw)
        {
            bw.Write(ASCIIEncoding.ASCII.GetBytes(Signature));
            bw.Write(GetSize() - 8);//dataSize
            bw.Write(ASCIIEncoding.ASCII.GetBytes(CfgRefrenceKey + " "));
            if (CfgReferenceValue.Last() != '\0')
                bw.Write(ASCIIEncoding.ASCII.GetBytes(CfgReferenceValue + "\0"));
            else
                bw.Write(ASCIIEncoding.ASCII.GetBytes(CfgReferenceValue));
        }
    }
}