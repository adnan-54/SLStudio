using Ionic.Zip;
using SLStudio.Framework;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace SLStudio.Logging
{
    internal class ZipExporter : IDisposable
    {
        private readonly ITempDirectory tempDirectory;
        private readonly string outputFileName;

        public ZipExporter(string fileName)
        {
            outputFileName = fileName;
            tempDirectory = TempStorage.NewDirectory();
        }

        public void Export()
        {
            CreateSystemInfoFile();
            CopyFilesToTempDirectory();
            ZipFiles();
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

            File.WriteAllText(Path.Combine(tempDirectory.FullName, "environment.bin"), sb.ToString());
        }

        private void CopyFilesToTempDirectory()
        {
            File.Copy(StudioConstants.StudioLogFile, Path.Combine(tempDirectory.FullName, "logs.bin"));
            File.Copy(StudioConstants.InternalLogFile, Path.Combine(tempDirectory.FullName, "internallogs.bin"));
        }

        private void ZipFiles()
        {
            using var zipFile = new ZipFile()
            {
                Password = "4@-B$Vq6@$*7QChW"
            };
            zipFile.AddDirectory(tempDirectory.FullName);
            zipFile.Save(outputFileName);
        }

        public void Dispose()
        {
            tempDirectory.Dispose();
        }
    }
}