using System;
using System.Text;

namespace SlrrLib.Model
{
    public class BinaryMaterialV3 : FileEntry
    {
        public static readonly int sizeOffset = 0;
        public static readonly int diffuseColorROffset = 4;
        public static readonly int diffuseColorGOffset = 8;
        public static readonly int diffuseColorBOffset = 12;
        public static readonly int opacityOffset = 16;
        public static readonly int specularColorROffset = 20;
        public static readonly int specularColorGOffset = 24;
        public static readonly int specularColorBOffset = 28;
        public static readonly int specularColorWeightOffset = 32;
        public static readonly int glossinessWeightOffset = 36;
        public static readonly int flagsOffset = 40;
        public static readonly int diffuseMapIndexOffset = 44;
        public static readonly int bumpMapIndexOffset = 46;
        public static readonly int specularMapIndexOffset = 48;
        public static readonly int reflectionMapIndexOffset = 50;
        public static readonly int unkownMapIndexOffset = 52;
        public static readonly int unkownMapIndex2Offset = 54;
        public static readonly int illuminationMapIndexOffset = 56;
        public static readonly int unkownMapIndex3Offset = 58;
        public static readonly int oneVertexSizeOffset = 60;
        public static readonly int unkownInt1Offset = 64;
        public static readonly int unkownShort1Offset = 68;
        public static readonly int diffMixFirstMapChanelOffset = 70;
        public static readonly int diffMixSecondMapChanelOffset = 72;
        public static readonly int bumpMapChanelOffset = 74;
        public static readonly int specularMapChanelOffset = 76;
        public static readonly int unkownShort2Offset = 78;
        public static readonly int unkownInt2Offset = 80;
        public static readonly int unkownInt3Offset = 84;
        public static readonly int unkownInt4Offset = 88;
        public static readonly int illuminationColorROffset = 92;
        public static readonly int illuminationColorGOffset = 96;
        public static readonly int illuminationColorBOffset = 100;
        public static readonly int materialNameOffset = 104;
        public static readonly int flagAlphaOpacity = 0x00000001;
        public static readonly int flagDiffuseBlend = 0x00000100;
        public static readonly int flagVertexColorBlend = 0x00000002;
        public static readonly int flagLayer2VertexColorBlend = 0x00001000;
        public static readonly int flagLayer2BlendByAlpha = 0x00000040;

        public float DiffuseColorR
        {
            get
            {
                return GetFloatFromFile(diffuseColorROffset);
            }
            set
            {
                SetFloatInFile(value, diffuseColorROffset);
            }
        }

        public float DiffuseColorG
        {
            get
            {
                return GetFloatFromFile(diffuseColorGOffset);
            }
            set
            {
                SetFloatInFile(value, diffuseColorGOffset);
            }
        }

        public float DiffuseColorB
        {
            get
            {
                return GetFloatFromFile(diffuseColorBOffset);
            }
            set
            {
                SetFloatInFile(value, diffuseColorBOffset);
            }
        }

        public float Opacity
        {
            get
            {
                return GetFloatFromFile(opacityOffset);
            }
            set
            {
                SetFloatInFile(value, opacityOffset);
            }
        }

        public float SpecularColorR
        {
            get
            {
                return GetFloatFromFile(specularColorROffset);
            }
            set
            {
                SetFloatInFile(value, specularColorROffset);
            }
        }

        public float SpecularColorG
        {
            get
            {
                return GetFloatFromFile(specularColorGOffset);
            }
            set
            {
                SetFloatInFile(value, specularColorGOffset);
            }
        }

        public float SpecularColorB
        {
            get
            {
                return GetFloatFromFile(specularColorBOffset);
            }
            set
            {
                SetFloatInFile(value, specularColorBOffset);
            }
        }

        public float SpecularColorWeight
        {
            get
            {
                return GetFloatFromFile(specularColorWeightOffset);
            }
            set
            {
                SetFloatInFile(value, specularColorWeightOffset);
            }
        }

