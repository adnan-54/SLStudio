using SLStudio.Core.Resources;
using SLStudio.Logging;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    internal class DefaultRecentFilesRespository : IRecentFilesRepository
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<DefaultRecentFilesRespository>();

        private readonly object repositoryLock;

        public DefaultRecentFilesRespository()
        {
            repositoryLock = new object();

            if (StudioSettings.Default.RecentFiles == null)
                StudioSettings.Default.RecentFiles = new();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public async Task<IEnumerable<IRecentFile>> GetAll()
        {
            return await Task.Run(() =>
            {
                lock (repositoryLock)
                {
                    var settings = StudioSettings.Default.RecentFiles;
                    if (settings == null || settings.Count == 0)
                        return Enumerable.Empty<IRecentFile>();

                    var recentFiles = new string[settings.Count];
                    settings.CopyTo(recentFiles, 0);

                    var hasChanged = false;
                    var result = new List<RecentFile>();
                    foreach (var file in recentFiles)
                    {
                        if (RecentFile.TryParse(file, out RecentFile parsed))
                            result.Add(parsed);
                        else
                            hasChanged |= RemoveInternal(file);
                    }

                    if (hasChanged)
                        StudioSettings.Default.Save();

                    return result;
                }
            });
        }

        Task IRecentFilesRepository.Add(string fileName)
        {
            lock (repositoryLock)
            {
                try
                {
                    RemoveInternal(fileName);

                    var item = new RecentFile(fileName, DateTime.Now);
                    StudioSettings.Default.RecentFiles.Add(item.ToString());
                    StudioSettings.Default.Save();

                    RaiseCollectionChanged(NotifyCollectionChangedAction.Add, fileName);
                }
                catch (Exception ex)
                {
                    logger.Warn(ex);
                }
            }

            return Task.FromResult(true);
        }

        Task IRecentFilesRepository.Remove(string fileName)
        {
            lock (repositoryLock)
            {
                try
                {
                    if (RemoveInternal(fileName))
                    {
                        StudioSettings.Default.Save();
                        RaiseCollectionChanged(NotifyCollectionChangedAction.Remove, fileName);
                    }
                }
                catch (Exception ex)
                {
                    logger.Warn(ex);
                }
            }

            return Task.FromResult(true);
        }

        Task IRecentFilesRepository.Clear()
        {
            lock (repositoryLock)
            {
                try
                {
                    StudioSettings.Default.RecentFiles.Clear();
                    StudioSettings.Default.Save();
                    RaiseCollectionChanged(NotifyCollectionChangedAction.Reset, null);
                }
                catch (Exception ex)
                {
                    logger.Warn(ex);
                }
            }

            return Task.FromResult(true);
        }

        private static bool RemoveInternal(string fileName)
        {
            for (int i = 0; i < StudioSettings.Default.RecentFiles.Count; i++)
            {
                var entry = StudioSettings.Default.RecentFiles[i];
                if (entry.Contains(fileName))
                {
                    StudioSettings.Default.RecentFiles.Remove(entry);
                    return true;
                }
            }
            return false;
        }

        private void RaiseCollectionChanged(NotifyCollectionChangedAction action, object item)
        {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, item));
        }
    }

    public interface IRecentFilesRepository
    {
        event NotifyCollectionChangedEventHandler CollectionChanged;

        Task<IEnumerable<IRecentFile>> GetAll();

        Task Add(string fileName);

        Task Remove(string fileName);

        Task Clear();
    }
}