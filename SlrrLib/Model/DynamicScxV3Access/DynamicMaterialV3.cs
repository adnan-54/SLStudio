using System;
using System.IO;
using System.Text;

namespace SlrrLib.Model
{
    public class DynamicMaterialV3
    {
        private const int flagAlphaOpacity = 0x00000001;
        private const int flagDiffuseBlend = 0x00000100;
        private const int flagVertexColorBlend = 0x00000002;
        private const int flagLayer2VertexColorBlend = 0x00001000;
        private const int flagLayer2BlendByAlpha = 0x00000040;

        private float diffuseColorR;
        private float diffuseColorG;
        private float diffuseColorB;
        private float opacity;
        private float specularColorR;
        private float specularColorG;
        private float specularColorB;
        private float specularColorWeight;
        private float glossinessWeight;
        private int flags;//1 if there is one 0 otherwise (even if there is none)
        private short diffuseMapIndex;
        private short bumpMapIndex;
        private short specularMapIndex;
        private short reflectionMapIndex;
        private short diffuseLayer2MapIndex;
        private short unkownMapIndex2;
        private short illuminationIndex;
        private short unkownMapIndex3;
        private int oneVertexSize;
        private int unkownInt1;
        private short unkownShort1;
        private short diffuseMixFirstMapChannel;
        private short diffuseMixSecondMapChanel;
        private short bumpMapChanel;
        private short specularMapChanel;
        private short unkownShort2;
        private int unkownInt2;
        private int unkownInt3;
        private int unkownInt4;
        private float illuminationColorR;
        private float illuminationColorG;
        private float illuminationColorB;
        private string materialName;

        public bool IsDiffuseColorRDefined
        {
            get;
            set;
        }

        public bool IsDiffuseColorGDefined
        {
            get;
            set;
        }

        public bool IsDiffuseColorBDefined
        {
            get;
            set;
        }

        public bool IsOpacityDefined
        {
            get;
            set;
        }

        public bool IsSpecularColorRDefined
        {
            get;
            set;
        }

        public bool IsSpecularColorGDefined
        {
            get;
            set;
        }

        public bool IsSpecularColorBDefined
        {
            get;
            set;
        }

        public bool IsSpecularColorWeightDefined
        {
            get;
            set;
        }

        public bool IsGlossinessWeightDefined
        {
            get;
            set;
        }

        public bool IsFlagsDefined
        {
            get;
            set;
        }

        public bool IsDiffuseMapIndexDefined
        {
            get;
            set;
        }

        public bool IsBumpMapIndexDefined
        {
            get;
            set;
        }

        public bool IsSpecularMapIndexDefined
        {
            get;
            set;
        }

        public bool IsReflectionMapIndexDefined
        {
            get;
            set;
        }

        public bool IsDiffuseLayer2MapIndexDefined
        {
            get;
            set;
        }

        public bool IsUnkownMapIndex2Defined
        {
            get;
            set;
        }

        public bool IsIlluminationIndexDefined
        {
            get;
            set;
        }

        public bool IsUnkownMapIndex3Defined
        {
            get;
            set;
        }

        public bool IsOneVertexSizeDefined
        {
            get;
            set;
        }

        public bool IsUnkownInt1Defined
        {
            get;
            set;
        }

        public bool IsUnkownShort1Defined
        {
            get;
            set;
        }

        public bool IsDiffuseMixFirstMapChannelDefined
        {
            get;
            set;
        }

        public bool IsDiffuseMixSecondMapChanelDefined
        {
            get;
            set;
        }

        public bool IsBumpMapChanelDefined
        {
            get;
            set;
        }

        public bool IsSpecularMapChanelDefined
        {
            get;
            set;
        }

        public bool IsUnkownShort2Defined
        {
            get;
            set;
        }

        public bool IsUnkownInt2Defined
        {
            get;
            set;
        }

        public bool IsUnkownInt3Defined
        {
            get;
            set;
        }

        public bool IsUnkownInt4Defined
        {
            get;
            set;
        }

        public bool IsIlluminationColorRDefined
        {
            get;
            set;
        }

        public bool IsIlluminationColorGDefined
        {
            get;
            set;
        }

        public bool IsIlluminationColorBDefined
        {
            get;
            set;
        }

