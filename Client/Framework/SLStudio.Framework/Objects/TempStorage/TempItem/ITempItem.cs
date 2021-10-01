using System;
using System.IO;

namespace SLStudio
{
    public interface ITempItem<T> : IDisposable where T : FileSystemInfo
    {
        string FullName { get; }

        T StorageInfo { get; }
    }
}