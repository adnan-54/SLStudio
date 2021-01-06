using Humanizer;
using SLStudio.Core.Modules.StartPage.Converters;
using SLStudio.Core.Modules.StartPage.Resources;
using SLStudio.Core.Resources;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SLStudio.Core.Modules.StartPage.ViewModels
{
    internal class StartPageViewModel : DocumentBase, IStartPage
    {
        private readonly IRecentFilesRepository recentFilesRepository;
        private readonly IFileService fileService;
        private readonly BindableCollection<RecentFileViewModel> recentFiles;

        public StartPageViewModel(IRecentFilesRepository recentFilesRepository, IFileService fileService)
        {
            this.recentFilesRepository = recentFilesRepository;
            this.fileService = fileService;
            recentFiles = new BindableCollection<RecentFileViewModel>();
            RecentFiles = CollectionViewSource.GetDefaultView(recentFiles);
            RecentFiles.GroupDescriptions.Add(new PropertyGroupDescription(null, new StartPageGroupFilterConverter()));
            RecentFiles.SortDescriptions.Add(new SortDescription("IsPinned", ListSortDirection.Descending));
            RecentFiles.SortDescriptions.Add(new SortDescription("ModifiedDate", ListSortDirection.Descending));
            RecentFiles.Filter += Filter;
            recentFiles.CollectionChanged += RecentFilesOnCollectionChanged;

            if (StudioSettings.Default.StartPagePinnedFiles == null)
                StudioSettings.Default.StartPagePinnedFiles = new();

            DisplayName = StartPageResources.StartPage;
        }

        public ICollectionView RecentFiles { get; }

        public string SearchTerm
        {
            get => GetProperty(() => SearchTerm);
            set
            {
                SetProperty(() => SearchTerm, value);
                RecentFiles.Refresh();
            }
        }

        public bool CanClearRecentFiles => recentFiles.Count > 0;

        public void ClearAll()
        {
            //todo: show confirmation

            foreach (var file in recentFiles)
                Remove(file);
        }

        public void UpdatePinned(RecentFileViewModel item)
        {
            if (item.IsPinned)
            {
                if (!StudioSettings.Default.StartPagePinnedFiles.Contains(item.Location))
                {
                    StudioSettings.Default.StartPagePinnedFiles.Add(item.Location);
                    StudioSettings.Default.Save();
                }
            }
            else
            {
                if (StudioSettings.Default.StartPagePinnedFiles.Contains(item.Location))
                {
                    StudioSettings.Default.StartPagePinnedFiles.Remove(item.Location);
                    StudioSettings.Default.Save();
                }
            }

            RecentFiles.Refresh();
        }

        public void OpenItem(RecentFileViewModel item)
        {
            if (File.Exists(item.Location))
            {
                fileService.Open(item.Location);
            }
            else
            {
                //todo: show error and ask if user want to remove from list
            }
        }

        public void Remove(RecentFileViewModel item)
        {
            recentFiles.Remove(item);
            if (StudioSettings.Default.StartPagePinnedFiles.Contains(item.Location))
            {
                StudioSettings.Default.StartPagePinnedFiles.Remove(item.Location);
                StudioSettings.Default.Save();
            }
            recentFilesRepository.Remove(item.Location);

            RecentFiles.Refresh();
        }

        public void OnLoaded()
        {
            FetchRecentFiles().FireAndForget();
        }

        private async Task FetchRecentFiles()
        {
            recentFiles.Clear();

            var fromRepository = await recentFilesRepository.GetAll();
            var files = fromRepository.Select(r => CreateRecentFile(r.FileName, r.CreationDate));
            recentFiles.AddRange(files);

            foreach (var pinned in StudioSettings.Default.StartPagePinnedFiles)
            {
                var target = recentFiles.FirstOrDefault(r => r.Location.Equals(pinned));
                if (target != null)
                    target.IsPinned = true;
            }

            RecentFiles.Refresh();

            RecentFileViewModel CreateRecentFile(string fileName, DateTime date)
            {
                Uri iconUri = null;
                var extension = Path.GetExtension(fileName);
                var iconSource = fileService.GetDescription(extension)?.IconSource;

                if (!string.IsNullOrEmpty(iconSource))
                    iconUri = new Uri(iconSource);

                return new RecentFileViewModel(fileName, date, this, iconUri);
            }
        }

        private void RecentFilesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(() => CanClearRecentFiles);
        }

        private bool Filter(object obj)
        {
            if (obj is RecentFileViewModel recentFile)
                return string.IsNullOrEmpty(SearchTerm)
                    || recentFile.FileName.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase)
                    || recentFile.Location.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase)
                    || recentFile.ModifiedDate.ToString().Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase)
                    || recentFile.ModifiedDate.Humanize(false).Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase);
            return false;
        }
    }

    internal interface IStartPage : IDocumentItem
    {
    }
}