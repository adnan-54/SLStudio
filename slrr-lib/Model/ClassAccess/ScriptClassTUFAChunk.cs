using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class ScriptClassTUFAChunk : FileEntry
  {
    protected static readonly int signatureOffset = 0;
    protected static readonly int unkint1Offset = 4;
    protected static readonly int unkint2Offset = 8;
    protected static readonly int entriesOffset = 12;

    private IEnumerable<ScriptClassChunk> entries_cache = null;

    public string Signature
    {
      get
      {
        return GetFixLengthString(signatureOffset, 4, true);
      }
      set
      {
        SetFixLengthString(value, 4, signatureOffset, true);
      }
    }
    public int UnkownData1
    {
      get
      {
        return GetIntFromFile(unkint1Offset, true);
      }
      set
      {
        SetIntInFile(value, unkint1Offset, true);
      }
    }
    public int UnkownData2
    {
      get
      {
        return GetIntFromFile(unkint2Offset, true);
      }
      set
      {
        SetIntInFile(value, unkint2Offset, true);
      }
    }
    public IEnumerable<ScriptClassChunk> Entries
    {
      get
      {
        if (entries_cache == null)
          entries_cache = ReLoadEntries();
        return entries_cache;
      }
    }

    public ScriptClassTUFAChunk(FileCacheHolder file, int offset, bool cache = false)
    : base(file, offset, cache)
    {
    }

    public IEnumerable<ScriptClassChunk> ReLoadEntries()
    {
      List<ScriptClassChunk> ret = new List<ScriptClassChunk>();
      int curOffset = entriesOffset;
      int fileLength = Cache.GetFileDataLength();
      while (curOffset + Offset < fileLength && GetFixLengthString(curOffset, 4, true) != "TUFA")
      {
        string curSignature = GetFixLengthString(curOffset, 4, true);
        if(curSignature == "CONS")
        {
          var toad = new ScriptClassCONSChunk(Cache, curOffset + Offset);
          ret.Add(toad);
        }
        else if(curSignature == "MTHD")
        {
          ret.Add(new ScriptClassIntCollectionChunk(Cache, curOffset + Offset));
        }
        else if (curSignature == "CLSS")
        {
          ret.Add(new ScriptClassIntCollectionChunk(Cache, curOffset + Offset));
        }
        else if (curSignature == "TREE")
        {
          ret.Add(new ScriptClassChunk(Cache, curOffset + Offset));
        }
        else if (curSignature == "FILD")
        {
          ret.Add(new ScriptClassIntCollectionChunk(Cache, curOffset + Offset));
        }
        else
        {
          throw new Exception("Unkown entry in class: " + Cache.FileName);
        }
        curOffset += ret.Last().Size+8;
      }
      return ret;
    }
    public ScriptClassChunk GetWithSignature(string sign)
    {
      return Entries.FirstOrDefault(x => x.Signature.ToUpper() == sign.ToUpper());
    }
    public int GetSizeFromEntries()
    {
      return 4 + 4 + 4 + Entries.Sum(x => x.Size + 8);
    }
    public ScriptClassCONSChunk ConsEntry
    {
      get
      {
        return Entries.First(x => x is ScriptClassCONSChunk) as ScriptClassCONSChunk;
      }
    }
    public void Save(BinaryWriter bw)
    {
      Cache.CacheData();
      string realSignature = Signature;
      while (realSignature.Length != 4)
        realSignature += " ";
      realSignature = realSignature.Substring(0, 4);
      bw.Write(ASCIIEncoding.ASCII.GetBytes(realSignature));
      bw.Write(UnkownData1);
      bw.Write(UnkownData2);
      foreach(var entry in Entries)
      {
        entry.Save(bw);
      }
    }
  }
}
