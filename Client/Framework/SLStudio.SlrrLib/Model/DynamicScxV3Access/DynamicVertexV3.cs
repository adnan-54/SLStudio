using System.IO;

namespace SlrrLib.Model
{
    public class DynamicVertexV3
    {
        private float vertexCoordX;
        private float vertexCoordY;
        private float vertexCoordZ;
        private float vertexNormalX;
        private float vertexNormalY;
        private float vertexNormalZ;
        private float uVChannel1X;
        private float uVChannel1Y;
        private float uVChannel2X;
        private float uVChannel2Y;
        private byte vertexColorB;
        private byte vertexColorG;
        private byte vertexColorR;
        private byte vertexColorA;
        private int unkown1;
        private int unkown2;
        private float uVChannel3X;
        private float uVChannel3Y;
        private int unkown3;

        public DynamicMeshV3 CorrespondingModelData
        {
            get;
            set;
        }

        public bool IsVertexCoordXDefined
        {
            get;
            set;
        }

        public bool IsVertexCoordYDefined
        {
            get;
            set;
        }

        public bool IsVertexCoordZDefined
        {
            get;
            set;
        }

        public bool IsVertexNormalXDefined
        {
            get;
            set;
        }

        public bool IsVertexNormalYDefined
        {
            get;
            set;
        }

        public bool IsVertexNormalZDefined
        {
            get;
            set;
        }

        public bool IsUVChannel1XDefined
        {
            get;
            set;
        }

        public bool IsUVChannel1YDefined
        {
            get;
            set;
        }

        public bool IsUVChannel2XDefined
        {
            get;
            set;
        }

        public bool IsUVChannel2YDefined
        {
            get;
            set;
        }

        public bool IsVertexColorBDefined
        {
            get;
            set;
        }

        public bool IsVertexColorRDefined
        {
            get;
            set;
        }

        public bool IsVertexColorGDefined
        {
            get;
            set;
        }

        public bool IsVertexColorADefined
        {
            get;
            set;
        }

        public bool IsUnkown1Defined
        {
            get;
            set;
        }

        public bool IsUnkown2Defined
        {
            get;
            set;
        }

        public bool IsUVChannel3XDefined
        {
            get;
            set;
        }

        public bool IsUVChannel3YDefined
        {
            get;
            set;
        }

        public bool IsUnkown3Defined
        {
            get;
            set;
        }

        public float VertexCoordX
        {
            get
            {
                return vertexCoordX;
            }
            set
            {
                vertexCoordX = value;
                IsVertexCoordXDefined = true;
            }
        }

        public float VertexCoordY
        {
            get
            {
                return vertexCoordY;
            }
            set
            {
                vertexCoordY = value;
                IsVertexCoordYDefined = true;
            }
        }

        public float VertexCoordZ
        {
            get
            {
                return vertexCoordZ;
            }
            set
            {
                vertexCoordZ = value;
                IsVertexCoordZDefined = true;
            }
        }

        public float VertexNormalX
        {
            get
            {
                return vertexNormalX;
            }
            set
            {
                vertexNormalX = value;
                IsVertexNormalXDefined = true;
            }
        }

        public float VertexNormalY
        {
            get
            {
                return vertexNormalY;
            }
            set
            {
                vertexNormalY = value;
                IsVertexNormalYDefined = true;
            }
        }

        public float VertexNormalZ
        {
            get
            {
                return vertexNormalZ;
            }
            set
            {
                vertexNormalZ = value;
                IsVertexNormalZDefined = true;
            }
        }

        public float UVChannel1X
        {
            get
            {
                return uVChannel1X;
            }
            set
            {
                uVChannel1X = value;
                IsUVChannel1XDefined = true;
            }
        }

        public float UVChannel1Y
        {
            get
            {
                return uVChannel1Y;
            }
            set
            {
                uVChannel1Y = value;
                IsUVChannel1YDefined = true;
            }
        }

        public float UVChannel2X
        {
            get
            {
                return uVChannel2X;
            }
            set
            {
                uVChannel2X = value;
                IsUVChannel2XDefined = true;
            }
        }

        public float UVChannel2Y
        {
            get
            {
                return uVChannel2Y;
            }
            set
            {
                uVChannel2Y = value;
                IsUVChannel2YDefined = true;
            }
        }

        public byte VertexColorB
        {
            get
            {
                return vertexColorB;
            }
            set
            {
                vertexColorB = value;
                IsVertexColorBDefined = true;
            }
        }

        public byte VertexColorG
        {
            get
            {
                return vertexColorG;
            }
            set
            {
                vertexColorG = value;
                IsVertexColorGDefined = true;
            }
        }

