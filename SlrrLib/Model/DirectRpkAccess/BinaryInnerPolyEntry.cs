using System.Collections.Generic;

namespace SlrrLib.Model
{
    public class BinaryInnerPolyEntry : FileEntry
    {
        protected static readonly int sigantureOffset = 0;
        protected static readonly int sizeOffest = 4;
        protected static readonly int unkownCount1Offset = 8;
        protected static readonly int unkownCount2Offset = 12;
        protected static readonly int meshListOffset = 16;

        private IEnumerable<BinaryInnerPolyMesh> meshes = null;

        public string Siganture
        {
            get
            {
                return GetFixLengthString(sigantureOffset, 4);
            }
            set
            {
                SetFixLengthString(value, 4, sigantureOffset);
            }
        }

        public int UnkownCount1
        {
            get
            {
                return GetIntFromFile(unkownCount1Offset);
            }
            set
            {
                SetIntInFile(value, unkownCount1Offset);
            }
        }

        public int MeshCount
        {
            get
            {
                return GetIntFromFile(unkownCount2Offset);
            }
            set
            {
                SetIntInFile(value, unkownCount2Offset);
            }
        }

        public IEnumerable<BinaryInnerPolyMesh> Meshes
        {
            get
            {
                if (meshes == null)
                    meshes = ReLoadMeshes();
                return meshes;
            }
            set
            {
                meshes = value;
            }
        }

        public override int Size
        {
            get
            {
                return GetIntFromFile(sizeOffest, true) + 8;
            }
            set
            {
                SetIntInFile(value - 8, sizeOffest);
            }
        }

        public BinaryInnerPolyEntry(FileCacheHolder file, int offset, bool cache = false)
        : base(file, offset, cache)
        {
        }

        public IEnumerable<BinaryInnerPolyMesh> ReLoadMeshes()
        {
            List<BinaryInnerPolyMesh> ret = new List<BinaryInnerPolyMesh>();
            int currentOffset = meshListOffset;
            for (int vert_i = 0; vert_i != MeshCount; ++vert_i)
            {
                var toad = new BinaryInnerPolyMesh(Cache, Offset + currentOffset, Cache.IsDataCached);
                ret.Add(toad);
                currentOffset += toad.Size;
            }
            return ret;
        }
    }
}