        public bool IsMaterialNameDefined
        {
            get;
            set;
        }

        public float DiffuseColorR
        {
            get
            {
                return diffuseColorR;
            }
            set
            {
                diffuseColorR = value;
                IsDiffuseColorRDefined = true;
            }
        }

        public float DiffuseColorG
        {
            get
            {
                return diffuseColorG;
            }
            set
            {
                diffuseColorG = value;
                IsDiffuseColorGDefined = true;
            }
        }

        public float DiffuseColorB
        {
            get
            {
                return diffuseColorB;
            }
            set
            {
                diffuseColorB = value;
                IsDiffuseColorBDefined = true;
            }
        }

        public float Opacity
        {
            get
            {
                return opacity;
            }
            set
            {
                opacity = value;
                IsOpacityDefined = true;
            }
        }

        public float SpecularColorR
        {
            get
            {
                return specularColorR;
            }
            set
            {
                specularColorR = value;
                IsSpecularColorRDefined = true;
            }
        }

        public float SpecularColorG
        {
            get
            {
                return specularColorG;
            }
            set
            {
                specularColorG = value;
                IsSpecularColorGDefined = true;
            }
        }

        public float SpecularColorB
        {
            get
            {
                return specularColorB;
            }
            set
            {
                specularColorB = value;
                IsSpecularColorBDefined = true;
            }
        }

        public float SpecularColorWeight
        {
            get
            {
                return specularColorWeight;
            }
            set
            {
                specularColorWeight = value;
                IsSpecularColorWeightDefined = true;
            }
        }

        public float GlossinessWeight
        {
            get
            {
                return glossinessWeight;
            }
            set
            {
                glossinessWeight = value;
                IsGlossinessWeightDefined = true;
            }
        }

        public bool FlagAlphaOpacity
        {
            get
            {
                return (Flags & flagAlphaOpacity) > 0;
            }
            set
            {
                if (value)
                    Flags |= flagAlphaOpacity;
                else
                    Flags &= ~flagAlphaOpacity;
            }
        }

        public bool FlagLayer1DiffuseBlend
        {
            get
            {
                return (Flags & flagDiffuseBlend) > 0;
            }
            set
            {
                if (value)
                    Flags |= flagDiffuseBlend;
                else
                    Flags &= ~flagDiffuseBlend;
            }
        }

        public bool FlagLayer1VertexColorBlend
        {
            get
            {
                return (Flags & flagVertexColorBlend) > 0;
            }
            set
            {
                if (value)
                    Flags |= flagVertexColorBlend;
                else
                    Flags &= ~flagVertexColorBlend;
            }
        }

        public bool FlagLayer2VertexColorBlend
        {
            get
            {
                return (Flags & flagLayer2VertexColorBlend) > 0;
            }
            set
            {
                if (value)
                    Flags |= flagLayer2VertexColorBlend;
                else
                    Flags &= ~flagLayer2VertexColorBlend;
            }
        }

        public bool FlagLayer2BlendByAlpha
        {
            get
            {
                return (Flags & flagLayer2BlendByAlpha) > 0;
            }
            set
            {
                if (value)
                    Flags |= flagLayer2BlendByAlpha;
                else
                    Flags &= ~flagLayer2BlendByAlpha;
            }
        }

        public int Flags
        {
            get
            {
                return flags;
            }
            set
            {
                flags = value;
                IsFlagsDefined = true;
            }
        }

        public short DiffuseMapIndex
        {
            get
            {
                return diffuseMapIndex;
            }
            set
            {
                diffuseMapIndex = value;
                IsDiffuseMapIndexDefined = true;
            }
        }

        public short BumpMapIndex
        {
            get
            {
                return bumpMapIndex;
            }
            set
            {
                bumpMapIndex = value;
                IsBumpMapIndexDefined = true;
            }
        }

        public short SpecularMapIndex
        {
            get
            {
                return specularMapIndex;
            }
            set
            {
                specularMapIndex = value;
                IsSpecularMapIndexDefined = true;
            }
        }

        public short ReflectionMapIndex
        {
            get
            {
                return reflectionMapIndex;
            }
            set
            {
                reflectionMapIndex = value;
                IsReflectionMapIndexDefined = true;
            }
        }

