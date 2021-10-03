using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlrrLib.Model
{
    public class DynamicInnerPhysEntry : DynamicRSDInnerEntryBase
    {
        public int OriginalOffset
        {
            get;
            private set;
        }

        public string Signature
        {
            get;
            set;
        }

        public List<DynamicPhysFacingDescriptor> Indices
        {
            get;
            set;
        } = new List<DynamicPhysFacingDescriptor>();

        public List<DynamicPhysVertex> VertexData
        {
            get;
            set;
        } = new List<DynamicPhysVertex>();

        public DynamicInnerPhysEntry()
        : this(null)
        {
            Signature = "PHYS";
        }

        public DynamicInnerPhysEntry(BinaryInnerPhysEntry from = null)
        {
            if (from == null)
                return;
            Signature = from.Siganture;
            foreach (var fCh in from.FacingProperties)
                Indices.Add(new DynamicPhysFacingDescriptor(fCh));
            foreach (var sCh in from.Vetices)
                VertexData.Add(new DynamicPhysVertex(sCh));
            OriginalOffset = from.Offset;
        }

        public override int GetSize()
        {
            return 16 + Indices.Sum(x => x.GetSize()) + VertexData.Sum(x => x.GetSize());
        }

        public override void Save(BinaryWriter bw)
        {
            bw.Write(ASCIIEncoding.ASCII.GetBytes(Signature));
            bw.Write(GetSize() - 8);
            bw.Write((int)Indices.Count);
            foreach (var fCh in Indices)
                fCh.Save(bw);
            bw.Write((int)VertexData.Count);
            foreach (var sCh in VertexData)
                sCh.Save(bw);
        }
    }
}