        public byte VertexColorR
        {
            get
            {
                return vertexColorR;
            }
            set
            {
                vertexColorR = value;
                IsVertexColorRDefined = true;
            }
        }

        public byte VertexColorA
        {
            get
            {
                return vertexColorA;
            }
            set
            {
                vertexColorA = value;
                IsVertexColorADefined = true;
            }
        }

        public int Unkown1
        {
            get
            {
                return unkown1;
            }
            set
            {
                unkown1 = value;
                IsUnkown1Defined = true;
            }
        }

        public int Unkown2
        {
            get
            {
                return unkown2;
            }
            set
            {
                unkown2 = value;
                IsUnkown2Defined = true;
            }
        }

        public float UVChannel3X
        {
            get
            {
                return uVChannel3X;
            }
            set
            {
                uVChannel3X = value;
                IsUVChannel3XDefined = true;
            }
        }

        public float UVChannel3Y
        {
            get
            {
                return uVChannel3Y;
            }
            set
            {
                uVChannel3Y = value;
                IsUVChannel3YDefined = true;
            }
        }

        public int Unkown3
        {
            get
            {
                return unkown3;
            }
            set
            {
                unkown3 = value;
                IsUnkown3Defined = true;
            }
        }

        public DynamicVertexV3(DynamicMeshV3 correspondingModelData, BinaryVertexV3 constructFrom = null)
        {
            this.CorrespondingModelData = correspondingModelData;
            if (constructFrom == null)
                return;
            constructFrom.Cache.CacheData();
            IsVertexCoordXDefined = constructFrom.IsVertexCoordXDefined;
            IsVertexCoordYDefined = constructFrom.IsVertexCoordYDefined;
            IsVertexCoordZDefined = constructFrom.IsVertexCoordZDefined;
            IsVertexNormalXDefined = constructFrom.IsVertexNormalXDefined;
            IsVertexNormalYDefined = constructFrom.IsVertexNormalYDefined;
            IsVertexNormalZDefined = constructFrom.IsVertexNormalZDefined;
            IsUVChannel1XDefined = constructFrom.IsUVChannel1XDefined;
            IsUVChannel1YDefined = constructFrom.IsUVChannel1YDefined;
            IsUVChannel2XDefined = constructFrom.IsUVChannel2XDefined;
            IsUVChannel2YDefined = constructFrom.IsUVChannel2YDefined;
            IsVertexColorBDefined = constructFrom.IsVertexColorBDefined;
            IsVertexColorRDefined = constructFrom.IsVertexColorRDefined;
            IsVertexColorGDefined = constructFrom.IsVertexColorGDefined;
            IsVertexColorADefined = constructFrom.IsVertexColorADefined;
            IsUnkown1Defined = constructFrom.IsUnkown1Defined;
            IsUnkown2Defined = constructFrom.IsUnkown2Defined;
            IsUVChannel3XDefined = constructFrom.IsUVChannel3XDefined;
            IsUVChannel3YDefined = constructFrom.IsUVChannel3YDefined;
            IsUnkown3Defined = constructFrom.IsUnkown3Defined;

            if (IsVertexCoordXDefined)
                VertexCoordX = constructFrom.VertexCoordX;
            if (IsVertexCoordYDefined)
                VertexCoordY = constructFrom.VertexCoordY;
            if (IsVertexCoordZDefined)
                VertexCoordZ = constructFrom.VertexCoordZ;
            if (IsVertexNormalXDefined)
                VertexNormalX = constructFrom.VertexNormalX;
            if (IsVertexNormalYDefined)
                VertexNormalY = constructFrom.VertexNormalY;
            if (IsVertexNormalZDefined)
                VertexNormalZ = constructFrom.VertexNormalZ;
            if (IsUVChannel1XDefined)
                UVChannel1X = constructFrom.UVChannel1X;
            if (IsUVChannel1YDefined)
                UVChannel1Y = constructFrom.UVChannel1Y;
            if (IsUVChannel2XDefined)
                UVChannel2X = constructFrom.UVChannel2X;
            if (IsUVChannel2YDefined)
                UVChannel2Y = constructFrom.UVChannel2Y;
            if (IsVertexColorBDefined)
                VertexColorB = constructFrom.VertexColorB;
            if (IsVertexColorRDefined)
                VertexColorR = constructFrom.VertexColorR;
            if (IsVertexColorGDefined)
                VertexColorG = constructFrom.VertexColorG;
            if (IsVertexColorADefined)
                VertexColorA = constructFrom.VertexColorA;
            if (IsUnkown1Defined)
                Unkown1 = constructFrom.Unkown1;
            if (IsUnkown2Defined)
                Unkown2 = constructFrom.Unkown2;
            if (IsUVChannel3XDefined)
                UVChannel3X = constructFrom.UVChannel3X;
            if (IsUVChannel3YDefined)
                UVChannel3Y = constructFrom.UVChannel3Y;
            if (IsUnkown3Defined)
                Unkown3 = constructFrom.Unkown3;
        }

