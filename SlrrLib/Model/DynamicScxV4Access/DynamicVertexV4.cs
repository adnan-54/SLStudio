using System;
using System.Collections.Generic;
using System.IO;

namespace SlrrLib.Model
{
    public class DynamicVertexV4
    {
        public static readonly int TypeDefer = 4;

        public bool IsPositionDefined
        {
            get
            {
                return (VertexType & (int)BinaryScxV4VertexDataFlag.Position) != 0;
            }
            set
            {
                if (value)
                {
                    VertexType |= (int)BinaryScxV4VertexDataFlag.Position;
                }
                else
                {
                    VertexType &= ~(int)BinaryScxV4VertexDataFlag.Position;
                }
            }
        }

        public bool IsBoneWeightNumIs0Defined
        {
            get
            {
                return (VertexType & (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs0) != 0;
            }
            set
            {
                if (value)
                {
                    IsBoneWeightNumIs1Defined = false;
                    IsBoneWeightNumIs2Defined = false;
                    IsBoneWeightNumIs3Defined = false;
                    VertexType |= (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs0;
                }
                else
                {
                    VertexType &= ~(int)BinaryScxV4VertexDataFlag.BoneWeightNumIs0;
                }
            }
        }

        public bool IsBoneWeightNumIs1Defined
        {
            get
            {
                return (VertexType & (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs1) != 0;
            }
            set
            {
                if (value)
                {
                    IsBoneWeightNumIs0Defined = false;
                    IsBoneWeightNumIs2Defined = false;
                    IsBoneWeightNumIs3Defined = false;
                    VertexType |= (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs1;
                }
                else
                {
                    VertexType &= ~(int)BinaryScxV4VertexDataFlag.BoneWeightNumIs1;
                }
            }
        }

        public bool IsBoneWeightNumIs2Defined
        {
            get
            {
                return (VertexType & (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs2) != 0;
            }
            set
            {
                if (value)
                {
                    IsBoneWeightNumIs0Defined = false;
                    IsBoneWeightNumIs1Defined = false;
                    IsBoneWeightNumIs3Defined = false;
                    VertexType |= (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs2;
                }
                else
                {
                    VertexType &= ~(int)BinaryScxV4VertexDataFlag.BoneWeightNumIs2;
                }
            }
        }

        public bool IsBoneWeightNumIs3Defined
        {
            get
            {
                return (VertexType & (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs3) != 0;
            }
            set
            {
                if (value)
                {
                    IsBoneWeightNumIs0Defined = false;
                    IsBoneWeightNumIs1Defined = false;
                    IsBoneWeightNumIs2Defined = false;
                    VertexType |= (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs3;
                }
                else
                {
                    VertexType &= ~(int)BinaryScxV4VertexDataFlag.BoneWeightNumIs3;
                }
            }
        }

        public bool IsBoneIndRefDefined
        {
            get
            {
                return (VertexType & (int)BinaryScxV4VertexDataFlag.BoneIndRef) != 0;
            }
            set
            {
                if (value)
                {
                    VertexType |= (int)BinaryScxV4VertexDataFlag.BoneIndRef;
                }
                else
                {
                    VertexType &= ~(int)BinaryScxV4VertexDataFlag.BoneIndRef;
                }
            }
        }

        public bool IsNormalDefined
        {
            get
            {
                return (VertexType & (int)BinaryScxV4VertexDataFlag.Normal) != 0;
            }
            set
            {
                if (value)
                {
                    VertexType |= (int)BinaryScxV4VertexDataFlag.Normal;
                }
                else
                {
                    VertexType &= ~(int)BinaryScxV4VertexDataFlag.Normal;
                }
            }
        }

        public bool IsVertexIlluminationDefined
        {
            get
            {
                return (VertexType & (int)BinaryScxV4VertexDataFlag.VertexIllumination) != 0;
            }
            set
            {
                if (value)
                {
                    VertexType |= (int)BinaryScxV4VertexDataFlag.VertexIllumination;
                }
                else
                {
                    VertexType &= ~(int)BinaryScxV4VertexDataFlag.VertexIllumination;
                }
            }
        }

        public bool IsVertexColorDefined
        {
            get
            {
                return (VertexType & (int)BinaryScxV4VertexDataFlag.VertexColor) != 0;
            }
            set
            {
                if (value)
                {
                    VertexType |= (int)BinaryScxV4VertexDataFlag.VertexColor;
                }
                else
                {
                    VertexType &= ~(int)BinaryScxV4VertexDataFlag.VertexColor;
                }
            }
        }

        public bool IsUV1Defined
        {
            get
            {
                return (VertexType & (int)BinaryScxV4VertexDataFlag.UV1) != 0;
            }
            set
            {
                if (value)
                {
                    VertexType |= (int)BinaryScxV4VertexDataFlag.UV1;
                }
                else
                {
                    VertexType &= ~(int)BinaryScxV4VertexDataFlag.UV1;
                }
            }
        }

        public bool IsUV2Defined
        {
            get
            {
                return (VertexType & (int)BinaryScxV4VertexDataFlag.UV2) != 0;
            }
            set
            {
                if (value)
                {
                    VertexType |= (int)BinaryScxV4VertexDataFlag.UV2;
                }
                else
                {
                    VertexType &= ~(int)BinaryScxV4VertexDataFlag.UV2;
                }
            }
        }

        public bool IsUV3Defined
        {
            get
            {
                return (VertexType & (int)BinaryScxV4VertexDataFlag.UV3) != 0;
            }
            set
            {
                if (value)
                {
                    VertexType |= (int)BinaryScxV4VertexDataFlag.UV3;
                }
                else
                {
                    VertexType &= ~(int)BinaryScxV4VertexDataFlag.UV3;
                }
            }
        }

        public bool IsBumpMapNormalDefined
        {
            get
            {
                return (VertexType & (int)BinaryScxV4VertexDataFlag.BumpMapNormal) != 0;
            }
            set
            {
                if (value)
                {
                    VertexType |= (int)BinaryScxV4VertexDataFlag.BumpMapNormal;
                }
                else
                {
                    VertexType &= ~(int)BinaryScxV4VertexDataFlag.BumpMapNormal;
                }
            }
        }

        public DynamicFaceDefV4 CorrespondingFaceDef
        {
            get;
            set;
        }

        public DynamicMaterialV4 CorrespondingMaterial
        {
            get;
            set;
        }

        public List<DynamicCompleteVertexDataV4> VertexDataList
        {
            get;
            set;
        } = new List<DynamicCompleteVertexDataV4>();

        public int Size
        {
            get
            {
                return 16 + (VertexDataList.Count * OneVertexSize);
            }
        }

        public int VertexType
        {
            get;
            set;
        }

        public int OneVertexSize
        {
            get
            {
                return getVertexSizeFromType();
            }
        }

        public DynamicVertexV4(BinaryVertexV4 constructFrom = null)
        {
            if (constructFrom == null)
                return;
            VertexType = constructFrom.VertexType;
            foreach (var vert in constructFrom.FullVertexDataList)
            {
                VertexDataList.Add(new DynamicCompleteVertexDataV4(vert, constructFrom));
            }

            if (constructFrom.Size != Size)
                throw new Exception("HeaderWill Mismatch");
        }

        public void VerticesSizeBound(int maxCount = ushort.MaxValue)
        {
            if (VertexDataList.Count > maxCount)
            {
                CorrespondingFaceDef.FixNumberOfIndices();
                VertexDataList.RemoveRange(maxCount, VertexDataList.Count - maxCount);
                for (int tri_i = 0; tri_i + 2 < CorrespondingFaceDef.Indices.Count; tri_i += 3)
                {
                    if (CorrespondingFaceDef.Indices[tri_i] > maxCount ||
                        CorrespondingFaceDef.Indices[tri_i + 1] > maxCount ||
                        CorrespondingFaceDef.Indices[tri_i + 2] > maxCount)
                    {
                        CorrespondingFaceDef.Indices.RemoveAt(tri_i);
                        CorrespondingFaceDef.Indices.RemoveAt(tri_i);
                        CorrespondingFaceDef.Indices.RemoveAt(tri_i);
                        tri_i -= 3;
                    }
                }
            }
        }

        public void RemoveOverflownTriangles()
        {
            int maxCount = VertexDataList.Count;
            {
                CorrespondingFaceDef.FixNumberOfIndices();
                for (int tri_i = 0; tri_i + 2 < CorrespondingFaceDef.Indices.Count; tri_i += 3)
                {
                    if (CorrespondingFaceDef.Indices[tri_i] >= maxCount ||
                        CorrespondingFaceDef.Indices[tri_i + 1] >= maxCount ||
                        CorrespondingFaceDef.Indices[tri_i + 2] >= maxCount)
                    {
                        CorrespondingFaceDef.Indices.RemoveAt(tri_i);
                        CorrespondingFaceDef.Indices.RemoveAt(tri_i);
                        CorrespondingFaceDef.Indices.RemoveAt(tri_i);
                        tri_i -= 3;
                    }
                }
            }
        }

        public int SetToAllOrVertexFlags()
        {
            int ret = 0;
            foreach (var vert in VertexDataList)
            {
                vert.SetBoneWeightDefinedsFromBoneWeights();
                ret |= vert.VertexType;
            }
            foreach (var vert in VertexDataList)
            {
                vert.VertexType = ret;
            }
            return ret;
        }

        public void Save(BinaryWriter bw)
        {
            bw.Write(TypeDefer);
            bw.Write(Size);
            bw.Write((int)VertexDataList.Count);
            bw.Write(VertexType);
            foreach (var vert in VertexDataList)
            {
                vert.Save(bw);
            }
        }

        private int getVertexSizeFromType()
        {
            VertexType = SetToAllOrVertexFlags();
            int ret = 0;
            ret += BinaryVertexV4.FlagToSize[(BinaryScxV4VertexDataFlag)(VertexType & (int)BinaryScxV4VertexDataFlag.Position)];
            ret += BinaryVertexV4.FlagToSize[(BinaryScxV4VertexDataFlag)(VertexType & (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs0)];
            ret += BinaryVertexV4.FlagToSize[(BinaryScxV4VertexDataFlag)(VertexType & (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs1)];
            ret += BinaryVertexV4.FlagToSize[(BinaryScxV4VertexDataFlag)(VertexType & (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs2)];
            ret += BinaryVertexV4.FlagToSize[(BinaryScxV4VertexDataFlag)(VertexType & (int)BinaryScxV4VertexDataFlag.BoneWeightNumIs3)];
            ret += BinaryVertexV4.FlagToSize[(BinaryScxV4VertexDataFlag)(VertexType & (int)BinaryScxV4VertexDataFlag.BoneIndRef)];
            ret += BinaryVertexV4.FlagToSize[(BinaryScxV4VertexDataFlag)(VertexType & (int)BinaryScxV4VertexDataFlag.Normal)];
            ret += BinaryVertexV4.FlagToSize[(BinaryScxV4VertexDataFlag)(VertexType & (int)BinaryScxV4VertexDataFlag.VertexIllumination)];
            ret += BinaryVertexV4.FlagToSize[(BinaryScxV4VertexDataFlag)(VertexType & (int)BinaryScxV4VertexDataFlag.VertexColor)];
            ret += BinaryVertexV4.FlagToSize[(BinaryScxV4VertexDataFlag)(VertexType & (int)BinaryScxV4VertexDataFlag.UV1)];
            ret += BinaryVertexV4.FlagToSize[(BinaryScxV4VertexDataFlag)(VertexType & (int)BinaryScxV4VertexDataFlag.UV2)];
            ret += BinaryVertexV4.FlagToSize[(BinaryScxV4VertexDataFlag)(VertexType & (int)BinaryScxV4VertexDataFlag.UV3)];
            ret += BinaryVertexV4.FlagToSize[(BinaryScxV4VertexDataFlag)(VertexType & (int)BinaryScxV4VertexDataFlag.BumpMapNormal)];
            return ret;
        }
    }
}