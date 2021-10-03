using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlrrLib.Model
{
    public class DynamicInnerPolyEntry : DynamicRSDInnerEntryBase
    {
        public string Signature
        {
            get;
            set;
        }

        public int UnkownCount1
        {
            get;
            set;
        }

        public int OriginalOffset
        {
            get;
            set;
        }

        public List<DynamicInnerPolyMesh> Meshes
        {
            get;
            set;
        } = new List<DynamicInnerPolyMesh>();

        public DynamicInnerPolyEntry()
        : this(null)
        {
            Signature = "POLY";
        }

        public DynamicInnerPolyEntry(BinaryInnerPolyEntry from = null)
        {
            if (from == null)
                return;
            Signature = from.Siganture;
            UnkownCount1 = from.UnkownCount1;
            foreach (var mesh in from.Meshes)
            {
                Meshes.Add(new DynamicInnerPolyMesh(mesh));
            }
            OriginalOffset = from.Offset;
        }

        public override int GetSize()
        {
            return 16 + Meshes.Sum(x => x.GetSize());
        }

        public override void Save(BinaryWriter bw)
        {
            bw.Write(ASCIIEncoding.ASCII.GetBytes(Signature));
            bw.Write(GetSize() - 8);
            bw.Write(UnkownCount1);
            bw.Write((int)Meshes.Count);
            foreach (var mesh in Meshes)
            {
                mesh.Save(bw);
            }
        }
    }
}