using System;
using System.Collections.Generic;
using System.Linq;

namespace SlrrLib.Model
{
    public class BinaryMeshV3 : FileEntry
    {
        private static readonly int vertexCountOffset = 0;
        private static readonly int vertexOffset = 4;

        private BinaryMaterialV3 correspondingMaterial;

        public int VerticesSize
        {
            get
            {
                return VertexCount * correspondingMaterial.OneVertexSize;
            }
        }

        public int VertexCount
        {
            get
            {
                return GetIntFromFile(vertexCountOffset);
            }
            set
            {
                SetIntInFile(value, vertexCountOffset);
            }
        }

        public int FacesCount
        {
            get
            {
                return GetIntFromFile(faceCountOffset);
            }
            set
            {
                SetIntInFile(value, faceCountOffset);
            }
        }

        public IEnumerable<BinaryVertexV3> VertexDatas
        {
            get
            {
                return LazyLoadVertexDatas();
            }
        }

        public IEnumerable<int> VertexIndices
        {
            get
            {
                return LazyLoadVertexIndices();
            }
        }

        public override int Size
        {
            get
            {
                return 4 + (GetIntFromFile(vertexCountOffset, true) * correspondingMaterial.OneVertexSize) + 4 + (GetIntFromFile((GetIntFromFile(vertexCountOffset, true) * correspondingMaterial.OneVertexSize) + 4, true) * 3 * 4);
            }
            set
            {
                throw new Exception("Can not set size of ScxV3ModelData it should always be = 4 + verticesSize + 4 + faceSize");
            }
        }

        private int facesOffset
        {
            get
            {
                return VerticesSize + 4;
            }
        }

        private int faceCountOffset
        {
            get
            {
                return facesOffset;
            }
        }

        private int faceListOffset
        {
            get
            {
                return facesOffset + 4;
            }
        }

        public BinaryMeshV3(BinaryMaterialV3 correspondingMaterial, FileCacheHolder fileCache, int offset, bool cache = false)
        : base(fileCache, offset, cache)
        {
            this.correspondingMaterial = correspondingMaterial;
        }

        public BinaryVertexV3 GetNthVertexData(int ind)
        {
            int oneVertexSize = VerticesSize / VertexCount;
            return new BinaryVertexV3(this, Cache, Offset + vertexOffset + (ind * oneVertexSize));
        }

        public int GetNthVertexIndex(int ind)
        {
            return GetIntFromFile(faceListOffset + (ind * 4));
        }

        public IEnumerable<BinaryVertexV3> ReLoadVertexDatas()
        {
            List<BinaryVertexV3> ret = new List<BinaryVertexV3>();
            for (int def_i = 0; def_i != VertexCount; ++def_i)
            {
                ret.Add(GetNthVertexData(def_i));
            }
            return ret;
        }

        public IEnumerable<BinaryVertexV3> LazyLoadVertexDatas()
        {
            List<int> ret = new List<int>();
            for (int def_i = 0; def_i != VertexCount; ++def_i)
            {
                ret.Add(def_i);
            }
            return ret.Select(x => GetNthVertexData(x));
        }

        public IEnumerable<int> ReLoadVertexIndices()
        {
            List<int> ret = new List<int>();
            for (int def_i = 0; def_i != FacesCount * 3; ++def_i)
            {
                ret.Add(GetNthVertexIndex(def_i));
            }
            return ret;
        }

        public IEnumerable<int> LazyLoadVertexIndices()
        {
            List<int> ret = new List<int>();
            for (int def_i = 0; def_i != FacesCount * 3; ++def_i)
            {
                ret.Add(def_i);
            }
            return ret.Select(x => GetNthVertexIndex(x));
        }

        public BinaryMaterialV3 GetCorrespondingMaterial()
        {
            return correspondingMaterial;
        }
    }
}