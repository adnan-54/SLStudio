﻿using Caliburn.Micro;
using SLStudio.Studio.Core.Framework;
using SLStudio.Studio.Core.Framework.Services;
using SLStudio.Studio.Core.Modules.Toolbox.Services;
using SLStudio.Studio.Core.Properties;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace SLStudio.Studio.Core.Modules.Toolbox.ViewModels
{
    [Export(typeof(IToolbox))]
    public class ToolboxViewModel : Tool, IToolbox
    {

        private RelayCommand _searchCommand;

        public ICommand SearchCommand
        {
            get { return _searchCommand ?? (_searchCommand = new RelayCommand(a => Search(a as string))); }
        }

        private readonly IToolboxService _toolboxService;

        public override PaneLocation PreferredLocation
        {
            get { return PaneLocation.Left; }
        }

        private BindableCollection<ToolboxItemViewModel> _filteredItems;

        private readonly BindableCollection<ToolboxItemViewModel> _items;
        public IObservableCollection<ToolboxItemViewModel> Items
        {
            get { return _filteredItems.Count == 0 ? _items : _filteredItems; }
        }

        [ImportingConstructor]
        public ToolboxViewModel(IShell shell, IToolboxService toolboxService)
        {
            DisplayName = Resources.ToolboxDisplayName;

            _items = new BindableCollection<ToolboxItemViewModel>();
            _filteredItems = new BindableCollection<ToolboxItemViewModel>();

            var groupedItems = CollectionViewSource.GetDefaultView(_items);
            groupedItems.GroupDescriptions.Add(new PropertyGroupDescription("Category"));

            _toolboxService = toolboxService;

            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;

            shell.ActiveDocumentChanged += (sender, e) => RefreshToolboxItems(shell);
            RefreshToolboxItems(shell);
        }

        private void RefreshToolboxItems(IShell shell)
        {
            _items.Clear();

            if (shell.ActiveItem == null)
                return;

            _items.AddRange(_toolboxService.GetToolboxItems(shell.ActiveItem.GetType())
                .Select(x => new ToolboxItemViewModel(x)));
        }

        private void Search(string searchTerm)
        {
            _filteredItems.Clear();
            if (!string.IsNullOrWhiteSpace(searchTerm) && searchTerm.Length >= 2)
            {
                foreach (var item in _items.Where(x => x.Name.ToUpper().Contains(searchTerm.ToUpper()) || x.Name.ToUpper().Equals(searchTerm.ToUpper())))
                    _filteredItems.Add(item);
            }
            NotifyOfPropertyChange(() => Items);
        }
    }
}