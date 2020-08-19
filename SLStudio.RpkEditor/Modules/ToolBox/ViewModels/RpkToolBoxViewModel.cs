using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using SLStudio.Core;
using SLStudio.Logging;
using SLStudio.RpkEditor.Data;
using SLStudio.RpkEditor.Modules.RpkEditor.ViewModels;
using SLStudio.RpkEditor.Modules.Toolbox.Models;
using SLStudio.RpkEditor.Modules.Toolbox.Resources;
using SLStudio.RpkEditor.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Data;
using Xceed.Wpf.Toolkit.Core.Converters;

namespace SLStudio.RpkEditor.Modules.ToolBox.ViewModels
{
    internal class RpkToolBoxViewModel : ViewModelBase, IToolboxContent
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<RpkToolBoxViewModel>();

        private readonly RpkEditorViewModel rpkEditor;
        private readonly IRpkManager rpkManager;

        private readonly BindableCollection<ToolboxItemModel> resources;

        public RpkToolBoxViewModel(RpkEditorViewModel rpkEditor, IRpkManager rpkManager)
        {
            this.rpkEditor = rpkEditor;
            this.rpkManager = rpkManager;

            resources = new BindableCollection<ToolboxItemModel>();

            Resources = CollectionViewSource.GetDefaultView(resources);
            Resources.GroupDescriptions.Add(new PropertyGroupDescription(null, new ToolboxItemsFilterConverter()));
            Resources.SortDescriptions.Add(new SortDescription("IsPinned", ListSortDirection.Descending));
            Resources.SortDescriptions.Add(new SortDescription("Category", ListSortDirection.Descending));
            Resources.CollectionChanged += OnCollectionChanged;
            Resources.Filter += Filter;
        }

        public bool IsBusy
        {
            get => GetProperty(() => IsBusy);
            set => SetProperty(() => IsBusy, value);
        }

        public string SearchTerm
        {
            get => GetProperty(() => SearchTerm);
            set
            {
                SetProperty(() => SearchTerm, value);
                Resources.Refresh();
            }
        }

        public ICollectionView Resources { get; }

        public ToolboxItemModel SelectedItem
        {
            get => GetProperty(() => SelectedItem);
            set => SetProperty(() => SelectedItem, value);
        }

        public bool FocusRequested
        {
            get => GetProperty(() => FocusRequested);
            set => SetProperty(() => FocusRequested, value);
        }

        public void OnLoaded()
        {
            FetchItems().FireAndForget();
        }

        public void SelectPrev()
        {
            var items = Resources.Cast<ToolboxItemModel>().ToList();
            var currentIndex = items.IndexOf(SelectedItem);
            if (currentIndex > 0)
                SelectedItem = items[--currentIndex];
        }

        public void SelectNext()
        {
            var items = Resources.Cast<ToolboxItemModel>().ToList();
            var currentIndex = items.IndexOf(SelectedItem);
            if (currentIndex < items.Count - 1)
                SelectedItem = items[++currentIndex];
        }

        public void AddResource()
        {
            if (SelectedItem != null)
                rpkManager.AddResource(SelectedItem.Metadata.GetType());
        }

        public void ToggleIsPinned()
        {
            if (SelectedItem != null)
            {
                SelectedItem.IsPinned = !SelectedItem.IsPinned;

                var tag = SelectedItem.Metadata.GetType().Name;

                if (SelectedItem.IsPinned)
                {
                    if (!ToolboxSettings.Default.PinnedItems.Contains(tag))
                        ToolboxSettings.Default.PinnedItems.Add(tag);
                }
                else
                {
                    if (ToolboxSettings.Default.PinnedItems.Contains(tag))
                        ToolboxSettings.Default.PinnedItems.Remove(tag);
                }

                ToolboxSettings.Default.Save();

                Resources.Refresh();
            }
        }

        public void FocusSearch()
        {
            FocusRequested = false;
            FocusRequested = true;
        }

        public void FocusDesignerSearch()
        {
            rpkEditor.FocusDesignerSearch();
        }

        private Task FetchItems()
        {
            try
            {
                IsBusy = true;

                resources.Clear();

                //todo: move this to a service so it once create the itens once
                //todo: create resource metadata using objectFactory
                var metadatas = Assembly.GetExecutingAssembly().GetTypes()
                                        .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ResourceMetadata)))
                                        .Select(t => (ResourceMetadata)Activator.CreateInstance(t))
                                        .Select(m => new ToolboxItemModel(m));

                resources.AddRange(metadatas);

                foreach (var resource in resources)
                {
                    var tag = resource.Metadata.GetType().Name;
                    if (ToolboxSettings.Default.PinnedItems.Contains(tag))
                        resource.IsPinned = true;
                }

                if (SelectedItem == null)
                    SelectedItem = resources.FirstOrDefault();

                Resources.Refresh();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            finally
            {
                IsBusy = false;
            }

            return Task.CompletedTask;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var items = Resources.Cast<ToolboxItemModel>();
            if (!items.Contains(SelectedItem))
                SelectedItem = items.FirstOrDefault();
        }

        private bool Filter(object obj)
        {
            if (obj is ToolboxItemModel item)
                return string.IsNullOrEmpty(SearchTerm)
                    || item.DisplayName.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase)
                    || item.Category.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase);
            return false;
        }
    }

    internal class ToolboxItemsFilterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ToolboxItemModel recentFile)
            {
                if (recentFile.IsPinned)
                    return "Pinned";
                else
                    return recentFile.Category;
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}