using System;
using System.Text;

namespace SlrrLib.Model
{
    public class Scx
    {
        protected int readVersion;
        protected int versionOffset = 4;
        protected FileCacheHolder fileCache = null;

        public string FileSignature
        {
            get;
            set;
        }

        public bool ProperlyReadData
        {
            get;
            protected set;
        }

        public string FileName
        {
            get
            {
                return fileCache.FileName;
            }
        }

        public int Version
        {
            get
            {
                var bytes = fileCache.GetFileData();
                if (bytes.Length < 8)
                    return -1;
                return BitConverter.ToInt32(bytes, versionOffset);
            }
            set
            {
                var bytes = fileCache.GetFileData();
                var bytesToWrite = BitConverter.GetBytes(value);
                Array.Copy(bytesToWrite, 0, bytes, 4, versionOffset);
            }
        }

        public static Scx ConstructScx(string fnam, bool cache = false)
        {
            Scx ret = new Scx(fnam);
            if (ret.Version == 4)
                return new BinaryScxV4(fnam, cache);
            if (ret.Version == 3)
                return new BinaryScxV3(fnam, cache);
            return ret;
        }

        public Scx(string fnam)
        {
            this.fileCache = new FileCacheHolder(fnam);
        }

        public void CahceData()
        {
            if (fileCache == null)
                return;
            fileCache.CacheData();
        }

        public void SaveCachedData(bool backUp = true)
        {
            fileCache.SaveCachedData(backUp);
        }

        public virtual void ReadData(bool readVertexData = false)
        {
            MessageLog.AddError("Non proper version");
        }

        public virtual bool HasAnyReflection()
        {
            return false;
        }

        public virtual void SetAllreflectionPercent(float percent)
        {
        }

        protected string GetDecTextFromBytes(byte[] bytes, int start, int size)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i != size; ++i)
            {
                if (i % 16 == 0)
                    sb.AppendLine();
                sb.Append(bytes[i + start].ToString("D3") + " ");
            }
            sb.AppendLine();
            return sb.ToString();
        }
    }
}