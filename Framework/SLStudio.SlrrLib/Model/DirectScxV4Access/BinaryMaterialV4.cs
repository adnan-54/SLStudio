using System;
using System.Collections.Generic;
using System.Linq;

namespace SlrrLib.Model
{
    public class BinaryMaterialV4 : FileEntry
    {
        private static readonly int unkownZeroOffset = 0;
        private static readonly int sizeOffset = 4;
        private static readonly int flagsOffset = 8;
        private static readonly int unkownZero2Offset = 12;
        private static readonly int entriesCountOffset = 16;

        private List<BinaryMaterialV4Entry> directEntries = new List<BinaryMaterialV4Entry>();

        public int UnkownZero1
        {
            get
            {
                return GetIntFromFile(unkownZeroOffset);
            }
            set
            {
                SetIntInFile(value, unkownZeroOffset);
            }
        }

        public int Flags
        {
            get
            {
                return GetIntFromFile(flagsOffset);
            }
            set
            {
                SetIntInFile(value, flagsOffset);
            }
        }

        public int UnkownZero2
        {
            get
            {
                return GetIntFromFile(unkownZero2Offset);
            }
            set
            {
                SetIntInFile(value, unkownZero2Offset);
            }
        }

        public int EntriesCount
        {
            get
            {
                return GetIntFromFile(entriesCountOffset);
            }
            set
            {
                SetIntInFile(value, entriesCountOffset);
            }
        }

        public IEnumerable<BinaryMaterialV4Entry> Entries
        {
            get
            {
                return directEntries;
            }
        }

        public override int Size
        {
            get
            {
                return GetIntFromFile(sizeOffset, true);
            }
            set
            {
                SetIntInFile(value, sizeOffset);
            }
        }

        public BinaryMaterialV4(FileCacheHolder fileCache, int offset, bool cache = false)
        : base(fileCache, offset, cache)
        {
            CheckConstraint();
        }

        public bool CheckConstraint()
        {
            var materialBytes = Cache.GetFileData().Skip(Offset).Take(Size).ToArray();
            for (int mBytes_i = 20/*material header is 20*/; mBytes_i != materialBytes.Length;)//++ left out
            {
                BinaryMaterialV4Entry toad = BinaryMaterialV4Entry.ConstructMaterialEntry(Cache, Offset + mBytes_i, Cache.IsDataCached);
                mBytes_i += toad.Size;
                if (directEntries.LastOrDefault() != null && directEntries.Last() is BinaryMaterialV4MapDefinition && directEntries.Last().UnknownFlag == 3)
                {
                    (directEntries.Last() as BinaryMaterialV4MapDefinition).FloatWeight = toad;
                }
                directEntries.Add(toad);
                if (mBytes_i == Size && directEntries.Count == EntriesCount)
                    break;//proper end
                else if (mBytes_i >= Size)
                {
                    MessageLog.AddError("CheckConstraint - Inconsistency");
                    break;
                }
            }
            return true;
        }

        public void SetReflectionStrength(float percent)
        {
            float realPercent = Math.Min(Math.Max(0.0f, percent), 1.0f);
            foreach (var materialEntry in directEntries)
            {
                if (materialEntry.UnknownFlag == 3 && materialEntry.TypeSwapped == 6)
                {
                    if ((materialEntry as BinaryMaterialV4MapDefinition).FloatWeight != null)
                    {
                        (materialEntry as BinaryMaterialV4MapDefinition).FloatWeight.DataAsFloat = realPercent;
                    }
                }
            }
        }

        public bool HasReflection()
        {
            foreach (var materialEntry in directEntries)
            {
                if (materialEntry.UnknownFlag == 3 && materialEntry.TypeSwapped == 6)
                {
                    if ((materialEntry as BinaryMaterialV4MapDefinition).FloatWeight != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public float ReflectionStrength()
        {
            foreach (var materialEntry in directEntries)
            {
                if (materialEntry.UnknownFlag == 3 && materialEntry.TypeSwapped == 6)
                {
                    if ((materialEntry as BinaryMaterialV4MapDefinition).FloatWeight != null)
                    {
                        return (materialEntry as BinaryMaterialV4MapDefinition).FloatWeight.DataAsFloat;
                    }
                }
            }
            return 0;
        }

        public override string ToString()
        {
            string ret = "";
            foreach (var entry in directEntries)
            {
                if (entry.TypeSwapped == 8)
                    ret += entry.DataAsString.Trim();
            }
            if (ret == "")
            {
                foreach (var entry in directEntries)
                {
                    ret += entry.PrettyName + "|";
                }
            }
            return ret;
        }
    }
}