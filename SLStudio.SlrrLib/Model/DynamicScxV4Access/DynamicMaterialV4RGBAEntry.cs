using System;
using System.IO;

namespace SlrrLib.Model
{
    public class DynamicMaterialV4RGBAEntry : DynamicMaterialV4Entry
    {
        public byte R
        {
            get;
            set;
        }

        public byte G
        {
            get;
            set;
        }

        public byte B
        {
            get;
            set;
        }

        public byte A
        {
            get;
            set;
        }

        public DynamicMaterialV4RGBAEntry(BinaryMaterialV4Entry constructFrom = null)
        : base(constructFrom)
        {
            base.type = 0;
            if (constructFrom == null)
                return;
            UnknownFlag = constructFrom.UnknownFlag;
            type = constructFrom.Type;
            R = constructFrom.DataRAsByte;
            G = constructFrom.DataGAsByte;
            B = constructFrom.DataBAsByte;
            A = constructFrom.DataAAsByte;

            if (constructFrom.Size != Size)
                throw new Exception("HeaderWill Mismatch");
        }

        public override void Save(BinaryWriter bw)
        {
            bw.Write(UnknownFlag);
            bw.Write(this.type);
            bw.Write(R);
            bw.Write(G);
            bw.Write(B);
            bw.Write(A);
        }
    }
}