        public short DiffuseLayer2MapIndex
        {
            get
            {
                return diffuseLayer2MapIndex;
            }
            set
            {
                diffuseLayer2MapIndex = value;
                IsDiffuseLayer2MapIndexDefined = true;
            }
        }

        public short UnkownMapIndex2
        {
            get
            {
                return unkownMapIndex2;
            }
            set
            {
                unkownMapIndex2 = value;
                IsUnkownMapIndex2Defined = true;
            }
        }

        public short IlluminationIndex
        {
            get
            {
                return illuminationIndex;
            }
            set
            {
                illuminationIndex = value;
                IsIlluminationIndexDefined = true;
            }
        }

        public short UnkownMapIndex3
        {
            get
            {
                return unkownMapIndex3;
            }
            set
            {
                unkownMapIndex3 = value;
                IsUnkownMapIndex3Defined = true;
            }
        }

        public int OneVertexSize
        {
            get
            {
                return oneVertexSize;
            }
            set
            {
                oneVertexSize = value;
                IsOneVertexSizeDefined = true;
            }
        }

        public short DiffuseMixSecondMapChanel
        {
            get
            {
                return diffuseMixSecondMapChanel;
            }
            set
            {
                diffuseMixSecondMapChanel = value;
                IsDiffuseMixSecondMapChanelDefined = true;
            }
        }

        public int UnkownInt1
        {
            get
            {
                return unkownInt1;
            }
            set
            {
                unkownInt1 = value;
                IsUnkownInt1Defined = true;
            }
        }

        public short UnkownShort1
        {
            get
            {
                return unkownShort1;
            }
            set
            {
                unkownShort1 = value;
                IsUnkownShort1Defined = true;
            }
        }

        public short DiffuseMixFirstMapChannel
        {
            get
            {
                return diffuseMixFirstMapChannel;
            }
            set
            {
                diffuseMixFirstMapChannel = value;
                IsDiffuseMixFirstMapChannelDefined = true;
            }
        }

        public short BumpMapChanel
        {
            get
            {
                return bumpMapChanel;
            }
            set
            {
                bumpMapChanel = value;
                IsBumpMapChanelDefined = true;
            }
        }

        public short SpecularMapChanel
        {
            get
            {
                return specularMapChanel;
            }
            set
            {
                specularMapChanel = value;
                IsSpecularMapChanelDefined = true;
            }
        }

        public short UnkownShort2
        {
            get
            {
                return unkownShort2;
            }
            set
            {
                unkownShort2 = value;
                IsUnkownShort2Defined = true;
            }
        }

        public int UnkownInt2
        {
            get
            {
                return unkownInt2;
            }
            set
            {
                unkownInt2 = value;
                IsUnkownInt2Defined = true;
            }
        }

        public int UnkownInt3
        {
            get
            {
                return unkownInt3;
            }
            set
            {
                unkownInt3 = value;
                IsUnkownInt3Defined = true;
            }
        }

        public int UnkownInt4
        {
            get
            {
                return unkownInt4;
            }
            set
            {
                unkownInt4 = value;
                IsUnkownInt4Defined = true;
            }
        }

        public float IlluminationColorR
        {
            get
            {
                return illuminationColorR;
            }
            set
            {
                illuminationColorR = value;
                IsIlluminationColorRDefined = true;
            }
        }

        public float IlluminationColorG
        {
            get
            {
                return illuminationColorG;
            }
            set
            {
                illuminationColorG = value;
                IsIlluminationColorGDefined = true;
            }
        }

        public float IlluminationColorB
        {
            get
            {
                return illuminationColorB;
            }
            set
            {
                illuminationColorB = value;
                IsIlluminationColorBDefined = true;
            }
        }

        public string MaterialName
        {
            get
            {
                return materialName;
            }
            set
            {
                string realSet = value;
                if (realSet.Length > 32)
                    realSet = value.Remove(32);
                while (realSet.Length != 32)
                    realSet += "\0";
                materialName = realSet;
                IsMaterialNameDefined = true;
            }
        }

