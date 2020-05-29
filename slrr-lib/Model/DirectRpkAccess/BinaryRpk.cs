using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlrrLib.Model
{
  public class BinaryRpk
  {
    private FileCacheHolder fileData;
    private IEnumerable<BinaryRpkExternalReference> externalReferences = null;
    private IEnumerable<BinaryResEntry> resEntries = null;

    public BinaryRpkHeader FileHeader
    {
      get;
    }
    public BinaryRpkEntriesHeader EntriesHeader
    {
      get;
    }
    public IEnumerable<BinaryRpkExternalReference> ExternalReferences
    {
      get
      {
        if (externalReferences == null)
          externalReferences = ReLoadExternalReferences();
        return externalReferences;
      }
      set
      {
        externalReferences = value;
      }
    }
    public List<BinaryResEntry> RESEntries
    {
      get
      {
        if (resEntries == null)
          resEntries = ReLoadRESEntries();
        return resEntries.ToList();
      }
      set
      {
        resEntries = value;
      }
    }

    private int offsetExternalReferences
    {
      get
      {
        return FileHeader.Size;
      }
    }
    private int offsetRESs
    {
      get
      {
        return FileHeader.Size + (FileHeader.ExternalReferencesCount * 64)+EntriesHeader.Size;
      }
    }

    public BinaryRpk(string fnam,bool cache = false)
    {
      fileData = new FileCacheHolder(fnam,cache);
      FileHeader = new BinaryRpkHeader(fileData);
      EntriesHeader = new BinaryRpkEntriesHeader(fileData, FileHeader.Size + (FileHeader.ExternalReferencesCount*64));
    }

    public BinaryRpkExternalReference GetNthExternalReference(int nth)
    {
      return new BinaryRpkExternalReference(fileData, offsetExternalReferences + (nth * 64));
    }
    public IEnumerable<BinaryRpkExternalReference> ReLoadExternalReferences()
    {
      List<BinaryRpkExternalReference> ret = new List<BinaryRpkExternalReference>();
      for (int def_i = 0; def_i != FileHeader.ExternalReferencesCount; ++def_i)
      {
        ret.Add(GetNthExternalReference(def_i));
      }
      return ret;
    }
    public IEnumerable<BinaryRpkExternalReference> LazyExternalReferences()
    {
      List<int> ret = new List<int>();
      for (int def_i = 0; def_i != FileHeader.ExternalReferencesCount; ++def_i)
      {
        ret.Add(def_i);
      }
      return ret.Select(x => GetNthExternalReference(x));
    }
    public IEnumerable<BinaryResEntry> ReLoadRESEntries()
    {
      List<BinaryResEntry> ret = new List<BinaryResEntry>();
      int curOffset = offsetRESs;
      BinaryResEntry entryBefore = null;
      for (int def_i = 0; def_i != EntriesHeader.EntriesCount; ++def_i)
      {
        BinaryResEntry toad = new BinaryResEntry(fileData, curOffset);
        if(entryBefore != null)
        {
          entryBefore.NextResEntriesRSDOffset = toad.FileOffsetOfRSD;
        }
        entryBefore = toad;
        curOffset += toad.Size;
        ret.Add(toad);
      }
      return ret;
    }
    public BinaryResEntry GetResEntry(int LocalTypeID)
    {
      return RESEntries.FirstOrDefault(x => x.TypeID == LocalTypeID);
    }
    public string GetFileName()
    {
      return fileData.FileName;
    }
    public void Save()
    {
      fileData.SaveCachedData();
    }
  }
}