        public float GlossinessWeight
        {
            get
            {
                return GetFloatFromFile(glossinessWeightOffset);
            }
            set
            {
                SetFloatInFile(value, glossinessWeightOffset);
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

        public int Flags//1 if there is one 0 otherwise (even if there is none)
        {
            get
            {
                return GetIntFromFile(flagsOffset);
            }
            set
            {
                SetIntInFile(value, flagsOffset);
            }
        }

        public short DiffuseMapIndex
        {
            get
            {
                return GetShortFromFile(diffuseMapIndexOffset);
            }
            set
            {
                SetShortInFile(value, diffuseMapIndexOffset);
            }
        }

        public short BumpMapIndex
        {
            get
            {
                return GetShortFromFile(bumpMapIndexOffset);
            }
            set
            {
                SetShortInFile(value, bumpMapIndexOffset);
            }
        }

        public short SpecularMapIndex
        {
            get
            {
                return GetShortFromFile(specularMapIndexOffset);
            }
            set
            {
                SetShortInFile(value, specularMapIndexOffset);
            }
        }

        public short ReflectionMapIndex
        {
            get
            {
                return GetShortFromFile(reflectionMapIndexOffset);
            }
            set
            {
                SetShortInFile(value, reflectionMapIndexOffset);
            }
        }

        public short DiffuseLayer2MapIndex
        {
            get
            {
                return GetShortFromFile(unkownMapIndexOffset);
            }
            set
            {
                SetShortInFile(value, unkownMapIndexOffset);
            }
        }

        public short UnkownMapIndex2
        {
            get
            {
                return GetShortFromFile(unkownMapIndex2Offset);
            }
            set
            {
                SetShortInFile(value, unkownMapIndex2Offset);
            }
        }

        public short IlluminationIndex
        {
            get
            {
                return GetShortFromFile(illuminationMapIndexOffset);
            }
            set
            {
                SetShortInFile(value, illuminationMapIndexOffset);
            }
        }

        public short UnkownMapIndex3
        {
            get
            {
                return GetShortFromFile(unkownMapIndex3Offset);
            }
            set
            {
                SetShortInFile(value, unkownMapIndex3Offset);
            }
        }

        public int OneVertexSize
        {
            get
            {
                int ret = GetIntFromFile(oneVertexSizeOffset);
                if (ret == -1)
                    ret = 44;
                return ret;
            }
            set
            {
                SetIntInFile(value, oneVertexSizeOffset);
            }
        }

        public int UnkownInt1
        {
            get
            {
                return GetIntFromFile(unkownInt1Offset);
            }
            set
            {
                SetIntInFile(value, unkownInt1Offset);
            }
        }

        public short UnkownShort1
        {
            get
            {
                return GetShortFromFile(unkownShort1Offset);
            }
            set
            {
                SetShortInFile(value, unkownShort1Offset);
            }
        }

        public short DiffuseMixFirstMapChannel
        {
            get
            {
                return GetShortFromFile(diffMixFirstMapChanelOffset);
            }
            set
            {
                SetShortInFile(value, diffMixFirstMapChanelOffset);
            }
        }

        public short DiffuseMixSecondMapChanel
        {
            get
            {
                return GetShortFromFile(diffMixSecondMapChanelOffset);
            }
            set
            {
                SetShortInFile(value, diffMixSecondMapChanelOffset);
            }
        }

        public short BumpMapChanel
        {
            get
            {
                return GetShortFromFile(bumpMapChanelOffset);
            }
            set
            {
                SetShortInFile(value, bumpMapChanelOffset);
            }
        }

        public short SpecularMapChanel
        {
            get
            {
                return GetShortFromFile(specularMapChanelOffset);
            }
            set
            {
                SetShortInFile(value, specularMapChanelOffset);
            }
        }

        public short UnkownShort2
        {
            get
            {
                return GetShortFromFile(unkownShort2Offset);
            }
            set
            {
                SetShortInFile(value, unkownShort2Offset);
            }
        }

        public int UnkownInt2
        {
            get
            {
                return GetIntFromFile(unkownInt2Offset);
            }
            set
            {
                SetIntInFile(value, unkownInt2Offset);
            }
        }

        public int UnkownInt3
        {
            get
            {
                return GetIntFromFile(unkownInt3Offset);
            }
            set
            {
                SetIntInFile(value, unkownInt3Offset);
            }
        }

        public int UnkownInt4
        {
            get
            {
                return GetIntFromFile(unkownInt4Offset);
            }
            set
            {
                SetIntInFile(value, unkownInt4Offset);
            }
        }

        public float IlluminationColorR
        {
            get
            {
                return GetFloatFromFile(illuminationColorROffset);
            }
            set
            {
                SetFloatInFile(value, illuminationColorROffset);
            }
        }

        public float IlluminationColorG
        {
            get
            {
                return GetFloatFromFile(illuminationColorGOffset);
            }
            set
            {
                SetFloatInFile(value, illuminationColorGOffset);
            }
        }

        public float IlluminationColorB
        {
            get
            {
                return GetFloatFromFile(illuminationColorBOffset);
            }
            set
            {
                SetFloatInFile(value, illuminationColorBOffset);
            }
        }

        public string MaterialName
        {
            get
            {
                if (Size == 136)
                {
                    return ASCIIEncoding.ASCII.GetString(GetFileData(), Offset + materialNameOffset, 32);
                }
                return "";
            }
            set
            {
                string realSet = value;
                if (realSet.Length > 32)
                    realSet = value.Remove(32);
                Cache.CacheData();
                byte[] stringByte = ASCIIEncoding.ASCII.GetBytes(realSet);
                Array.Copy(stringByte, 0, Cache.GetFileData(), Offset + materialNameOffset, 32);
            }
        }

        public override int Size
        {
            get
            {
                return GetIntFromFile(sizeOffset, true);
            }
            set
            {
                SetIntInFile(value, sizeOffset);
            }
        }

        public BinaryMaterialV3(FileCacheHolder fileCache, int offset, bool cache = false)
        : base(fileCache, offset, cache)
        {
        }

        public void SetReflectionStrength(float percent)
        {
            float realPercent = Math.Min(Math.Max(0.0f, percent), 1.0f);
            SpecularColorR = SpecularColorG = SpecularColorB = realPercent;
        }

        public float ReflectionStrength()
        {
            return (SpecularColorR + SpecularColorG + SpecularColorB / (255.0f * 3.0f));
        }

        public bool HasReflection()
        {
            return SpecularColorR != -1;
        }
    }
}