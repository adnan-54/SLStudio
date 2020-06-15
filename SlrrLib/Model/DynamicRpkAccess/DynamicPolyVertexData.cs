using System.IO;

namespace SlrrLib.Model
{
    public class DynamicPolyVertexData : DynamicRSDInnerEntryBase
    {
        public float VertexCoordX
        {
            get;
            set;
        }

        public float VertexCoordY
        {
            get;
            set;
        }

        public float VertexCoordZ
        {
            get;
            set;
        }

        public float VertexNormalX
        {
            get;
            set;
        }

        public float VertexNormalY
        {
            get;
            set;
        }

        public float VertexNormalZ
        {
            get;
            set;
        }

        public float UVChannel1X
        {
            get;
            set;
        }

        public float UVChannel1Y
        {
            get;
            set;
        }

        public float UVChannel2X
        {
            get;
            set;
        }

        public float UVChannel2Y
        {
            get;
            set;
        }

        public float UVChannel3X
        {
            get;
            set;
        }

        public float UVChannel3Y
        {
            get;
            set;
        }

        public byte VertexColorR
        {
            get;
            set;
        }

        public byte VertexColorG
        {
            get;
            set;
        }

        public byte VertexColorB
        {
            get;
            set;
        }

        public byte VertexColorA
        {
            get;
            set;
        }

        public byte VertexIlluminationR
        {
            get;
            set;
        }

        public byte VertexIlluminationG
        {
            get;
            set;
        }

        public byte VertexIlluminationB
        {
            get;
            set;
        }

        public byte VertexIlluminationA
        {
            get;
            set;
        }

        public DynamicPolyVertexData()
        : this(null)
        {
        }

        public DynamicPolyVertexData(BinaryPolyVertexData from = null)
        {
            if (from == null)
                return;
            VertexCoordX = from.VertexCoordX;
            VertexCoordY = from.VertexCoordY;
            VertexCoordZ = from.VertexCoordZ;
            VertexNormalX = from.VertexNormalX;
            VertexNormalY = from.VertexNormalY;
            VertexNormalZ = from.VertexNormalZ;
            UVChannel1X = from.UVChannel1X;
            UVChannel1Y = from.UVChannel1Y;
            UVChannel2X = from.UVChannel2X;
            UVChannel2Y = from.UVChannel2Y;
            UVChannel3X = from.UVChannel3X;
            UVChannel3Y = from.UVChannel3Y;
            VertexColorR = from.VertexColorR;
            VertexColorG = from.VertexColorG;
            VertexColorB = from.VertexColorB;
            VertexColorA = from.VertexColorA;
            VertexIlluminationR = from.VertexIlluminationR;
            VertexIlluminationG = from.VertexIlluminationG;
            VertexIlluminationB = from.VertexIlluminationB;
            VertexIlluminationA = from.VertexIlluminationA;
        }

        public DynamicPolyVertexData Copy()
        {
            var ret = new DynamicPolyVertexData();
            ret.VertexCoordX = VertexCoordX;
            ret.VertexCoordY = VertexCoordY;
            ret.VertexCoordZ = VertexCoordZ;
            ret.VertexNormalX = VertexNormalX;
            ret.VertexNormalY = VertexNormalY;
            ret.VertexNormalZ = VertexNormalZ;
            ret.UVChannel1X = UVChannel1X;
            ret.UVChannel1Y = UVChannel1Y;
            ret.UVChannel2X = UVChannel2X;
            ret.UVChannel2Y = UVChannel2Y;
            ret.UVChannel3X = UVChannel3X;
            ret.UVChannel3Y = UVChannel3Y;
            ret.VertexColorR = VertexColorR;
            ret.VertexColorG = VertexColorG;
            ret.VertexColorB = VertexColorB;
            ret.VertexColorA = VertexColorA;
            ret.VertexIlluminationR = VertexIlluminationR;
            ret.VertexIlluminationG = VertexIlluminationG;
            ret.VertexIlluminationB = VertexIlluminationB;
            ret.VertexIlluminationA = VertexIlluminationA;
            return ret;
        }

        public override string ToString()
        {
            return VertexCoordX.ToString("F3") + "," + VertexCoordY.ToString("F3") + "," + VertexCoordZ.ToString("F3");
        }

        public override int GetSize()
        {
            return 56;
        }

        public override void Save(BinaryWriter bw)
        {
            bw.Write(VertexCoordX);
            bw.Write(VertexCoordY);
            bw.Write(VertexCoordZ);
            bw.Write(VertexNormalX);
            bw.Write(VertexNormalY);
            bw.Write(VertexNormalZ);
            bw.Write(VertexColorR);
            bw.Write(VertexColorG);
            bw.Write(VertexColorB);
            bw.Write(VertexColorA);
            bw.Write(VertexIlluminationR);
            bw.Write(VertexIlluminationG);
            bw.Write(VertexIlluminationB);
            bw.Write(VertexIlluminationA);
            bw.Write(UVChannel1X);
            bw.Write(UVChannel1Y);
            bw.Write(UVChannel2X);
            bw.Write(UVChannel2Y);
            bw.Write(UVChannel3X);
            bw.Write(UVChannel3Y);
        }
    }
}