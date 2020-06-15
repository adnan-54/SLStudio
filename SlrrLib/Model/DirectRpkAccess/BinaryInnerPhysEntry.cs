using System.Collections.Generic;

namespace SlrrLib.Model
{
    public class BinaryInnerPhysEntry : FileEntry
    {
        protected static readonly int sigantureOffset = 0;
        protected static readonly int sizeOffest = 4;
        protected static readonly int firstChunkCountOffset = 8;
        protected static readonly int firstChunkOffset = 12;

        private IEnumerable<BinaryPhysFacingDescriptor> firstChunk = null;
        private IEnumerable<BinaryPhysVertex> secondChunk = null;

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

        public int FacingPropertyCount
        {
            get
            {
                return GetIntFromFile(firstChunkCountOffset);
            }
            set
            {
                SetIntInFile(value, firstChunkCountOffset);
            }
        }

        public int VerticesCount
        {
            get
            {
                return GetIntFromFile(secondChunkCountOffset);
            }
            set
            {
                SetIntInFile(value, secondChunkCountOffset);
            }
        }

        public IEnumerable<BinaryPhysFacingDescriptor> FacingProperties
        {
            get
            {
                if (firstChunk == null)
                    firstChunk = ReLoadFirstChunk();
                return firstChunk;
            }
            set
            {
                firstChunk = value;
            }
        }

        public IEnumerable<BinaryPhysVertex> Vetices
        {
            get
            {
                if (secondChunk == null)
                    secondChunk = ReLoadSecondChunk();
                return secondChunk;
            }
            set
            {
                secondChunk = value;
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

        private int secondChunkCountOffset
        {
            get
            {
                return firstChunkOffset + GetIntFromFile(firstChunkCountOffset, true) * 28;
            }
        }

        private int secondChunkOffset
        {
            get
            {
                return secondChunkCountOffset + 4;
            }
        }

        public BinaryInnerPhysEntry(FileCacheHolder file, int offset, bool cache = false)
        : base(file, offset, cache)
        {
        }

        public IEnumerable<BinaryPhysFacingDescriptor> ReLoadFirstChunk()
        {
            List<BinaryPhysFacingDescriptor> ret = new List<BinaryPhysFacingDescriptor>();
            int currentOffset = firstChunkOffset;
            for (int vert_i = 0; vert_i != FacingPropertyCount; ++vert_i)
            {
                var toad = new BinaryPhysFacingDescriptor(Cache, Offset + currentOffset, Cache.IsDataCached);
                ret.Add(toad);
                currentOffset += toad.Size;
            }
            return ret;
        }

        public IEnumerable<BinaryPhysVertex> ReLoadSecondChunk()
        {
            List<BinaryPhysVertex> ret = new List<BinaryPhysVertex>();
            int currentOffset = secondChunkOffset;
            for (int vert_i = 0; vert_i != VerticesCount; ++vert_i)
            {
                var toad = new BinaryPhysVertex(Cache, Offset + currentOffset, Cache.IsDataCached);
                ret.Add(toad);
                currentOffset += toad.Size;
            }
            return ret;
        }
    }
}