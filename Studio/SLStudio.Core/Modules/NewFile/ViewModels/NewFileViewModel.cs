using SLStudio.Core.Modules.NewFile.Models;
using SLStudio.Core.Modules.NewFile.Resources;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SLStudio.Core.Modules.NewFile.ViewModels
{
    internal partial class NewFileViewModel : WindowViewModel, INewFileDialog
    {
        private readonly IFileService fileService;
        private readonly Dictionary<string, CategoryModel> categoriesCache;
        private readonly ICollectionView availableFilesView;

        public NewFileViewModel(IFileService fileService)
        {
            this.fileService = fileService;
            categoriesCache = new Dictionary<string, CategoryModel>();

            AvailableCategories = new BindableCollection<CategoryModel>();
            AvailableSortModes = new List<SortModeModel>()
            {
                new SortModeModel(NewFileResources.combo_Default, new SortDescription()),
                new SortModeModel(NewFileResources.combo_NameAscending, new SortDescription(nameof(SortModeModel.DisplayName), ListSortDirection.Ascending)),
                new SortModeModel(NewFileResources.combo_NameDescending, new SortDescription(nameof(SortModeModel.DisplayName), ListSortDirection.Descending))
            };
            AvailableFiles = new BindableCollection<NewFileModel>();
            availableFilesView = CollectionViewSource.GetDefaultView(AvailableFiles);
            availableFilesView.Filter += Filter;
            availableFilesView.CollectionChanged += OnCollectionChanged;

            FetchCategories().FireAndForget();
            FetchFiles().FireAndForget();

            DisplayName = NewFileResources.window_Title;
        }

        public BindableCollection<CategoryModel> AvailableCategories { get; }

        public IEnumerable<SortModeModel> AvailableSortModes { get; }

        public BindableCollection<NewFileModel> AvailableFiles { get; }

        public CategoryModel SelectedCategory => categoriesCache.Values.FirstOrDefault(c => c.IsSelected);

        public SortModeModel SelectedSortMode
        {
            get => GetProperty(() => SelectedSortMode);
            set
            {
                SetProperty(() => SelectedSortMode, value);
                Sort();
            }
        }

        public NewFileModel SelectedFile
        {
            get => GetProperty(() => SelectedFile);
            set
            {
                SetProperty(() => SelectedFile, value);
                RaisePropertyChanged(() => CanOpen);
            }
        }

        public string SearchTerm
        {
            get => GetProperty(() => SearchTerm);
            set
            {
                SetProperty(() => SearchTerm, value);
                availableFilesView.Refresh();
            }
        }

        public bool CanOpen => SelectedFile != null;

        public void Open()
        {
            if (!CanOpen)
                return;

            fileService.New(SelectedFile.FileDescription.Extensions.FirstOrDefault()).FireAndForget();
            TryClose(true);
        }

        private Task FetchCategories()
        {
            CreateBaseCache();
            CreateCategories();
            PopulateParents();
            AddRootCategories();

            return Task.FromResult(true);
        }

        private void CreateBaseCache()
        {
            foreach (var file in fileService.GetDescriptions().Where(d => !d.ReadOnly))
            {
                if (!categoriesCache.ContainsKey(file.Category))
                    categoriesCache.Add(file.Category, new CategoryModel(file.Category));
            }
        }

        private void CreateCategories()
        {
            foreach (var category in categoriesCache.Values.ToList())
            {
                var splitted = category.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);
                var newPath = string.Empty;
                foreach (var split in splitted)
                {
                    newPath = $"{newPath}{split}/";
                    if (!categoriesCache.ContainsKey(newPath))
                        categoriesCache.Add(newPath, new CategoryModel(newPath));
                }
            }
        }

        private void PopulateParents()
        {
            foreach (var category in categoriesCache.Values)
            {
                var splittedPath = category.Path.Split('/', StringSplitOptions.RemoveEmptyEntries);
                var parentPath = string.Join("/", splittedPath.Take(splittedPath.Length - 1));

                if (!parentPath.EndsWith('/'))
                    parentPath = $"{parentPath}/";

                if (categoriesCache.TryGetValue(parentPath, out var parent))
                    parent.Children.Add(category);
            }
        }

        private void AddRootCategories()
        {
            var rootCategories = categoriesCache.Where(c => c.Key.Count(c => c == '/') == 1).Select(c => c.Value);
            foreach (var category in rootCategories)
                AvailableCategories.Add(category);
        }

        private Task FetchFiles()
        {
            var files = fileService.GetDescriptions().Where(d => !d.ReadOnly);

            foreach (var file in files)
            {
                var fileModel = new NewFileModel(file);
                AvailableFiles.Add(fileModel);
            }

            return Task.FromResult(true);
        }

        private bool Filter(object obj)
        {
            if (obj is not NewFileModel model)
                return false;

            if (SelectedCategory != null)
            {
                if (string.IsNullOrEmpty(SearchTerm))
                    return SelectedCategory.Match(model);

                return SelectedCategory.Match(model) && (model.DisplayName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) || model.Category.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (string.IsNullOrEmpty(SearchTerm))
                return true;

            return model.DisplayName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) || model.Category.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase);
        }

        private void Sort()
        {
            if (SelectedSortMode == null)
                return;

            using (availableFilesView.DeferRefresh())
            {
                availableFilesView.SortDescriptions.Clear();
                if (!string.IsNullOrEmpty(SelectedSortMode.SortDescription.PropertyName))
                    availableFilesView.SortDescriptions.Add(SelectedSortMode.SortDescription);
            }
        }

        public void OnSelectedCategoryChanged()
        {
            availableFilesView.Refresh();
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var view = availableFilesView.Cast<NewFileModel>();
            if (!view.Contains(SelectedFile))
                SelectedFile = view.FirstOrDefault();
        }

        protected override void OnLoaded()
        {
            if (SelectedCategory == null)
                AvailableCategories.FirstOrDefault().IsSelected = true;

            if (SelectedSortMode == null)
                SelectedSortMode = AvailableSortModes.FirstOrDefault();

            if (SelectedFile == null)
                SelectedFile = AvailableFiles.FirstOrDefault();
        }
    }

    internal interface INewFileDialog : IWindow
    {
    }
}