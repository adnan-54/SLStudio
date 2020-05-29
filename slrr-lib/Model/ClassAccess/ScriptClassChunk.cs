using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class ScriptClassChunk : FileEntry
  {
    protected static readonly int signatureOffset = 0;
    protected static readonly int sizeOffset = 4; //real size in file is size+8
    protected static readonly int dataOffset = 8;

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
    public override int Size
    {
      get
      {
        return GetIntFromFile(sizeOffset, true);
      }
      set
      {
        SetIntInFile(value, sizeOffset, true);
      }
    }
    public byte[] RawDataOfEntry
    {
      get
      {
        return GetFixLengthChunk(dataOffset, Size, true);
      }
    }

    public ScriptClassChunk(FileCacheHolder file, int offset, bool cache = false)
    : base(file, offset, cache)
    {
    }

    public virtual void Save(BinaryWriter bw)
    {
      Cache.CacheData();
      string realSignature = Signature;
      while (realSignature.Length != 4)
        realSignature += " ";
      realSignature = realSignature.Substring(0, 4);
      bw.Write(ASCIIEncoding.ASCII.GetBytes(realSignature));
      bw.Write(RawDataOfEntry.Length);
      bw.Write(RawDataOfEntry);
    }
  }
}
