using Humanizer;
using SLStudio.Core.Modules.StartPage.Converters;
using SLStudio.Core.Modules.StartPage.Resources;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SLStudio.Core.Modules.StartPage.ViewModels
{
    internal class StartPageViewModel : DocumentBase
    {
        private readonly IRecentFilesRepository recentFilesRepository;
        private readonly BindableCollection<RecentFileViewModel> recentFiles;

        public StartPageViewModel(IRecentFilesRepository recentFilesRepository)
        {
            this.recentFilesRepository = recentFilesRepository;
            recentFiles = new BindableCollection<RecentFileViewModel>();
            RecentFiles = CollectionViewSource.GetDefaultView(recentFiles);
            RecentFiles.GroupDescriptions.Add(new PropertyGroupDescription(null, new StartPageGroupFilterConverter()));
            RecentFiles.SortDescriptions.Add(new SortDescription("IsPinned", ListSortDirection.Descending));
            RecentFiles.SortDescriptions.Add(new SortDescription("ModifiedDate", ListSortDirection.Descending));
            RecentFiles.Filter += Filter;
            recentFiles.CollectionChanged += RecentFilesOnCollectionChanged;

            FetchRecentFiles().FireAndForget();

            DisplayName = StartPageResources.StartPage;
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

        public bool CanClearRecentFiles => recentFiles.Count > 0;

        public ICollectionView RecentFiles { get; }

        public void RemoveItem(RecentFileViewModel item)
        {
            recentFiles.Remove(item);
        }

        public void ClearAll()
        {
            recentFiles.Clear();
        }

        private async Task FetchRecentFiles()
        {
            recentFiles.Clear();

            var fromRepository = await recentFilesRepository.GetAll();
            recentFiles.AddRange(fromRepository.Select(r => new RecentFileViewModel(r.FileName, r.CreationDate)));
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
}