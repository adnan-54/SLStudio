using Humanizer;
using System;
using System.IO;
using System.Windows;

namespace SLStudio.Core.Modules.StartPage.Models
{
    internal class RecentFileModel
    {
        public RecentFileModel(string location)
        {
            if (File.Exists(location) || Directory.Exists(location))
            {
                Location = location;

                var atribute = File.GetAttributes(location);
                if (atribute.HasFlag(FileAttributes.Directory))
                {
                    var dirInfo = new DirectoryInfo(location);
                    DisplayName = dirInfo.Name;
                    LastTimeSaved = dirInfo.LastWriteTime;
                    IconSource = Application.Current.TryFindResource("FolderClosed");
                    IsDirectory = true;
                }
                else
                {
                    var fileInfo = new FileInfo(location);
                    DisplayName = Path.GetFileName(location);
                    LastTimeSaved = fileInfo.LastWriteTime;
                    IconSource = Application.Current.TryFindResource("Document");
                    IsDirectory = false;
                }
            }
        }

        public object IconSource { get; }
        public string Location { get; }
        public string DisplayName { get; }
        public DateTime LastTimeSaved { get; }
        public string HumanizedLastTimeSaved => LastTimeSaved.Humanize(false);
        public bool IsDirectory { get; }
    }
}