using Caliburn.Micro;
using SLStudio.Core.Docking;
using SLStudio.Core.Modules.StartPage.Models;
using SLStudio.Core.Modules.StartPage.Resources;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Data;

namespace SLStudio.Core.Modules.StartPage.ViewModels
{
    internal class StartPageViewModel : DocumentBase
    {
        private static readonly ILogger logger = LogManager.GetLogger<StartPageViewModel>();
        private readonly BindableCollection<RecentFileModel> recentFiles;

        public StartPageViewModel()
        {
            recentFiles = new BindableCollection<RecentFileModel>();
            FetchRecentFiles();
            DisplayName = StartPageResources.StartPage;

            RecentFiles = CollectionViewSource.GetDefaultView(recentFiles);
            RecentFiles.GroupDescriptions.Add(new PropertyGroupDescription("HumanizedLastTimeSaved"));
            RecentFiles.SortDescriptions.Add(new SortDescription("LastTimeSaved", ListSortDirection.Descending));
            RecentFiles.Filter += Filter;
            logger.Info("caiu aqui");
        }

        public string SearchTerm
        {
            get => GetProperty(() => SearchTerm);
            set
            {
                SetProperty(() => SearchTerm, value);
                RecentFiles.Refresh();
            }
        }

        public ICollectionView RecentFiles { get; }

        public RecentFileModel SelectedFile
        {
            get => GetProperty(() => SelectedFile);
            set => SetProperty(() => SelectedFile, value);
        }

        public void OpenRecentFile(RecentFileModel recentFile)
        {
        }

        public void ClearAll()
        {
            recentFiles.Clear();
        }

        public void OpenDirectory()
        {
            if (SelectedFile == null)
                return;

            try
            {
                var targetDir = SelectedFile.Location;
                if (!SelectedFile.IsDirectory)
                    targetDir = Path.GetDirectoryName(targetDir);

                Process.Start(targetDir);
            }
            catch (Exception exception)
            {
                logger.Warning(exception.Message, exception.ToString());
            }
        }

        public void CloneRepository()
        {
        }

        public void OpenProjectOrSolution()
        {
        }

        public void OpenLocalFolder()
        {
        }

        public void CreateNewProject()
        {
        }

        private void FetchRecentFiles()
        {
        }

        private bool Filter(object obj)
        {
            if (obj is RecentFileModel recentFile)
                return string.IsNullOrEmpty(SearchTerm)
                    || recentFile.DisplayName.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase)
                    || recentFile.Location.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase)
                    || recentFile.LastTimeSaved.ToString().Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase)
                    || recentFile.HumanizedLastTimeSaved.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase);
            return false;
        }
    }
}