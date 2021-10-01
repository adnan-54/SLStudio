namespace SlrrLib.Model
{
    public class BinaryCompleteVertexDataV4
    {
        public BasicXYZ Position
        {
            get;
            set;
        }

        public BinaryBoneWeightsV4 BoneWeightList
        {
            get;
            set;
        }

        public BinaryBoneIndRefV4 BoneIndRefs
        {
            get;
            set;
        }

        public BasicXYZ Normal
        {
            get;
            set;
        }

        public BasicRGBA VertexIllumination
        {
            get;
            set;
        }

        public BasicRGBA VertexColor
        {
            get;
            set;
        }

        public BasicUV UV1
        {
            get;
            set;
        }

        public BasicUV UV2
        {
            get;
            set;
        }

        public BasicUV UV3
        {
            get;
            set;
        }

        public BasicXYZ BumpNormal
        {
            get;
            set;
        }

        public string GetStringByFlag(BinaryScxV4VertexDataFlag flag)
        {
            switch (flag)
            {
                case BinaryScxV4VertexDataFlag.Position:
                    return Position.ToString();

                case BinaryScxV4VertexDataFlag.BoneWeightNumIs0:
                    return "";

                case BinaryScxV4VertexDataFlag.BoneWeightNumIs1:
                case BinaryScxV4VertexDataFlag.BoneWeightNumIs2:
                case BinaryScxV4VertexDataFlag.BoneWeightNumIs3:
                    return BoneWeightList.ToString();

                case BinaryScxV4VertexDataFlag.BoneIndRef:
                    return BoneIndRefs.ToString();

                case BinaryScxV4VertexDataFlag.Normal:
                    return Normal.ToString();

                case BinaryScxV4VertexDataFlag.VertexIllumination:
                    return VertexIllumination.ToString();

                case BinaryScxV4VertexDataFlag.VertexColor:
                    return VertexColor.ToString();

                case BinaryScxV4VertexDataFlag.UV1:
                    return UV1.ToString();

                case BinaryScxV4VertexDataFlag.UV2:
                    return UV2.ToString();

                case BinaryScxV4VertexDataFlag.UV3:
                    return UV3.ToString();

                case BinaryScxV4VertexDataFlag.BumpMapNormal:
                    return BumpNormal.ToString();

                default:
                    return "";
            }
        }
    }
}