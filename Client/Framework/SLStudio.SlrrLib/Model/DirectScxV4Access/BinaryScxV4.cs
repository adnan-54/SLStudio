using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlrrLib.Model
{
    public class BinaryScxV4 : Scx
    {
        private int entriesCountRead;
        private List<BinaryScxV4HeaderEntry> entries = new List<BinaryScxV4HeaderEntry>();

        public IEnumerable<BinaryScxV4HeaderEntry> HeaderEntries
        {
            get
            {
                return entries;
            }
        }

        public IEnumerable<BinaryVertexV4> MeshDatas
        {
            get
            {
                try
                {
                    return entries.Where(x => x.EntryType == BinaryScxV4PrettyEntryType.VertexData).Select(x => x.CorrespondingFileEntry as BinaryVertexV4);
                }
                catch (Exception)
                {
                }
                return Enumerable.Empty<BinaryVertexV4>();
            }
        }

        public IEnumerable<BinaryFaceDefV4> FaceDefs
        {
            get
            {
                try
                {
                    return entries.Where(x => x.EntryType == BinaryScxV4PrettyEntryType.FaceIndices).Select(x => x.CorrespondingFileEntry as BinaryFaceDefV4);
                }
                catch (Exception)
                {
                }
                return Enumerable.Empty<BinaryFaceDefV4>();
            }
        }

        public IEnumerable<BinaryMaterialV4> Materials
        {
            get
            {
                try
                {
                    return entries.Where(x => x.EntryType == BinaryScxV4PrettyEntryType.MaterialDef).Select(x => x.CorrespondingFileEntry as BinaryMaterialV4);
                }
                catch (Exception)
                {
                }
                return Enumerable.Empty<BinaryMaterialV4>();
            }
        }

        public BinaryScxV4(string fnam, bool cache = false)
        : base(fnam)
        {
            ReadData(cache);
        }

        public void DumpDataAsCSV(string fnam)
        {
            int mesInd = 0;
            foreach (var mesh in MeshDatas)
            {
                string curnam = Path.GetDirectoryName(fnam) + "\\" + Path.GetFileNameWithoutExtension(fnam) + "_" + mesInd.ToString() + ".csv";
                curnam = curnam.Trim('\\');
                StringBuilder meshSb = new StringBuilder();
                foreach (var flag in BinaryVertexV4.FlagToSize.Keys)
                {
                    if (mesh.IsFalgDefined(flag))
                    {
                        meshSb.Append(Enum.GetName(typeof(BinaryScxV4VertexDataFlag), flag) + "; ");
                    }
                }
                meshSb.AppendLine();
                foreach (var vert in mesh.FullVertexDataList)
                {
                    foreach (var flag in BinaryVertexV4.FlagToSize.Keys)
                    {
                        if (mesh.IsFalgDefined(flag))
                        {
                            meshSb.Append(vert.GetStringByFlag(flag) + "; ");
                        }
                    }
                    meshSb.AppendLine();
                }
                File.WriteAllText(curnam, meshSb.ToString());
                mesInd++;
            }
        }

        public BinaryVertexV4 VertexDataOfFaceDef(BinaryFaceDefV4 fdef)
        {
            BinaryVertexV4 lastVertData = null;
            foreach (var entry in entries)
            {
                if (entry.EntryType == BinaryScxV4PrettyEntryType.VertexData)
                    lastVertData = entry.CorrespondingFileEntry as BinaryVertexV4;
                if (entry.CorrespondingFileEntry == fdef)
                    return lastVertData;
            }
            return null;
        }

        public void SaveAs(string fnamToWriteTo)
        {
            foreach (var entry in entries)
            {
                if (entry.CorrespondingFileEntry == null)
                    continue;
                entry.CorrespondingFileEntry.Cache.CacheData();
            }
            string fnam = fnamToWriteTo;
            int lengthOfFileHeader = 4 * 3;//INVO, version, number of headerEntries
            lengthOfFileHeader += entries.Count() * 8;//type, offset;
            int offsetCounter = lengthOfFileHeader;
            foreach (var entry in entries)
            {
                if (entry.CorrespondingFileEntry == null)
                    continue;
                if (entry.EntryOffset != offsetCounter)
                    MessageLog.AddError("Different write");
                entry.EntryOffset = offsetCounter;
                offsetCounter += entry.CorrespondingFileEntry.GetFileData().Length;
            }
            BinaryWriter br = new BinaryWriter(File.Create(fnam));
            br.Write(ASCIIEncoding.ASCII.GetBytes("INVO"));//INVO
            br.Write((int)4);//Version
            br.Write((int)entries.Count());//number of headerEntries
            foreach (var entry in entries)
            {
                br.Write((int)entry.RawentryType);
                br.Write((int)entry.EntryOffset);
            }
            foreach (var entry in entries)
            {
                if (entry.CorrespondingFileEntry == null)
                    continue;
                br.Write(entry.CorrespondingFileEntry.GetFileData());
            }
            br.Close();
        }

        public override void SetAllreflectionPercent(float percent)
        {
            foreach (var mat in entries
                     .Where(x => x.EntryType == BinaryScxV4PrettyEntryType.MaterialDef)
                     .Select(x => x.CorrespondingFileEntry as BinaryMaterialV4))
            {
                mat.SetReflectionStrength(percent);
            }
        }

        public override bool HasAnyReflection()
        {
            foreach (var mat in entries
                     .Where(x => x.EntryType == BinaryScxV4PrettyEntryType.MaterialDef)
                     .Select(x => x.CorrespondingFileEntry as BinaryMaterialV4))
            {
                if (mat.HasReflection())
                    return true;
            }
            return false;
        }

        public override void ReadData(bool readVertexData = false)
        {
            ProperlyReadData = true;
            var bytes = fileCache.GetFileData();
            int currentInd = 0;
            FileSignature = ASCIIEncoding.ASCII.GetString(bytes, currentInd, 4);
            currentInd += 4;
            readVersion = BitConverter.ToInt32(bytes, currentInd);
            currentInd += 4;
            entriesCountRead = 0;
            try
            {
                entriesCountRead = BitConverter.ToInt32(bytes, currentInd);
            }
            catch (Exception)
            {
                MessageLog.AddError("ERROR READING MESH(PROBABLY EMPTY) : " + fileCache.FileName);
                return;
            }
            currentInd += 4;
            if (entriesCountRead == 0)
                return;
            for (int i = 0; i != entriesCountRead; ++i)
            {
                BinaryScxV4HeaderEntry toad = new BinaryScxV4HeaderEntry(fileCache, readVertexData);
                toad.RawentryType = BitConverter.ToInt32(bytes, currentInd);
                currentInd += 4;
                toad.EntryOffset = BitConverter.ToInt32(bytes, currentInd);
                if (toad.EntryOffset >= bytes.Length)
                    ProperlyReadData = false;
                currentInd += 4;
                entries.Add(toad);
            }
        }

        public override string ToString()
        {
            return Path.GetFileNameWithoutExtension(fileCache.FileName);
        }

        private int sizeOfEntry(BinaryScxV4HeaderEntry entry)
        {
            for (int entry_i = 0; entry_i != entries.Count; ++entry_i)
            {
                if (entry == entries[entry_i])
                {
                    if (entry_i == entries.Count - 1)
                    {
                        return fileCache.GetFileData().Length - entries[entry_i].EntryOffset;
                    }
                    return entries[entry_i + 1].EntryOffset - entries[entry_i].EntryOffset;
                }
            }
            return -1;
        }
    }
}