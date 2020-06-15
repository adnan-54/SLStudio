using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlrrLib.Model
{
    public class ScriptClassCONSChunk : ScriptClassChunk
    {
        protected static readonly int countOffset = 8;
        protected static readonly int entriesOffset = 12;

        private List<ScriptClassCONSConstantEntry> constants_cache = null;

        public string ConstReadErrors
        {
            get;
            private set;
        } = "";

        public int CountOfEntries
        {
            get
            {
                return GetIntFromFile(countOffset, true);
            }
            set
            {
                SetIntInFile(value, countOffset, true);
            }
        }

        public string FullClassName
        {
            get
            {
                return (Constants.First(x => x.ID == 4 && (x.DataAsIndexLookup.Length > 0 && x.DataAsIndexLookup[0] is ScriptClassCONSNameEntry && (x.DataAsIndexLookup[0] as ScriptClassCONSNameEntry).DataAsString != "")).DataAsIndexLookup[0] as ScriptClassCONSNameEntry).DataAsString;
            }
            set
            {
                var toSet = (Constants.First(x => x.ID == 4).DataAsIndexLookup[0] as ScriptClassCONSNameEntry);
                int indTowrite = Constants.First(x => x.ID == 4).DataAsInt[0];
                directConsts[indTowrite] = new ScriptClassCONSNameEntry(value, this);
            }
        }

        public string FullExtendsClassName
        {
            get
            {
                var only4s = Constants.Where(x => x.ID == 4).ToList();
                if (only4s.Count < 2)
                    return "";
                return (directConsts[only4s[1].DataAsInt[0]] as ScriptClassCONSNameEntry).DataAsString;
            }
            set
            {
                var only4s = Constants.Where(x => x.ID == 4).ToList();
                if (only4s.Count < 2)
                    return;
                int indTowrite = only4s[1].DataAsInt[0];
                directConsts[indTowrite] = new ScriptClassCONSNameEntry(value, this);
            }
        }

        public string PackageName
        {
            get
            {
                return FullClassName.Remove(FullClassName.LastIndexOf('.'));
            }
            set
            {
                string shortName = FullClassName.Substring(FullClassName.LastIndexOf('.') + 1);
                if (value.EndsWith("."))
                    FullClassName = value + shortName;
                else
                    FullClassName = value + "." + shortName;
            }
        }

        public string ShortClassName
        {
            get
            {
                return FullClassName.Substring(FullClassName.LastIndexOf('.') + 1);
            }
            set
            {
                FullClassName = PackageName + "." + value;
            }
        }

        public string ShortSuperClassName
        {
            get
            {
                return FullExtendsClassName.Substring(FullExtendsClassName.LastIndexOf('.') + 1);
            }
        }

        public IEnumerable<ScriptClassCONSRpkRefEntry> RpkRefs
        {
            get
            {
                return Constants.Where(x => x is ScriptClassCONSRpkRefEntry).Select(x => x as ScriptClassCONSRpkRefEntry);
            }
        }

        public IEnumerable<ScriptClassCONSConstantEntry> Constants
        {
            get
            {
                if (constants_cache == null)
                    constants_cache = loadConstants();
                return constants_cache;
            }
        }

        public override int Size// this will cahce the whole file....
        {
            get
            {
                return Constants.Sum(x => x.GetSizeInFile()) + 4;
            }
            set
            {
                throw new Exception("Size should be Constants.Sum(x => x.getSizeInFile())+4");
            }
        }

        private List<ScriptClassCONSConstantEntry> directConsts
        {
            get
            {
                if (constants_cache == null)
                    constants_cache = loadConstants();
                return constants_cache;
            }
        }

        public ScriptClassCONSChunk(FileCacheHolder file, int offset, bool cache = false)
        : base(file, offset, cache)
        {
        }

        public void AddConstantEntry(ScriptClassCONSConstantEntry toad)
        {
            if (constants_cache == null)
                constants_cache = loadConstants();
            constants_cache.Add(toad);
        }

        public void RemoveConstantEntry(ScriptClassCONSConstantEntry torem)
        {
            if (constants_cache == null)
                constants_cache = loadConstants();
            constants_cache.Remove(torem);
        }

        public void RemoveConstantEntryAt(int index)
        {
            if (constants_cache == null)
                constants_cache = loadConstants();
            constants_cache.RemoveAt(index);
        }

        public ScriptClassCONSConstantEntry CreateAndAddIntConstEntry(int value)
        {
            var tmpLst = BitConverter.GetBytes(4).ToList();//ID
            tmpLst.AddRange(BitConverter.GetBytes(value));
            FileCacheHolder independent = new FileCacheHolder(tmpLst.ToArray());
            ScriptClassCONSConstantEntry toad = new ScriptClassCONSConstantEntry(independent, 0, this, true);
            AddConstantEntry(toad);
            return toad;
        }

        public ScriptClassCONSConstantEntry CreateAndAddIntPairConstEntry(int ID, int value1, int value2)
        {
            if (ID != 7 && ID != 5 && ID != 3)
                throw new Exception("Bad ID for Int pair Const Entry");
            var tmpLst = BitConverter.GetBytes(ID).ToList();
            tmpLst.AddRange(BitConverter.GetBytes(value1));
            tmpLst.AddRange(BitConverter.GetBytes(value2));
            FileCacheHolder independent = new FileCacheHolder(tmpLst.ToArray());
            ScriptClassCONSConstantEntry toad = new ScriptClassCONSConstantEntry(independent, 0, this, true);
            AddConstantEntry(toad);
            return toad;
        }

        public ScriptClassCONSNameEntry CreateAndAddNameEntry(string data)
        {
            ScriptClassCONSNameEntry toad = new ScriptClassCONSNameEntry(data);
            AddConstantEntry(toad);
            return toad;
        }

        public ScriptClassCONSRpkRefEntry CreateAndAddRpkRefEntry(string slrrRootRelativeRpkName, int refID)
        {
            int rpkEntryIndex = 0;
            for (; rpkEntryIndex != directConsts.Count; ++rpkEntryIndex)
            {
                if (directConsts[rpkEntryIndex] is ScriptClassCONSNameEntry)
                {
                    var rpkRef = directConsts[rpkEntryIndex] as ScriptClassCONSNameEntry;
                    if (rpkRef.DataAsString.ToLower() == slrrRootRelativeRpkName.ToLower())
                    {
                        break;
                    }
                }
            }
            if (rpkEntryIndex == directConsts.Count)
            {
                CreateAndAddNameEntry(slrrRootRelativeRpkName);
            }
            var tmpLst = BitConverter.GetBytes(3).ToList();//ID
            tmpLst.AddRange(BitConverter.GetBytes(rpkEntryIndex));
            tmpLst.AddRange(BitConverter.GetBytes(refID));
            FileCacheHolder independent = new FileCacheHolder(tmpLst.ToArray());
            ScriptClassCONSRpkRefEntry toad = new ScriptClassCONSRpkRefEntry(independent, 0, this, true);
            AddConstantEntry(toad);
            return toad;
        }

        public int IndexOfStringEntryWithStr(string str)
        {
            for (int str_i = 0; str_i != directConsts.Count; ++str_i)
            {
                if (directConsts[str_i] is ScriptClassCONSNameEntry)
                {
                    var nmEntr = (directConsts[str_i] as ScriptClassCONSNameEntry);
                    if (nmEntr.DataAsString == str)
                        return str_i;
                }
            }
            return -1;
        }

        public ScriptClassCONSNameEntry GetUsedRPKNameContainingString(string rpk)
        {
            var corresondingRPKRef = Constants.FirstOrDefault(x => (x is ScriptClassCONSRpkRefEntry) && (x as ScriptClassCONSRpkRefEntry).RPKNameString.Contains(rpk));
            if (corresondingRPKRef == Constants.DefaultIfEmpty())
                return null;
            return (corresondingRPKRef as ScriptClassCONSRpkRefEntry).DataAsIndexLookup[0] as ScriptClassCONSNameEntry;
        }

        public void OverwriteEntry(int index, ScriptClassCONSConstantEntry entry)
        {
            directConsts[index] = entry;
        }

        public override void Save(BinaryWriter bw)
        {
            Cache.CacheData();
            string realSignature = Signature;
            while (realSignature.Length != 4)
                realSignature += " ";
            realSignature = realSignature.Substring(0, 4);
            bw.Write(ASCIIEncoding.ASCII.GetBytes(realSignature));
            bw.Write(Size);
            bw.Write((int)directConsts.Count);
            long curPos = bw.BaseStream.Position;
            foreach (var entry in Constants)
            {
                entry.Save(bw);
                long afterPos = bw.BaseStream.Position;
                if (afterPos - curPos != entry.GetSizeInFile())
                {
                    throw new Exception("Size mismatch saving class file");
                }
                curPos = bw.BaseStream.Position;
            }
        }

        private List<ScriptClassCONSConstantEntry> loadConstants()
        {
            ConstReadErrors = "";
            List<ScriptClassCONSConstantEntry> ret = new List<ScriptClassCONSConstantEntry>();
            Cache.CacheData();
            var bytes = Cache.GetFileData();
            int bytes_i = Offset + entriesOffset;
            int entriesCount = 0;
            while (bytes_i < bytes.Length && entriesCount < CountOfEntries)
            {
                int currentID = BitConverter.ToInt32(bytes, bytes_i);
                if (currentID == 1145590861 || //MTHD
                    currentID == 1145850182 || //FILD
                    currentID == 1397967939 || //CLSS
                    currentID == 1162170964 //TREE
                   )
                {
                    ConstReadErrors += (Cache.FileName + " | NON PROPER ENTRY COUNT IN FILE!! at index: " + bytes_i.ToString() + "\n");
                    break;
                }
                int currentEntryOffset = bytes_i;
                bytes_i += 4;//skip over ID
                bool heuristicFix_EngineProbablyCrashesOnThisEntry = false;
                if (currentID == 0)
                {
                    int stringSize = BitConverter.ToInt32(bytes, bytes_i);
                    int toSkipp = stringSize;
                    bytes_i += 4;
                    if (bytes[bytes_i + stringSize] == 0 && canIntFollowConstEntry(BitConverter.ToInt32(bytes, bytes_i + stringSize + 1))
                        /*&& bytes.Skip(bytes_i).Take(stringSize).All(x => CanByteBePartOfString(x))*/)
                    {
                        toSkipp++;
                        //DONT add the trailing zero
                        //stringSize++;
                    }
                    else
                    {
                        heuristicFix_EngineProbablyCrashesOnThisEntry = true;
                        stringSize = 0;
                        toSkipp = 0;
                        while (CanByteBePartOfString(bytes[bytes_i + stringSize]))
                        {
                            stringSize++;
                            toSkipp++;
                        }

                        if (bytes[bytes_i + stringSize] == 0)
                        {
                            toSkipp++;
                            //DONT add the trailing zero
                            //stringSize++;
                        }
                    }
                    ScriptClassCONSNameEntry toad = new ScriptClassCONSNameEntry(Cache, currentEntryOffset);
                    toad.StringLength = stringSize;
                    ret.Add(toad);
                    bytes_i += toSkipp;
                }
                else if (currentID == 4)
                {
                    var toad = new ScriptClassCONSConstantEntry(Cache, currentEntryOffset, this);
                    ret.Add(toad);
                    bytes_i += toad.GetDataSizeFromID();
                }
                else if (currentID == 7)
                {
                    var toad = new ScriptClassCONSConstantEntry(Cache, currentEntryOffset, this);
                    ret.Add(toad);
                    bytes_i += toad.GetDataSizeFromID();
                }
                else if (currentID == 5)
                {
                    var toad = new ScriptClassCONSConstantEntry(Cache, currentEntryOffset, this);
                    ret.Add(toad);
                    bytes_i += toad.GetDataSizeFromID();
                }
                else if (currentID == 3)
                {
                    var toad = new ScriptClassCONSRpkRefEntry(Cache, currentEntryOffset, this);
                    ret.Add(toad);
                    bytes_i += toad.GetDataSizeFromID();
                }
                else if (currentID == 2)
                {
                    var toad = new ScriptClassCONSConstantEntry(Cache, currentEntryOffset, this);
                    ret.Add(toad);
                    bytes_i += toad.GetDataSizeFromID();
                }
                else if (currentID == -1)
                {
                    MessageLog.AddError("-1 FOUND IN CLASS FILE AS CONS ENTRY ID SKIPPING....");
                    continue;
                }
                else
                {
                    ConstReadErrors += (Cache.FileName + " | UNKOWN CONST ENTRY at index: " + bytes_i.ToString() + "\n");
                    return ret;
                }
                if (heuristicFix_EngineProbablyCrashesOnThisEntry)
                {
                    ConstReadErrors += (Cache.FileName + " | HEURISTIC READ ENGINE COULD CRASH ON THIS! at index: " + bytes_i.ToString() + "\n");
                }
                entriesCount++;
            }
            if (ret.Count != CountOfEntries)
            {
                ConstReadErrors += (Cache.FileName + " | NON PROPER ENTRY COUNT IN FILE!!" + "\n");
                CountOfEntries = ret.Count;
            }
            int sumSize = ret.Sum(x => x.GetSizeInFile()) + 4;
            if (sumSize != GetIntFromFile(sizeOffset, true))
            {
                int nextHeader = BitConverter.ToInt32(bytes, bytes_i);
                if (nextHeader == 1145590861 || //MTHD
                    nextHeader == 1145850182 || //FILD
                    nextHeader == 1397967939 || //CLSS
                    nextHeader == 1162170964 //TREE
                   )
                {
                    SetIntInFile(sumSize, sizeOffset, true);
                }
                else
                {
                    ConstReadErrors += (Cache.FileName + " | LengthOfConstTable not right!!" + "\n");
                }
            }
            return ret;
        }

        private bool canIntFollowConstEntry(int theInt)
        {
            switch (theInt)
            {
                case 0:
                case 4:
                case 7:
                case 5:
                case 3:
                case 1145590861://MTHD
                case 1145850182://FILD
                case 1397967939://CLSS
                case 1162170964://TREE
                    return true;
            }
            return false;
        }

        private static bool CanByteBePartOfString(byte ch)
        {
            return char.IsLetterOrDigit((char)ch) || char.IsWhiteSpace((char)ch) ||
                   (char)ch == '.' ||
                   (char)ch == '_' ||
                   (char)ch == ';' ||
                   (char)ch == ':' ||
                   (char)ch == '<' ||
                   (char)ch == '>' ||
                   (char)ch == '(' ||
                   (char)ch == ')' ||
                   (char)ch == '[' ||
                   (char)ch == ']' ||
                   (char)ch == '-' ||
                   (char)ch == ',' ||
                   (char)ch == '\\' ||
                   (char)ch == '/' ||
                   (char)ch == '|' ||
                   (char)ch == '\'' ||
                   (char)ch == '\"' ||
                   (char)ch == '%' ||
                   (char)ch == '?' ||
                   (char)ch == '!' ||
                   (char)ch == '+' ||
                   (char)ch == '*' ||
                   (char)ch == '`' ||
                   (char)ch == '&' ||
                   (char)ch == '#' ||
                   (char)ch == '@' ||
                   (char)ch == '=' ||
                   (char)ch == '^' ||
                   (char)ch == '$' ||
                   ch == 150 ||
                   ch == 146 ||
                   ch == 151 ||
                   ch == 148;
        }
    }
}