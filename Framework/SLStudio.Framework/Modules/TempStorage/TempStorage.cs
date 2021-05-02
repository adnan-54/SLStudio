using System;
using System.IO;

namespace SLStudio
{
    internal class TempStorage : StudioService, ITempStorage
    {
        public ITempDirectory NewDirectory(string directory)
        {
            if (string.IsNullOrEmpty(directory))
                directory = StudioConstants.TempDirectory;
            else
                Path.GetFullPath(directory);

            EnsureDirectory(directory);

            var path = Path.Combine(directory, $"{Guid.NewGuid()}");

            while (Directory.Exists(path))
                path = Path.Combine(directory, $"{Guid.NewGuid()}");

            var directoryInfo = new DirectoryInfo(path);
            directoryInfo.Create();

            return new TempDirectory(directoryInfo);
        }

        public ITempFile NewFile(string extension, string directory)
        {
            if (string.IsNullOrEmpty(extension))
                extension = StudioConstants.TempFileExtension;

            if (string.IsNullOrEmpty(directory))
                directory = StudioConstants.TempDirectory;
            else
                Path.GetFullPath(directory);

            if (!extension.StartsWith('.'))
                extension = $".{extension}";

            EnsureDirectory(directory);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var path = Path.Combine(directory, fileName);

            while (File.Exists(path))
            {
                fileName = $"{Guid.NewGuid()}{extension}";
                path = Path.Combine(directory, fileName);
            }

            var fileInfo = new FileInfo(path);
            fileInfo.Create();

            return new TempFile(fileInfo);
        }

        private static void EnsureDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }
    }
}