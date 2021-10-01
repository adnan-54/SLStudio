using System.IO;

namespace SLStudio
{
    internal class TempDirectory : TempItem<DirectoryInfo>, ITempDirectory
    {
        internal TempDirectory(DirectoryInfo directoryInfo) : base(directoryInfo)
        {
        }
    }
}