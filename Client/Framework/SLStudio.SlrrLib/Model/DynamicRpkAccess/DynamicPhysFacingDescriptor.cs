using System.IO;

namespace SlrrLib.Model
{
    public class DynamicPhysFacingDescriptor : DynamicRSDInnerEntryBase
    {
        public int UnkownInt1
        {
            get;
            set;
        }

        public int TriIndex0
        {
            get;
            set;
        }

        public int TriIndex1
        {
            get;
            set;
        }

        public float NormalX
        {
            get;
            set;
        }

        public float NormalY
        {
            get;
            set;
        }

        public float NormalZ
        {
            get;
            set;
        }

        public int TriIndex2
        {
            get;
            set;
        }

        public DynamicPhysFacingDescriptor()
        : this(null)
        {
        }

        public DynamicPhysFacingDescriptor(BinaryPhysFacingDescriptor from = null)
        {
            if (from == null)
                return;
            UnkownInt1 = from.UnkownInt1;
            TriIndex0 = from.TriIndex0;
            TriIndex1 = from.TriIndex1;
            NormalX = from.NormalX;
            NormalY = from.NormalY;
            NormalZ = from.NormalZ;
            TriIndex2 = from.TriIndex2;
        }

        public override int GetSize()
        {
            return 28;
        }

        public override void Save(BinaryWriter bw)
        {
            bw.Write(UnkownInt1);
            bw.Write(TriIndex0);
            bw.Write(TriIndex1);
            bw.Write(TriIndex2);
            bw.Write(NormalX);
            bw.Write(NormalY);
            bw.Write(NormalZ);
        }
    }
}