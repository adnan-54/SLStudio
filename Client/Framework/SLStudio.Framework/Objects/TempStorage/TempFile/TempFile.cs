using System.IO;

namespace SLStudio
{
    internal class TempFile : TempItem<FileInfo>, ITempFile
    {
        internal TempFile(FileInfo fileInfo) : base(fileInfo)
        {
        }
    }
}