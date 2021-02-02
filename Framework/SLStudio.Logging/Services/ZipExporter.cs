using Ionic.Zip;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace SLStudio.Logging
{
    internal class ZipExporter : IDisposable
    {
        private readonly string tempDirectoryName;
        private readonly string outputFileName;

        public ZipExporter(string fileName)
        {
            outputFileName = fileName;
            tempDirectoryName = StudioConstants.NewTempDirectory;
        }

        public void Export()
        {
            CreateTempDirectory();
            CreateSystemInfoFile();
            CopyFilesToTempDirectory();
            ZipFiles();
        }

        private void CreateTempDirectory()
        {
            Directory.CreateDirectory(tempDirectoryName);
        }

        private void CreateSystemInfoFile()
        {
            var sb = new StringBuilder("SLStudio Local System Environment Infos");
            sb.AppendLine();

            var properties = typeof(Environment).GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            foreach (var property in properties)
            {
                try
                {
                    sb.AppendLine($"{property.Name}: {property.GetValue(null, null)}");
                }
                catch { }
            }

            File.WriteAllText(Path.Combine(tempDirectoryName, "localInfos.txt"), sb.ToString());
        }

        private void CopyFilesToTempDirectory()
        {
            File.Copy(StudioConstants.LogsFile, Path.Combine(tempDirectoryName, "logs.db"));
            File.Copy(StudioConstants.InternalLogsFile, Path.Combine(tempDirectoryName, "logs.txt"));
        }

        private void ZipFiles()
        {
            using var zipFile = new ZipFile()
            {
                Password = "4@-B$Vq6@$*7QChW"
            };
            zipFile.AddDirectory(tempDirectoryName);
            zipFile.Save(outputFileName);
        }

        public void Dispose()
        {
            try
            {
                if (Directory.Exists(tempDirectoryName))
                    Directory.Delete(tempDirectoryName, true);
            }
            catch { }
        }
    }
}