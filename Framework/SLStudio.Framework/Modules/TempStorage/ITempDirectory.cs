using System.IO;

namespace SLStudio
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