        public float PositionDisatnceSqrFrom(DynamicVertexV3 other)
        {
            float x = VertexCoordX - other.VertexCoordX;
            float y = VertexCoordY - other.VertexCoordY;
            float z = VertexCoordZ - other.VertexCoordZ;
            return (float)(x * x + y * y + z * z);
        }

        public void FixIsDefines()
        {
            if (IsUnkown3Defined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = IsVertexNormalXDefined = IsVertexNormalYDefined = IsVertexNormalZDefined = IsUVChannel1XDefined = IsUVChannel1YDefined = IsUVChannel2XDefined = IsUVChannel2YDefined = IsVertexColorBDefined = IsVertexColorRDefined = IsVertexColorGDefined = IsVertexColorADefined = IsUnkown1Defined = IsUnkown2Defined = IsUVChannel3XDefined = IsUVChannel3YDefined = true;
            if (IsUVChannel3YDefined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = IsVertexNormalXDefined = IsVertexNormalYDefined = IsVertexNormalZDefined = IsUVChannel1XDefined = IsUVChannel1YDefined = IsUVChannel2XDefined = IsUVChannel2YDefined = IsVertexColorBDefined = IsVertexColorRDefined = IsVertexColorGDefined = IsVertexColorADefined = IsUnkown1Defined = IsUnkown2Defined = IsUVChannel3XDefined = IsUVChannel3YDefined = true;
            if (IsUVChannel3XDefined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = IsVertexNormalXDefined = IsVertexNormalYDefined = IsVertexNormalZDefined = IsUVChannel1XDefined = IsUVChannel1YDefined = IsUVChannel2XDefined = IsUVChannel2YDefined = IsVertexColorBDefined = IsVertexColorRDefined = IsVertexColorGDefined = IsVertexColorADefined = IsUnkown1Defined = IsUnkown2Defined = IsUVChannel3XDefined = IsUVChannel3YDefined = true;
            if (IsUnkown2Defined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = IsVertexNormalXDefined = IsVertexNormalYDefined = IsVertexNormalZDefined = IsUVChannel1XDefined = IsUVChannel1YDefined = IsUVChannel2XDefined = IsUVChannel2YDefined = IsVertexColorBDefined = IsVertexColorRDefined = IsVertexColorGDefined = IsVertexColorADefined = IsUnkown1Defined = true;
            if (IsUnkown1Defined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = IsVertexNormalXDefined = IsVertexNormalYDefined = IsVertexNormalZDefined = IsUVChannel1XDefined = IsUVChannel1YDefined = IsUVChannel2XDefined = IsUVChannel2YDefined = IsVertexColorBDefined = IsVertexColorRDefined = IsVertexColorGDefined = IsVertexColorADefined = true;
            if (IsVertexColorADefined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = IsVertexNormalXDefined = IsVertexNormalYDefined = IsVertexNormalZDefined = IsUVChannel1XDefined = IsUVChannel1YDefined = IsUVChannel2XDefined = IsUVChannel2YDefined = IsVertexColorBDefined = IsVertexColorRDefined = IsVertexColorGDefined = IsVertexColorADefined = true;
            if (IsVertexColorRDefined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = IsVertexNormalXDefined = IsVertexNormalYDefined = IsVertexNormalZDefined = IsUVChannel1XDefined = IsUVChannel1YDefined = IsUVChannel2XDefined = IsUVChannel2YDefined = IsVertexColorBDefined = IsVertexColorRDefined = IsVertexColorGDefined = IsVertexColorADefined = true;
            if (IsVertexColorGDefined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = IsVertexNormalXDefined = IsVertexNormalYDefined = IsVertexNormalZDefined = IsUVChannel1XDefined = IsUVChannel1YDefined = IsUVChannel2XDefined = IsUVChannel2YDefined = IsVertexColorBDefined = IsVertexColorRDefined = IsVertexColorGDefined = IsVertexColorADefined = true;
            if (IsVertexColorBDefined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = IsVertexNormalXDefined = IsVertexNormalYDefined = IsVertexNormalZDefined = IsUVChannel1XDefined = IsUVChannel1YDefined = IsUVChannel2XDefined = IsUVChannel2YDefined = IsVertexColorBDefined = IsVertexColorRDefined = IsVertexColorGDefined = IsVertexColorADefined = true;
            if (IsUVChannel2YDefined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = IsVertexNormalXDefined = IsVertexNormalYDefined = IsVertexNormalZDefined = IsUVChannel1XDefined = IsUVChannel1YDefined = IsUVChannel2XDefined = IsUVChannel2YDefined = true;
            if (IsUVChannel2XDefined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = IsVertexNormalXDefined = IsVertexNormalYDefined = IsVertexNormalZDefined = IsUVChannel1XDefined = IsUVChannel1YDefined = IsUVChannel2XDefined = IsUVChannel2YDefined = true;
            if (IsUVChannel1YDefined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = IsVertexNormalXDefined = IsVertexNormalYDefined = IsVertexNormalZDefined = IsUVChannel1XDefined = IsUVChannel1YDefined = true;
            if (IsUVChannel1XDefined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = IsVertexNormalXDefined = IsVertexNormalYDefined = IsVertexNormalZDefined = IsUVChannel1XDefined = IsUVChannel1YDefined = true;
            if (IsVertexNormalZDefined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = IsVertexNormalXDefined = IsVertexNormalYDefined = IsVertexNormalZDefined = true;
            if (IsVertexNormalXDefined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = IsVertexNormalXDefined = IsVertexNormalYDefined = IsVertexNormalZDefined = true;
            if (IsVertexNormalYDefined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = IsVertexNormalXDefined = IsVertexNormalYDefined = IsVertexNormalZDefined = true;
            if (IsVertexCoordZDefined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = true;
            if (IsVertexCoordYDefined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = true;
            if (IsVertexCoordXDefined)
                IsVertexCoordXDefined = IsVertexCoordYDefined = IsVertexCoordZDefined = true;
        }

        public int GetVertexSize()
        {
            int sum = 0;
            if (IsVertexCoordXDefined)
                sum += sizeof(float);
            if (IsVertexCoordYDefined)
                sum += sizeof(float);
            if (IsVertexCoordZDefined)
                sum += sizeof(float);
            if (IsVertexNormalXDefined)
                sum += sizeof(float);
            if (IsVertexNormalYDefined)
                sum += sizeof(float);
            if (IsVertexNormalZDefined)
                sum += sizeof(float);
            if (IsUVChannel1XDefined)
                sum += sizeof(float);
            if (IsUVChannel1YDefined)
                sum += sizeof(float);
            if (IsUVChannel2XDefined)
                sum += sizeof(float);
            if (IsUVChannel2YDefined)
                sum += sizeof(float);
            if (IsVertexColorBDefined)
                sum += sizeof(byte);
            if (IsVertexColorRDefined)
                sum += sizeof(byte);
            if (IsVertexColorGDefined)
                sum += sizeof(byte);
            if (IsVertexColorADefined)
                sum += sizeof(byte);
            if (IsUnkown1Defined)
                sum += sizeof(int);
            if (IsUnkown2Defined)
                sum += sizeof(int);
            if (IsUVChannel3XDefined)
                sum += sizeof(float);
            if (IsUVChannel3YDefined)
                sum += sizeof(float);
            if (IsUnkown3Defined)
                sum += sizeof(int);
            return sum;
        }

        public void Save(BinaryWriter bw)
        {
            if (IsVertexCoordXDefined)
                bw.Write(VertexCoordX);
            if (IsVertexCoordYDefined)
                bw.Write(VertexCoordY);
            if (IsVertexCoordZDefined)
                bw.Write(VertexCoordZ);
            if (IsVertexNormalXDefined)
                bw.Write(VertexNormalX);
            if (IsVertexNormalYDefined)
                bw.Write(VertexNormalY);
            if (IsVertexNormalZDefined)
                bw.Write(VertexNormalZ);
            if (IsUVChannel1XDefined)
                bw.Write(UVChannel1X);
            if (IsUVChannel1YDefined)
                bw.Write(UVChannel1Y);
            if (IsUVChannel2XDefined)
                bw.Write(UVChannel2X);
            if (IsUVChannel2YDefined)
                bw.Write(UVChannel2Y);
            if (IsVertexColorBDefined)
                bw.Write(VertexColorB);
            if (IsVertexColorGDefined)
                bw.Write(VertexColorG);
            if (IsVertexColorRDefined)
                bw.Write(VertexColorR);
            if (IsVertexColorADefined)
                bw.Write(VertexColorA);
            if (IsUnkown1Defined)
                bw.Write(Unkown1);
            if (IsUnkown2Defined)
                bw.Write(Unkown2);
            if (IsUVChannel3XDefined)
                bw.Write(UVChannel3X);
            if (IsUVChannel3YDefined)
                bw.Write(UVChannel3Y);
            if (IsUnkown3Defined)
                bw.Write(Unkown3);
        }

        public override string ToString()
        {
            return VertexCoordX.ToString("F3") + "," + VertexCoordY.ToString("F3") + "," + VertexCoordZ.ToString("F3");
        }
    }
}