using System;
using System.IO;

namespace SlrrLib.Model
{
    public class DynamicMaterialV4MapDefinition : DynamicMaterialV4Entry
    {
        private int tillingFlags
        {
            get;
            set;
        }//3=UTileVTile 0=UMirrorVMirror

        public int TextureIndex
        {
            get;
            set;
        }

        public int MapChannel
        {
            get;
            set;
        }

        public bool UTileVTile
        {
            get
            {
                return tillingFlags == 3;
            }
            set
            {
                tillingFlags = 3;
            }
        }

        public bool UMirrorVMirror
        {
            get
            {
                return tillingFlags == 0;
            }
            set
            {
                tillingFlags = 0;
            }
        }

        public float TillingU
        {
            get;
            set;
        }

        public float TillingV
        {
            get;
            set;
        }

        public float OffsetU
        {
            get;
            set;
        }

        public float OffsetV
        {
            get;
            set;
        }

        public DynamicMaterialV4FloatEntry FloatWeight
        {
            get;
            set;
        } = null;

        public override int Size
        {
            get
            {
                if (FloatWeight != null)
                    return BinaryMaterialV4Entry.TypeToSize[type] + 4 + FloatWeight.Size;
                return BinaryMaterialV4Entry.TypeToSize[type] + 4;//4 header;
            }
        }

        public DynamicMaterialV4MapDefinition(BinaryMaterialV4MapDefinition constructFrom = null)
        : base(constructFrom)
        {
            base.type = 1536;
            if (constructFrom == null)
                return;
            UnknownFlag = constructFrom.UnknownFlag;
            type = constructFrom.Type;
            TextureIndex = constructFrom.TextureIndex;
            MapChannel = constructFrom.MapChannel;
            tillingFlags = constructFrom.TillingFlags;
            TillingU = constructFrom.TillingU;
            TillingV = constructFrom.TillingV;
            OffsetU = constructFrom.OffsetU;
            OffsetV = constructFrom.OffsetV;
            if (constructFrom.FloatWeight != null)
            {
                FloatWeight = new DynamicMaterialV4FloatEntry(constructFrom.FloatWeight);
                if (constructFrom.Size != Size - FloatWeight.Size)
                    throw new Exception("HeaderWill Mismatch");
            }
            else
            {
                if (constructFrom.Size != Size)
                    throw new Exception("HeaderWill Mismatch");
            }
        }

        public override void Save(BinaryWriter bw)
        {
            bw.Write(UnknownFlag);
            bw.Write(this.type);
            bw.Write(TextureIndex);
            bw.Write(MapChannel);
            bw.Write(tillingFlags);
            bw.Write(TillingU);
            bw.Write(TillingV);
            bw.Write(OffsetU);
            bw.Write(OffsetV);
            if (FloatWeight != null)
                FloatWeight.Save(bw);
        }
    }
}