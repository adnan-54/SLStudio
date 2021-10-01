using System;
using System.Diagnostics;
using System.IO;

namespace SLStudio
{
    internal class TempItem<T> : ITempItem<T> where T : FileSystemInfo
    {
        internal TempItem(T storageInfo)
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