using System.IO;

namespace SLStudio.Framework
{
    public interface ITempDirectory : ITempStorage<DirectoryInfo>
    {
    }

    internal class TempDirectory : TempStorage<DirectoryInfo>, ITempDirectory
    {
        internal TempDirectory(DirectoryInfo directoryInfo) : base(directoryInfo)
        {
        }
    }
}