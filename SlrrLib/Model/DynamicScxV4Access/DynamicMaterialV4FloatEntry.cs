using System;
using System.IO;

namespace SlrrLib.Model
{
    public class DynamicMaterialV4FloatEntry : DynamicMaterialV4Entry
    {
        public float Data
        {
            get;
            set;
        }

        public DynamicMaterialV4FloatEntry(BinaryMaterialV4Entry constructFrom = null)
        : base(constructFrom)
        {
            base.type = 256;
            if (constructFrom == null)
                return;
            UnknownFlag = constructFrom.UnknownFlag;
            type = constructFrom.Type;
            Data = constructFrom.DataAsFloat;

            if (constructFrom.Size != Size)
                throw new Exception("HeaderWill Mismatch");
        }

        public override void Save(BinaryWriter bw)
        {
            bw.Write(UnknownFlag);
            bw.Write(this.type);
            bw.Write(Data);
        }
    }
}