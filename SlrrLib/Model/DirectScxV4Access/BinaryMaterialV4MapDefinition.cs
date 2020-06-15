namespace SlrrLib.Model
{
    public class BinaryMaterialV4MapDefinition : BinaryMaterialV4Entry
    {
        private static readonly int texIndexOffset = 4;
        private static readonly int mapChannelOffset = 8;
        private static readonly int unkownIntOffset = 12;
        private static readonly int unkownFloatWeightOffset = 16;
        private static readonly int unkownFloatWeight2Offset = 20;
        private static readonly int unkownInt2Offset = 24;
        private static readonly int unkownInt3Offset = 28;

        public int TextureIndex
        {
            get
            {
                return GetIntFromFile(texIndexOffset, true);
            }
            set
            {
                SetIntInFile(value, texIndexOffset);
            }
        }

        public int MapChannel
        {
            get
            {
                return GetIntFromFile(mapChannelOffset, true);
            }
            set
            {
                SetIntInFile(value, mapChannelOffset);
            }
        }

        public int TillingFlags//3=UTileVTile 0=UMirrorVMirror
        {
            get
            {
                return GetIntFromFile(unkownIntOffset, true);
            }
            set
            {
                SetIntInFile(value, unkownIntOffset);
            }
        }

        public float TillingU
        {
            get
            {
                return GetFloatFromFile(unkownFloatWeightOffset, true);
            }
            set
            {
                SetFloatInFile(value, unkownFloatWeightOffset);
            }
        }

        public float TillingV
        {
            get
            {
                return GetFloatFromFile(unkownFloatWeight2Offset, true);
            }
            set
            {
                SetFloatInFile(value, unkownFloatWeight2Offset);
            }
        }

        public float OffsetU
        {
            get
            {
                return GetFloatFromFile(unkownInt2Offset, true);
            }
            set
            {
                SetFloatInFile(value, unkownInt2Offset);
            }
        }

        public float OffsetV
        {
            get
            {
                return GetFloatFromFile(unkownInt3Offset, true);
            }
            set
            {
                SetFloatInFile(value, unkownInt3Offset);
            }
        }

        public BinaryMaterialV4Entry FloatWeight
        {
            get;
            set;
        } = null;

        public BinaryMaterialV4MapDefinition(FileCacheHolder fileCache, int offset, bool cache = false)
        : base(fileCache, offset, cache)
        {
        }
    }
}