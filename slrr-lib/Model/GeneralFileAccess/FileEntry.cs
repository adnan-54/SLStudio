using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class FileEntry
  {
    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError=true, CharSet=System.Runtime.InteropServices.CharSet.Auto)]
    private static extern uint GetLongPathName(string ShortPath, StringBuilder sb, int buffer);
    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern uint GetShortPathName(string longpath, StringBuilder sb, int buffer);
    public static string GetWindowsPhysicalPath(string path)
    {
      var ret = Directory.GetFiles(Path.GetDirectoryName(path), Path.GetFileName(path)).FirstOrDefault();
      if (ret == null)
        return path;
      return ret;
    }

    protected readonly bool disableWrite = false;

    public int Offset
    {
      get;
      set;
    } = -1;
    public virtual int Size
    {
      get;
      set;
    }
    public FileCacheHolder Cache
    {
      get;
      set;
    }

    public FileEntry(FileCacheHolder fileCache, int offset, bool cache = false)
    {
      this.Offset = offset;
      this.Cache = fileCache;
      if (cache)
        fileCache.CacheData();
    }

    public byte[] GetFileData()
    {
      return ReadBytesOrUseCahce();
    }

    protected byte[] ReadBytesOrUseCahce()
    {
      return Cache.GetFileData();
    }
    protected byte[] ReadBytesUseCahce()
    {
      Cache.CacheData();
      return Cache.GetFileData();
    }
    protected float GetFloatFromFile(int entryRelativeOffset, bool forceRetrive = false)
    {
      if (!forceRetrive && entryRelativeOffset + sizeof(float) > Size)
        return -1;
      return BitConverter.ToSingle(ReadBytesOrUseCahce(), Offset + entryRelativeOffset);
    }
    protected void SetFloatInFile(float value, int entryRelativeOffset, bool forceWrite = false)
    {
      if (disableWrite)
        throw new Exception("WRITING IS DISABLED");
      if (!forceWrite && entryRelativeOffset + sizeof(float) > Size)
        return;
      var bytes = Cache.GetFileDataUseCache(Offset+entryRelativeOffset+sizeof(float));
      Array.Copy(BitConverter.GetBytes(value), 0, bytes, Offset + entryRelativeOffset, sizeof(float));
      if (!Cache.IsDataCached)
        File.WriteAllBytes(Cache.FileName, bytes);
    }
    protected int GetIntFromFile(int entryRelativeOffset, bool forceRetrive = false)
    {
      if (!forceRetrive && entryRelativeOffset + sizeof(int) > Size)
        return -1;
      return BitConverter.ToInt32(ReadBytesOrUseCahce(), Offset + entryRelativeOffset);
    }
    protected void SetIntInFile(int value, int entryRelativeOffset, bool forceWrite = false)
    {
      if (disableWrite)
        throw new Exception("WRITING IS DISABLED");
      if (!forceWrite && entryRelativeOffset + sizeof(int) > Size)
        return;
      var bytes = Cache.GetFileDataUseCache(Offset+entryRelativeOffset+sizeof(int));
      Array.Copy(BitConverter.GetBytes(value), 0, bytes, Offset + entryRelativeOffset, sizeof(int));
      if (!Cache.IsDataCached)
        File.WriteAllBytes(Cache.FileName, bytes);
    }
    protected short GetShortFromFile(int entryRelativeOffset, bool forceRetrive = false)
    {
      if (!forceRetrive && entryRelativeOffset + sizeof(short) > Size)
        return -1;
      return BitConverter.ToInt16(ReadBytesOrUseCahce(), Offset + entryRelativeOffset);
    }
    protected void SetShortInFile(short value, int entryRelativeOffset, bool forceWrite = false)
    {
      if (disableWrite)
        throw new Exception("WRITING IS DISABLED");
      if (!forceWrite && entryRelativeOffset + sizeof(short) > Size)
        return;
      var bytes = Cache.GetFileDataUseCache(Offset + entryRelativeOffset + sizeof(short));
      Array.Copy(BitConverter.GetBytes(value), 0, bytes, Offset + entryRelativeOffset, sizeof(short));
      if (!Cache.IsDataCached)
        File.WriteAllBytes(Cache.FileName, bytes);
    }
    protected byte GetByteFromFile(int entryRelativeOffset, bool forceRetrive = false)
    {
      if (!forceRetrive && entryRelativeOffset > Size)
        return 255;
      return ReadBytesOrUseCahce()[Offset + entryRelativeOffset];
    }
    protected void SetByteInFile(byte value, int entryRelativeOffset, bool forceWrite = false)
    {
      if (disableWrite)
        throw new Exception("WRITING IS DISABLED");
      if (!forceWrite && entryRelativeOffset > Size)
        return;
      var bytes = Cache.GetFileDataUseCache(Offset + entryRelativeOffset + 1);
      bytes[Offset + entryRelativeOffset] = value;
      if (!Cache.IsDataCached)
        File.WriteAllBytes(Cache.FileName, bytes);
    }
    protected string GetFixLengthString(int entryRelativeOffset,int stringLength, bool forceRetrive = false)
    {
      if (!forceRetrive && entryRelativeOffset + stringLength > Size)
        return "";
      return ASCIIEncoding.ASCII.GetString(ReadBytesOrUseCahce(), Offset + entryRelativeOffset, stringLength);
    }
    protected void SetFixLengthString(string value, int stringLength, int entryRelativeOffset, bool forceWrite = false)
    {
      if (disableWrite)
        throw new Exception("WRITING IS DISABLED");
      if (!forceWrite && entryRelativeOffset + value.Length > Size)
        return;
      while(value.Length < stringLength)
      {
        value += '\0';
      }
      var bytesStr = ASCIIEncoding.ASCII.GetBytes(value.ToArray(), 0, stringLength);
      var bytes = Cache.GetFileDataUseCache(Offset + entryRelativeOffset + stringLength);
      Array.Copy(bytesStr, 0, bytes, Offset + entryRelativeOffset, stringLength);
      if (!Cache.IsDataCached)
        File.WriteAllBytes(Cache.FileName, bytes);
    }
    protected byte[] GetFixLengthChunk(int entryRelativeOffset, int length, bool forceRetrive = false)
    {
      if (!forceRetrive && entryRelativeOffset + length > Size)
        return null;
      byte[] ret = new byte[length];
      Array.Copy(ReadBytesOrUseCahce(), Offset+entryRelativeOffset, ret, 0, length);
      return ret;
    }
    protected void SetFixLengthChunk(byte[] value, int entryRelativeOffset, bool forceWrite = false)
    {
      if (disableWrite)
        throw new Exception("WRITING IS DISABLED");
      if (!forceWrite && entryRelativeOffset + value.Length > Size)
        return;
      Array.Copy(value, 0, ReadBytesUseCahce(), Offset+entryRelativeOffset,value.Length);
    }
    protected void LengthChangingReplace(byte[] value,int valueStartInd,int valueLength,int entryRelativeOffsetOfData,int previousDataLength)
    {
      var dataToManipulate = Cache.GetFileData();
      var dataBegin = dataToManipulate.Take(entryRelativeOffsetOfData).ToList();
      var dataMiddle = value.Skip(valueStartInd).Take(valueLength);
      var dataEnd = dataToManipulate.Skip(entryRelativeOffsetOfData + previousDataLength);
      dataBegin.AddRange(dataMiddle);
      dataBegin.AddRange(dataEnd);
      Cache = new FileCacheHolder(dataBegin.ToArray());
    }
  }
}
