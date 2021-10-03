using System;
using System.Collections.Generic;
using System.Linq;

namespace SlrrLib.Model
{
    public class BinaryInnerPolyMesh : FileEntry
    {
        protected static readonly int unkownCount3Offset = 0;
        protected static readonly int verticesCountOffset = 4;
        protected static readonly int triCountOffset = 8;
        protected static readonly int verticesOffset = 12;

        private IEnumerable<BinaryPolyVertexData> vertices = null;
        private IEnumerable<int> indices = null;

        public int TextureIndex
        {
            get
            {
                return GetIntFromFile(unkownCount3Offset);
            }
            set
            {
                SetIntInFile(value, unkownCount3Offset);
            }
        }

        public int VerticesCount
        {
            get
            {
                return GetIntFromFile(verticesCountOffset, true);
            }
            set
            {
                SetIntInFile(value, verticesCountOffset);
            }
        }

        public int TriCount
        {
            get
            {
                return GetIntFromFile(triCountOffset, true);
            }
            set
            {
                SetIntInFile(value, triCountOffset);
            }
        }

        public IEnumerable<BinaryPolyVertexData> Vertices
        {
            get
            {
                if (vertices == null)
                    vertices = ReLoadVertices();
                return vertices;
            }
            set
            {
                vertices = value;
            }
        }

        public IEnumerable<int> Indices
        {
            get
            {
                if (indices == null)
                    indices = ReLoadIndices();
                return indices;
            }
            set
            {
                int currentOffset = trisOffset;
                for (int ind_i = 0; ind_i != TriCount * 3; ++ind_i)
                {
                    SetIntInFile(value.ElementAt(ind_i), currentOffset);
                    currentOffset += 4;
                }
                indices = value;
            }
        }

        public override int Size
        {
            get
            {
                return 12 + VerticesCount * 56 + TriCount * 3 * 4;
            }
            set
            {
                throw new Exception("Mesh size directly can not be set Size = 12 + VerticesCount*56 + TriCount*3*4");
            }
        }

        private int trisOffset
        {
            get
            {
                return verticesOffset + VerticesCount * 56;
            }
        }

        public BinaryInnerPolyMesh(FileCacheHolder file, int offset, bool cache = false)
        : base(file, offset, cache)
        {
        }

        public IEnumerable<BinaryPolyVertexData> ReLoadVertices()
        {
            List<BinaryPolyVertexData> ret = new List<BinaryPolyVertexData>();
            int currentOffset = verticesOffset;
            for (int vert_i = 0; vert_i != VerticesCount; ++vert_i)
            {
                var toad = new BinaryPolyVertexData(Cache, Offset + currentOffset, Cache.IsDataCached);
                ret.Add(toad);
                currentOffset += toad.Size;
            }
            return ret;
        }

        public IEnumerable<int> ReLoadIndices()
        {
            List<int> ret = new List<int>();
            int currentOffset = trisOffset;
            for (int ind_i = 0; ind_i != TriCount * 3; ++ind_i)
            {
                ret.Add(GetIntFromFile(currentOffset));
                currentOffset += 4;
            }
            return ret;
        }
    }
}