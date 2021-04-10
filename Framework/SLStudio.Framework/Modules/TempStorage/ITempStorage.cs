using System;
using System.Diagnostics;
using System.IO;

namespace SLStudio
{
    public interface ITempStorage : IDisposable
    {
        string FullName { get; }
    }

    public interface ITempStorage<T> : ITempStorage where T : FileSystemInfo
    {
        T StorageInfo { get; }
    }

    internal class TempStorage<T> : ITempStorage<T> where T : FileSystemInfo
    {
        internal TempStorage(T storageInfo)
        {
            StorageInfo = storageInfo;
        }

        public string FullName => StorageInfo.FullName;

        public T StorageInfo { get; }

        public void Dispose()
        {
            try
            {
                StorageInfo.Delete();
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }
        }

        public override string ToString()
        {
            return FullName;
        }
    }
}