        public DynamicMaterialV3(BinaryMaterialV3 constructFrom = null)
        {
            if (constructFrom == null)
                return;
            constructFrom.Cache.CacheData();

            IsDiffuseColorRDefined = false;
            IsDiffuseColorGDefined = false;
            IsDiffuseColorBDefined = false;
            IsOpacityDefined = false;
            IsSpecularColorRDefined = false;
            IsSpecularColorGDefined = false;
            IsSpecularColorBDefined = false;
            IsSpecularColorWeightDefined = false;
            IsGlossinessWeightDefined = false;
            IsFlagsDefined = false;
            IsDiffuseMapIndexDefined = false;
            IsBumpMapIndexDefined = false;
            IsSpecularMapIndexDefined = false;
            IsReflectionMapIndexDefined = false;
            IsDiffuseLayer2MapIndexDefined = false;
            IsUnkownMapIndex2Defined = false;
            IsIlluminationIndexDefined = false;
            IsUnkownMapIndex3Defined = false;
            IsOneVertexSizeDefined = false;

            IsUnkownInt1Defined = false;
            IsUnkownShort1Defined = false;
            IsDiffuseMixFirstMapChannelDefined = false;
            IsDiffuseMixSecondMapChanelDefined = false;
            IsBumpMapChanelDefined = false;
            IsSpecularMapChanelDefined = false;
            IsUnkownShort2Defined = false;
            IsUnkownInt2Defined = false;
            IsUnkownInt3Defined = false;
            IsUnkownInt4Defined = false;

            IsIlluminationColorRDefined = false;
            IsIlluminationColorGDefined = false;
            IsIlluminationColorBDefined = false;
            IsMaterialNameDefined = false;

            int originalSize = constructFrom.Size;
            if (originalSize >= BinaryMaterialV3.diffuseColorROffset + sizeof(float))
            {
                IsDiffuseColorRDefined = true;
                diffuseColorR = constructFrom.DiffuseColorR;
            }
            if (originalSize >= BinaryMaterialV3.diffuseColorGOffset + sizeof(float))
            {
                IsDiffuseColorGDefined = true;
                diffuseColorG = constructFrom.DiffuseColorG;
            }
            if (originalSize >= BinaryMaterialV3.diffuseColorBOffset + sizeof(float))
            {
                IsDiffuseColorBDefined = true;
                diffuseColorB = constructFrom.DiffuseColorB;
            }
            if (originalSize >= BinaryMaterialV3.opacityOffset + sizeof(float))
            {
                IsOpacityDefined = true;
                opacity = constructFrom.Opacity;
            }
            if (originalSize >= BinaryMaterialV3.specularColorROffset + sizeof(float))
            {
                IsSpecularColorRDefined = true;
                specularColorR = constructFrom.SpecularColorR;
            }
            if (originalSize >= BinaryMaterialV3.specularColorGOffset + sizeof(float))
            {
                IsSpecularColorGDefined = true;
                specularColorG = constructFrom.SpecularColorG;
            }
            if (originalSize >= BinaryMaterialV3.specularColorBOffset + sizeof(float))
            {
                IsSpecularColorBDefined = true;
                specularColorB = constructFrom.SpecularColorB;
            }
            if (originalSize >= BinaryMaterialV3.specularColorWeightOffset + sizeof(float))
            {
                IsSpecularColorWeightDefined = true;
                specularColorWeight = constructFrom.SpecularColorWeight;
            }
            if (originalSize >= BinaryMaterialV3.glossinessWeightOffset + sizeof(float))
            {
                IsGlossinessWeightDefined = true;
                glossinessWeight = constructFrom.GlossinessWeight;
            }
            if (originalSize >= BinaryMaterialV3.flagsOffset + sizeof(int))
            {
                IsFlagsDefined = true;
                flags = constructFrom.Flags;
            }
            if (originalSize >= BinaryMaterialV3.diffuseMapIndexOffset + sizeof(short))
            {
                IsDiffuseMapIndexDefined = true;
                diffuseMapIndex = constructFrom.DiffuseMapIndex;
            }
            if (originalSize >= BinaryMaterialV3.bumpMapIndexOffset + sizeof(short))
            {
                IsBumpMapIndexDefined = true;
                bumpMapIndex = constructFrom.BumpMapIndex;
            }
            if (originalSize >= BinaryMaterialV3.specularMapIndexOffset + sizeof(short))
            {
                IsSpecularMapIndexDefined = true;
                specularMapIndex = constructFrom.SpecularMapIndex;
            }
            if (originalSize >= BinaryMaterialV3.reflectionMapIndexOffset + sizeof(short))
            {
                IsReflectionMapIndexDefined = true;
                reflectionMapIndex = constructFrom.ReflectionMapIndex;
            }
            if (originalSize >= BinaryMaterialV3.unkownMapIndexOffset + sizeof(short))
            {
                IsDiffuseLayer2MapIndexDefined = true;
                diffuseLayer2MapIndex = constructFrom.DiffuseLayer2MapIndex;
            }
            if (originalSize >= BinaryMaterialV3.unkownMapIndex2Offset + sizeof(short))
            {
                IsUnkownMapIndex2Defined = true;
                unkownMapIndex2 = constructFrom.UnkownMapIndex2;
            }
            if (originalSize >= BinaryMaterialV3.illuminationMapIndexOffset + sizeof(short))
            {
                IsIlluminationIndexDefined = true;
                illuminationIndex = constructFrom.IlluminationIndex;
            }
            if (originalSize >= BinaryMaterialV3.unkownMapIndex3Offset + sizeof(short))
            {
                IsUnkownMapIndex3Defined = true;
                unkownMapIndex3 = constructFrom.UnkownMapIndex3;
            }
            if (originalSize >= BinaryMaterialV3.oneVertexSizeOffset + sizeof(int))
            {
                IsOneVertexSizeDefined = true;
                oneVertexSize = constructFrom.OneVertexSize;
            }
            if (originalSize >= BinaryMaterialV3.unkownInt1Offset + sizeof(int))
            {
                IsUnkownInt1Defined = true;
                UnkownInt1 = constructFrom.UnkownInt1;
            }
            if (originalSize >= BinaryMaterialV3.unkownInt2Offset + sizeof(int))
            {
                IsUnkownInt2Defined = true;
                UnkownInt2 = constructFrom.UnkownInt2;
            }
            if (originalSize >= BinaryMaterialV3.unkownInt3Offset + sizeof(int))
            {
                IsUnkownInt3Defined = true;
                UnkownInt3 = constructFrom.UnkownInt3;
            }
            if (originalSize >= BinaryMaterialV3.unkownInt4Offset + sizeof(int))
            {
                IsUnkownInt4Defined = true;
                UnkownInt4 = constructFrom.UnkownInt4;
            }
            if (originalSize >= BinaryMaterialV3.unkownShort1Offset + sizeof(short))
            {
                IsUnkownShort1Defined = true;
                unkownShort1 = constructFrom.UnkownShort1;
            }
            if (originalSize >= BinaryMaterialV3.unkownShort2Offset + sizeof(short))
            {
                IsUnkownShort2Defined = true;
                unkownShort2 = constructFrom.UnkownShort2;
            }
            if (originalSize >= BinaryMaterialV3.diffMixFirstMapChanelOffset + sizeof(short))
            {
                IsDiffuseMixFirstMapChannelDefined = true;
                diffuseMixFirstMapChannel = constructFrom.DiffuseMixFirstMapChannel;
            }
            if (originalSize >= BinaryMaterialV3.diffMixSecondMapChanelOffset + sizeof(short))
            {
                IsDiffuseMixSecondMapChanelDefined = true;
                diffuseMixSecondMapChanel = constructFrom.DiffuseMixSecondMapChanel;
            }
            if (originalSize >= BinaryMaterialV3.bumpMapChanelOffset + sizeof(short))
            {
                IsBumpMapChanelDefined = true;
                bumpMapChanel = constructFrom.BumpMapChanel;
            }
            if (originalSize >= BinaryMaterialV3.specularMapChanelOffset + sizeof(short))
            {
                IsSpecularMapChanelDefined = true;
                specularMapChanel = constructFrom.SpecularMapChanel;
            }
            if (originalSize >= BinaryMaterialV3.illuminationColorROffset + sizeof(float))
            {
                IsIlluminationColorRDefined = true;
                illuminationColorR = constructFrom.IlluminationColorR;
            }
            if (originalSize >= BinaryMaterialV3.illuminationColorGOffset + sizeof(float))
            {
                IsIlluminationColorGDefined = true;
                illuminationColorG = constructFrom.IlluminationColorG;
            }
            if (originalSize >= BinaryMaterialV3.illuminationColorBOffset + sizeof(float))
            {
                IsIlluminationColorBDefined = true;
                illuminationColorB = constructFrom.IlluminationColorB;
            }
            if (originalSize >= BinaryMaterialV3.materialNameOffset)
            {
                IsMaterialNameDefined = true;
                materialName = constructFrom.MaterialName;
            }
        }

