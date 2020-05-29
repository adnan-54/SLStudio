using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SlrrLib.Model
{
  public class FileCacheHolder
  {
    private string fnam = "";
    private byte[] cachedData = null;
    private bool isDataCached = false;

    public bool IsDataCached
    {
      get
      {
        return isDataCached;
      }
    }
    public string FileName
    {
      get
      {
        return fnam;
      }
    }

    public FileCacheHolder(string fnam, bool cacheData = false)
    {
      this.fnam = fnam;
      if (cacheData)
        CacheData();
    }
    public FileCacheHolder(byte[] dataToCopy)
    {
      this.fnam = "";
      isDataCached = true;
      cachedData = new byte[dataToCopy.Length];
      Array.Copy(dataToCopy, cachedData, dataToCopy.Length);
    }
    public FileCacheHolder(int bufferSize)
    {
      this.fnam = "";
      isDataCached = true;
      cachedData = new byte[bufferSize];
    }

    public void ClearCache()
    {
      isDataCached = false;
      cachedData = null;
    }
    public void CacheData()
    {
      if (isDataCached == false)
      {
        isDataCached = true;
        cachedData = File.ReadAllBytes(fnam);
      }
    }
    public int GetFileDataLength()
    {
      return (int)(ReadBytesOrUseCahce().Length);
    }
    public byte[] GetFileData()
    {
      return ReadBytesOrUseCahce();
    }
    public byte[] GetFileDataUseCache(int minSize)
    {
      CacheData();
      if (cachedData.Length < minSize)
      {
        byte[] newCache = new byte[minSize];
        Array.Copy(cachedData, newCache, cachedData.Length);
        cachedData = newCache;
      }
      return cachedData;
    }
    public void SaveCachedData(bool backUp = true)
    {
      if (backUp && File.Exists(fnam))
      {
        int bakInd = 0;
        while (File.Exists(fnam + "_BAK_FileCache_" + bakInd.ToString()))
          bakInd++;
        File.Copy(fnam, fnam + "_BAK_FileCache_" + bakInd.ToString());
      }
      try
      {

        File.WriteAllBytes(fnam,ReadBytesOrUseCahce());
      }
      catch(Exception)
      {
        MessageLog.AddError("Could not write file: " + fnam);
      }
    }
    public override string ToString()
    {
      return fnam;
    }

    protected byte[] ReadBytesOrUseCahce()
    {

      if (isDataCached)
        return cachedData;
      return File.ReadAllBytes(fnam);
    }
  }
}
