using System.IO;

namespace SlrrLib.Model
{
    public class DynamicPhysVertex : DynamicRSDInnerEntryBase
    {
        public float VertexX
        {
            get;
            set;
        }

        public float VertexY
        {
            get;
            set;
        }

        public float VertexZ
        {
            get;
            set;
        }

        public DynamicPhysVertex()
        : this(null)
        {
        }

        public DynamicPhysVertex(BinaryPhysVertex from = null)
        {
            if (from == null)
                return;
            VertexX = from.VertexX;
            VertexY = from.VertexY;
            VertexZ = from.VertexZ;
        }

        public override int GetSize()
        {
            return 12;
        }

        public override void Save(BinaryWriter bw)
        {
            bw.Write(VertexX);
            bw.Write(VertexY);
            bw.Write(VertexZ);
        }
    }
}