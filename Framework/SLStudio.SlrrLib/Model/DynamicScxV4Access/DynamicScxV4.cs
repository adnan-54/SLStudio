using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlrrLib.Model
{
    public class DynamicScxV4
    {
        public static readonly string FileSignature = "INVO";
        public static readonly int Version = 4;

        public string OriginalFileName
        {
            get;
            private set;
        }

        public List<DynamicMeshV4> Meshes
        {
            get;
            set;
        } = new List<DynamicMeshV4>();

        public int EntriesCount
        {
            get
            {
                return Meshes.Sum(x => x.CountDefed);
            }
        }

        public DynamicScxV4(BinaryScxV4 constructFrom = null)
        {
            if (constructFrom == null)
                return;
            OriginalFileName = constructFrom.ToString();

            DynamicMeshV4 currentlyAdding = null;
            foreach (var entry in constructFrom.HeaderEntries)
            {
                if (currentlyAdding == null)
                    currentlyAdding = new DynamicMeshV4();
                if (entry.EntryType == BinaryScxV4PrettyEntryType.BoneIndexList || entry.EntryType == BinaryScxV4PrettyEntryType.SparseBoneIndexList)
                {
                    if (currentlyAdding.BoneList == null)
                        currentlyAdding.BoneList = new DynamicBoneListV4(entry.CorrespondingFileEntry as BinaryBoneListV4);
                    else
                    {
                        Meshes.Add(currentlyAdding);
                        currentlyAdding = new DynamicMeshV4();
                        currentlyAdding.BoneList = new DynamicBoneListV4(entry.CorrespondingFileEntry as BinaryBoneListV4);
                    }
                }
                else if (entry.EntryType == BinaryScxV4PrettyEntryType.FaceIndices)
                {
                    if (currentlyAdding.FaceDef == null)
                        currentlyAdding.FaceDef = new DynamicFaceDefV4(entry.CorrespondingFileEntry as BinaryFaceDefV4);
                    else
                    {
                        Meshes.Add(currentlyAdding);
                        currentlyAdding = new DynamicMeshV4();
                        currentlyAdding.FaceDef = new DynamicFaceDefV4(entry.CorrespondingFileEntry as BinaryFaceDefV4);
                    }
                }
                else if (entry.EntryType == BinaryScxV4PrettyEntryType.HardSurfaceVertexDefinition)
                {
                    if (currentlyAdding.HardSurfaceDef == null)
                        currentlyAdding.HardSurfaceDef = new DynamicHardSurfaceDefV4(entry.CorrespondingFileEntry as BinaryHardSurfaceDefV4);
                    else
                    {
                        Meshes.Add(currentlyAdding);
                        currentlyAdding = new DynamicMeshV4();
                        currentlyAdding.HardSurfaceDef = new DynamicHardSurfaceDefV4(entry.CorrespondingFileEntry as BinaryHardSurfaceDefV4);
                    }
                }
                else if (entry.EntryType == BinaryScxV4PrettyEntryType.MaterialDef)
                {
                    if (currentlyAdding.Material == null)
                        currentlyAdding.Material = new DynamicMaterialV4(entry.CorrespondingFileEntry as BinaryMaterialV4);
                    else
                    {
                        Meshes.Add(currentlyAdding);
                        currentlyAdding = new DynamicMeshV4();
                        currentlyAdding.Material = new DynamicMaterialV4(entry.CorrespondingFileEntry as BinaryMaterialV4);
                    }
                }
                else if (entry.EntryType == BinaryScxV4PrettyEntryType.VertexData)
                {
                    if (currentlyAdding.VertexData == null)
                        currentlyAdding.VertexData = new DynamicVertexV4(entry.CorrespondingFileEntry as BinaryVertexV4);
                    else
                    {
                        Meshes.Add(currentlyAdding);
                        currentlyAdding = new DynamicMeshV4();
                        currentlyAdding.VertexData = new DynamicVertexV4(entry.CorrespondingFileEntry as BinaryVertexV4);
                    }
                }
                else
                {
                    throw new Exception("Not recognised header entry");
                }
            }
            if (currentlyAdding != null)
            {
                Meshes.Add(currentlyAdding);
                currentlyAdding.VertexData.CorrespondingFaceDef = currentlyAdding.FaceDef;
                currentlyAdding.VertexData.CorrespondingMaterial = currentlyAdding.Material;
            }
        }

        public bool HasAnyReflection()
        {
            foreach (var m in Meshes)
            {
                if (m.Material.HasReflection())
                    return true;
            }
            return false;
        }

        public void SetAllreflectionPercent(float percent)
        {
            foreach (var m in Meshes)
            {
                m.Material.SetReflectionStrength(percent);
            }
        }

        public void SaveAs(string fnamToWriteTo, bool backUp = true, string bakSuffix = "_BAK_ScxConst_")
        {
            if (backUp && File.Exists(fnamToWriteTo))
            {
                int bakInd = 0;
                while (File.Exists(fnamToWriteTo + bakSuffix + bakInd.ToString()))
                    bakInd++;
                File.Copy(fnamToWriteTo, fnamToWriteTo + bakSuffix + bakInd.ToString());
            }
            BinaryWriter bw = new BinaryWriter(File.Create(fnamToWriteTo));
            bw.Write(ASCIIEncoding.ASCII.GetBytes(FileSignature));
            bw.Write(Version);
            bw.Write(EntriesCount);
            int dataOffset = 4 + 4 + 4 + (EntriesCount * 2 * 4);
            foreach (var mesh in Meshes)
            {
                if (mesh.Material != null)
                {
                    bw.Write(DynamicMaterialV4.TypeDefer);
                    bw.Write(dataOffset);
                    dataOffset += mesh.Material.Size;
                }
                if (mesh.BoneList != null)
                {
                    bw.Write(mesh.BoneList.Type);
                    bw.Write(dataOffset);
                    dataOffset += mesh.BoneList.Size;
                }
                if (mesh.HardSurfaceDef != null)
                {
                    bw.Write(DynamicHardSurfaceDefV4.TypeDefer);
                    bw.Write(dataOffset);
                    dataOffset += mesh.HardSurfaceDef.Size;
                }
                if (mesh.VertexData != null)
                {
                    bw.Write(DynamicVertexV4.TypeDefer);
                    bw.Write(dataOffset);
                    dataOffset += mesh.VertexData.Size;
                }
                if (mesh.FaceDef != null)
                {
                    bw.Write(DynamicFaceDefV4.TypeDefer);
                    bw.Write(dataOffset);
                    dataOffset += mesh.FaceDef.Size;
                }
            }
            dataOffset = 4 + 4 + 4 + (EntriesCount * 2 * 4);
            foreach (var mesh in Meshes)
            {
                if (dataOffset != bw.BaseStream.Position)
                    throw new Exception("HeaderOffsetMissmatch");
                mesh.Save(bw);
                dataOffset += mesh.Size;
            }
            bw.Close();
        }

        public override string ToString()
        {
            return OriginalFileName;
        }
    }
}