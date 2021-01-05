using Humanizer;
using SLStudio.Core.Modules.StartPage.Converters;
using SLStudio.Core.Modules.StartPage.Resources;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

namespace SLStudio.Core.Modules.StartPage.ViewModels
{
    internal class StartPageViewModel : DocumentBase
    {
        private readonly BindableCollection<RecentFileViewModel> recentFiles;

        public StartPageViewModel()
        {
            recentFiles = new BindableCollection<RecentFileViewModel>();
            RecentFiles = CollectionViewSource.GetDefaultView(recentFiles);
            RecentFiles.GroupDescriptions.Add(new PropertyGroupDescription(null, new StartPageGroupFilterConverter()));
            RecentFiles.SortDescriptions.Add(new SortDescription("IsPinned", ListSortDirection.Descending));
            RecentFiles.SortDescriptions.Add(new SortDescription("ModifiedDate", ListSortDirection.Descending));
            RecentFiles.Filter += Filter;
            recentFiles.CollectionChanged += RecentFilesOnCollectionChanged;

            FetchRecentFiles();

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
            item.PropertyChanged -= RecentFileOnPropertyChanged;
            recentFiles.Remove(item);
        }

        public void ClearAll()
        {
            foreach (var recentFile in recentFiles)
                recentFile.PropertyChanged -= RecentFileOnPropertyChanged;

            recentFiles.Clear();
        }

        private void FetchRecentFiles()
        {
            foreach (var recentFile in recentFiles)
                recentFile.PropertyChanged -= RecentFileOnPropertyChanged;

            recentFiles.Clear();

            foreach (var recentFile in recentFiles)
                recentFile.PropertyChanged += RecentFileOnPropertyChanged;
        }

        private void RecentFilesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(() => CanClearRecentFiles);
        }

        private void RecentFileOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(RecentFileViewModel.IsPinned))
                RecentFiles.Refresh();
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