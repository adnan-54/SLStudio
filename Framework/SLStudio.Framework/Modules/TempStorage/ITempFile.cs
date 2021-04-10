using System.IO;

namespace SLStudio
{
    public interface ITempFile : ITempStorage<FileInfo>
    {
    }

    internal class TempFile : TempStorage<FileInfo>, ITempFile
    {
        internal TempFile(FileInfo fileInfo) : base(fileInfo)
        {
        }
    }
}