using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public enum BinaryScxV4PrettyEntryType
  {
    FaceIndices = 5,
    MaterialDef = 0,
    HardSurfaceVertexDefinition = 1,
    VertexData = 4,
    SparseBoneIndexList = 2,
    BoneIndexList = 3,
    Unkown = 99999
  }

  public class BinaryScxV4HeaderEntry
  {
    protected bool useCacheInFileEntry;
    protected FileCacheHolder fileCache;
    protected FileEntry fentry = null;

    public BinaryScxV4PrettyEntryType EntryType
    {
      get
      {
        if (Enum.IsDefined(typeof(BinaryScxV4PrettyEntryType), RawentryType))
        {
          return (BinaryScxV4PrettyEntryType)RawentryType;
        }
        return BinaryScxV4PrettyEntryType.Unkown;
      }
    }
    public FileEntry CorrespondingFileEntry
    {
      get
      {
        if(fentry == null)
        {
          switch (EntryType)
          {
            case BinaryScxV4PrettyEntryType.FaceIndices:
              fentry = new BinaryFaceDefV4(EntryOffset, fileCache, useCacheInFileEntry);
              break;
            case BinaryScxV4PrettyEntryType.MaterialDef:
              fentry = new BinaryMaterialV4(fileCache, EntryOffset, useCacheInFileEntry);
              break;
            case BinaryScxV4PrettyEntryType.HardSurfaceVertexDefinition:
              fentry = new BinaryHardSurfaceDefV4(fileCache, EntryOffset, useCacheInFileEntry);
              break;
            case BinaryScxV4PrettyEntryType.VertexData:
              fentry = new BinaryVertexV4(EntryOffset, fileCache, useCacheInFileEntry);
              break;
            case BinaryScxV4PrettyEntryType.SparseBoneIndexList:
            case BinaryScxV4PrettyEntryType.BoneIndexList:
              fentry = new BinaryBoneListV4(fileCache, EntryOffset, useCacheInFileEntry);
              break;
            case BinaryScxV4PrettyEntryType.Unkown:
            default:
              fentry = null;
              break;
          }
        }
        return fentry;
      }
    }
    public int EntryOffset
    {
      get;
      set;
    }
    public int RawentryType
    {
      get;
      set;
    }

    public BinaryScxV4HeaderEntry(FileCacheHolder fileCache, bool useCache = false)
    {
      this.fileCache = fileCache;
      useCacheInFileEntry = useCache;
    }
    public override string ToString()
    {
      return Enum.GetName(typeof(BinaryScxV4PrettyEntryType), EntryType) + "(" + RawentryType.ToString() + ")";
    }
  }
}
