using Newtonsoft.Json;
using SLStudio.Core.Services.RecentFilesRepository.Resources;
using System.Collections.Generic;

namespace SLStudio.Core
{
    internal class DefaultRecentFilesRespository : IRecentFilesRepository
    {
        private readonly List<RecentFile> recentFiles;

        public DefaultRecentFilesRespository()
        {
            recentFiles = new List<RecentFile>();
            Load();
        }

        public IEnumerable<RecentFile> RecentFiles => recentFiles;

        public void Add(RecentFile file)
        {
            if (!recentFiles.Contains(file))
            {
                recentFiles.Add(file);
                Save();
            }
        }

        public void Remove(RecentFile file)
        {
            recentFiles.Remove(file);
            Save();
        }

        public void RemoveAll()
        {
            recentFiles.Clear();
            Save();
        }

        private void Load()
        {
            var list = JsonConvert.DeserializeObject<List<RecentFile>>(RecentFilesSettings.Default.RecentFiles);
            if (list != null)
            {
                recentFiles.Clear();
                recentFiles.AddRange(list);
            }
        }

        private void Save()
        {
            var jsonString = JsonConvert.SerializeObject(recentFiles);
            RecentFilesSettings.Default.RecentFiles = jsonString;
            RecentFilesSettings.Default.Save();
        }
    }

    public interface IRecentFilesRepository
    {
        IEnumerable<RecentFile> RecentFiles { get; }

        void Add(RecentFile file);

        void Remove(RecentFile file);

        void RemoveAll();
    }
}