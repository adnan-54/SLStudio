using System.Collections.Generic;
using System.IO;

namespace SlrrLib.Model
{
    public class DynamicInnerPolyMesh : DynamicRSDInnerEntryBase
    {
        public List<DynamicPolyVertexData> Vertices
        {
            get;
            set;
        } = new List<DynamicPolyVertexData>();

        public List<int> Indices
        {
            get;
            set;
        } = new List<int>();

        public int TextureIndex
        {
            get;
            set;
        }

        public DynamicInnerPolyMesh()
        : this(null)
        {
        }

        public DynamicInnerPolyMesh(BinaryInnerPolyMesh from = null)
        {
            if (from == null)
                return;
            TextureIndex = from.TextureIndex;
            foreach (var vert in from.Vertices)
            {
                Vertices.Add(new DynamicPolyVertexData(vert));
            }
            foreach (var ind in from.Indices)
            {
                Indices.Add(ind);
            }
        }

        public override int GetSize()
        {
            fixIndices();
            return 12 + Vertices.Count * 56 + Indices.Count * 4;
        }

        public override void Save(BinaryWriter bw)
        {
            fixIndices();
            bw.Write(TextureIndex);
            bw.Write((int)Vertices.Count);
            bw.Write((int)Indices.Count / 3);
            foreach (var vert in Vertices)
                vert.Save(bw);
            foreach (var ind in Indices)
                bw.Write(ind);
        }

        private void fixIndices()
        {
            while (Indices.Count % 3 != 0)
                Indices.RemoveAt(Indices.Count - 1);
        }
    }
}