        public void SetReflectionStrength(float percent)
        {
            float realPercent = Math.Min(Math.Max(0.0f, percent), 1.0f);
            specularColorR = specularColorG = specularColorB = realPercent;
        }

        public float ReflectionStrength()
        {
            return (specularColorR + specularColorG + specularColorB / (255.0f * 3.0f));
        }

        public bool HasReflection()
        {
            return specularColorR != -1;
        }

        public void FixIsDefines()
        {
            if (IsMaterialNameDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = IsSpecularColorWeightDefined = IsGlossinessWeightDefined = IsFlagsDefined = IsDiffuseMapIndexDefined = IsBumpMapIndexDefined = IsSpecularMapIndexDefined = IsReflectionMapIndexDefined = IsDiffuseLayer2MapIndexDefined = IsUnkownMapIndex2Defined = IsIlluminationIndexDefined = IsUnkownMapIndex3Defined = IsOneVertexSizeDefined = IsIlluminationColorRDefined = IsIlluminationColorGDefined = IsIlluminationColorBDefined = true;
            else if (IsIlluminationColorBDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = IsSpecularColorWeightDefined = IsGlossinessWeightDefined = IsFlagsDefined = IsDiffuseMapIndexDefined = IsBumpMapIndexDefined = IsSpecularMapIndexDefined = IsReflectionMapIndexDefined = IsDiffuseLayer2MapIndexDefined = IsUnkownMapIndex2Defined = IsIlluminationIndexDefined = IsUnkownMapIndex3Defined = IsOneVertexSizeDefined = IsIlluminationColorRDefined = IsIlluminationColorGDefined = IsIlluminationColorBDefined = true;
            else if (IsIlluminationColorGDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = IsSpecularColorWeightDefined = IsGlossinessWeightDefined = IsFlagsDefined = IsDiffuseMapIndexDefined = IsBumpMapIndexDefined = IsSpecularMapIndexDefined = IsReflectionMapIndexDefined = IsDiffuseLayer2MapIndexDefined = IsUnkownMapIndex2Defined = IsIlluminationIndexDefined = IsUnkownMapIndex3Defined = IsOneVertexSizeDefined = IsIlluminationColorRDefined = IsIlluminationColorGDefined = IsIlluminationColorBDefined = true;
            else if (IsIlluminationColorRDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = IsSpecularColorWeightDefined = IsGlossinessWeightDefined = IsFlagsDefined = IsDiffuseMapIndexDefined = IsBumpMapIndexDefined = IsSpecularMapIndexDefined = IsReflectionMapIndexDefined = IsDiffuseLayer2MapIndexDefined = IsUnkownMapIndex2Defined = IsIlluminationIndexDefined = IsUnkownMapIndex3Defined = IsOneVertexSizeDefined = IsIlluminationColorRDefined = IsIlluminationColorGDefined = IsIlluminationColorBDefined = true;
            else if (IsOneVertexSizeDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = IsSpecularColorWeightDefined = IsGlossinessWeightDefined = IsFlagsDefined = IsDiffuseMapIndexDefined = IsBumpMapIndexDefined = IsSpecularMapIndexDefined = IsReflectionMapIndexDefined = IsDiffuseLayer2MapIndexDefined = IsUnkownMapIndex2Defined = IsIlluminationIndexDefined = IsUnkownMapIndex3Defined = true;
            else if (IsUnkownMapIndex3Defined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = IsSpecularColorWeightDefined = IsGlossinessWeightDefined = IsFlagsDefined = IsDiffuseMapIndexDefined = IsBumpMapIndexDefined = IsSpecularMapIndexDefined = IsReflectionMapIndexDefined = IsDiffuseLayer2MapIndexDefined = IsUnkownMapIndex2Defined = IsIlluminationIndexDefined = true;
            else if (IsIlluminationIndexDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = IsSpecularColorWeightDefined = IsGlossinessWeightDefined = IsFlagsDefined = IsDiffuseMapIndexDefined = IsBumpMapIndexDefined = IsSpecularMapIndexDefined = IsReflectionMapIndexDefined = IsDiffuseLayer2MapIndexDefined = IsUnkownMapIndex2Defined = true;
            else if (IsUnkownMapIndex2Defined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = IsSpecularColorWeightDefined = IsGlossinessWeightDefined = IsFlagsDefined = IsDiffuseMapIndexDefined = IsBumpMapIndexDefined = IsSpecularMapIndexDefined = IsReflectionMapIndexDefined = IsDiffuseLayer2MapIndexDefined = true;
            else if (IsDiffuseLayer2MapIndexDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = IsSpecularColorWeightDefined = IsGlossinessWeightDefined = IsFlagsDefined = IsDiffuseMapIndexDefined = IsBumpMapIndexDefined = IsSpecularMapIndexDefined = IsReflectionMapIndexDefined = true;
            else if (IsReflectionMapIndexDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = IsSpecularColorWeightDefined = IsGlossinessWeightDefined = IsFlagsDefined = IsDiffuseMapIndexDefined = IsBumpMapIndexDefined = IsSpecularMapIndexDefined = true;
            else if (IsSpecularMapIndexDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = IsSpecularColorWeightDefined = IsGlossinessWeightDefined = IsFlagsDefined = IsDiffuseMapIndexDefined = IsBumpMapIndexDefined = true;
            else if (IsBumpMapIndexDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = IsSpecularColorWeightDefined = IsGlossinessWeightDefined = IsFlagsDefined = IsDiffuseMapIndexDefined = true;
            else if (IsDiffuseMapIndexDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = IsSpecularColorWeightDefined = IsGlossinessWeightDefined = IsFlagsDefined = true;
            else if (IsFlagsDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = IsSpecularColorWeightDefined = IsGlossinessWeightDefined = true;
            else if (IsGlossinessWeightDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = IsSpecularColorWeightDefined = true;
            else if (IsSpecularColorWeightDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = true;
            else if (IsSpecularColorBDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = true;
            else if (IsSpecularColorGDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = true;
            else if (IsSpecularColorRDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = IsOpacityDefined = IsSpecularColorRDefined = IsSpecularColorGDefined = IsSpecularColorBDefined = true;
            else if (IsOpacityDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = true;
            else if (IsDiffuseColorBDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = true;
            else if (IsDiffuseColorGDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = true;
            else if (IsDiffuseColorBDefined)
                IsDiffuseColorRDefined = IsDiffuseColorGDefined = IsDiffuseColorBDefined = true;
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(calcSize());
            if (IsDiffuseColorRDefined)
                bw.Write(DiffuseColorR);
            if (IsDiffuseColorGDefined)
                bw.Write(DiffuseColorG);
            if (IsDiffuseColorBDefined)
                bw.Write(DiffuseColorB);
            if (IsOpacityDefined)
                bw.Write(Opacity);
            if (IsSpecularColorRDefined)
                bw.Write(SpecularColorR);
            if (IsSpecularColorGDefined)
                bw.Write(SpecularColorG);
            if (IsSpecularColorBDefined)
                bw.Write(SpecularColorB);
            if (IsSpecularColorWeightDefined)
                bw.Write(SpecularColorWeight);
            if (IsGlossinessWeightDefined)
                bw.Write(GlossinessWeight);
            if (IsFlagsDefined)
                bw.Write(Flags);
            if (IsDiffuseMapIndexDefined)
                bw.Write(DiffuseMapIndex);
            if (IsBumpMapIndexDefined)
                bw.Write(BumpMapIndex);
            if (IsSpecularMapIndexDefined)
                bw.Write(SpecularMapIndex);
            if (IsReflectionMapIndexDefined)
                bw.Write(ReflectionMapIndex);
            if (IsDiffuseLayer2MapIndexDefined)
                bw.Write(DiffuseLayer2MapIndex);
            if (IsUnkownMapIndex2Defined)
                bw.Write(UnkownMapIndex2);
            if (IsIlluminationIndexDefined)
                bw.Write(IlluminationIndex);
            if (IsUnkownMapIndex3Defined)
                bw.Write(UnkownMapIndex3);
            if (IsOneVertexSizeDefined)
                bw.Write(OneVertexSize);

            if (IsUnkownInt1Defined)
                bw.Write(unkownInt1);
            if (IsUnkownShort1Defined)
                bw.Write(unkownShort1);
            if (IsDiffuseMixFirstMapChannelDefined)
                bw.Write(diffuseMixFirstMapChannel);
            if (IsDiffuseMixSecondMapChanelDefined)
                bw.Write(diffuseMixSecondMapChanel);
            if (IsBumpMapChanelDefined)
                bw.Write(bumpMapChanel);
            if (IsSpecularMapChanelDefined)
                bw.Write(specularMapChanel);
            if (IsUnkownShort2Defined)
                bw.Write(unkownShort2);
            if (IsUnkownInt2Defined)
                bw.Write(unkownInt2);
            if (IsUnkownInt3Defined)
                bw.Write(unkownInt3);
            if (IsUnkownInt4Defined)
                bw.Write(unkownInt4);

            if (IsIlluminationColorRDefined)
                bw.Write(IlluminationColorR);
            if (IsIlluminationColorGDefined)
                bw.Write(IlluminationColorG);
            if (IsIlluminationColorBDefined)
                bw.Write(IlluminationColorB);
            if (IsMaterialNameDefined)
                bw.Write(ASCIIEncoding.ASCII.GetBytes(MaterialName));
        }

        private int calcSize()
        {
            int sum = 4;//size is written too
            if (IsDiffuseColorRDefined)
                sum += sizeof(float);
            if (IsDiffuseColorGDefined)
                sum += sizeof(float);
            if (IsDiffuseColorBDefined)
                sum += sizeof(float);
            if (IsOpacityDefined)
                sum += sizeof(float);
            if (IsSpecularColorRDefined)
                sum += sizeof(float);
            if (IsSpecularColorGDefined)
                sum += sizeof(float);
            if (IsSpecularColorBDefined)
                sum += sizeof(float);
            if (IsSpecularColorWeightDefined)
                sum += sizeof(float);
            if (IsGlossinessWeightDefined)
                sum += sizeof(float);
            if (IsFlagsDefined)
                sum += sizeof(int);
            if (IsDiffuseMapIndexDefined)
                sum += sizeof(short);
            if (IsBumpMapIndexDefined)
                sum += sizeof(short);
            if (IsSpecularMapIndexDefined)
                sum += sizeof(short);
            if (IsReflectionMapIndexDefined)
                sum += sizeof(short);
            if (IsDiffuseLayer2MapIndexDefined)
                sum += sizeof(short);
            if (IsUnkownMapIndex2Defined)
                sum += sizeof(short);
            if (IsIlluminationIndexDefined)
                sum += sizeof(short);
            if (IsUnkownMapIndex3Defined)
                sum += sizeof(short);
            if (IsOneVertexSizeDefined)
                sum += sizeof(int);

            if (IsUnkownInt1Defined)
                sum += sizeof(int);
            if (IsUnkownInt2Defined)
                sum += sizeof(int);
            if (IsUnkownInt3Defined)
                sum += sizeof(int);
            if (IsUnkownInt4Defined)
                sum += sizeof(int);
            if (IsUnkownShort1Defined)
                sum += sizeof(short);
            if (IsUnkownShort2Defined)
                sum += sizeof(short);
            if (IsDiffuseMixFirstMapChannelDefined)
                sum += sizeof(short);
            if (IsDiffuseMixSecondMapChanelDefined)
                sum += sizeof(short);
            if (IsBumpMapChanelDefined)
                sum += sizeof(short);
            if (IsSpecularMapChanelDefined)
                sum += sizeof(short);

            if (IsIlluminationColorRDefined)
                sum += sizeof(float);
            if (IsIlluminationColorGDefined)
                sum += sizeof(float);
            if (IsIlluminationColorBDefined)
                sum += sizeof(float);
            if (IsMaterialNameDefined)
                sum += MaterialName.Length;

            return sum;
        }
    }
}