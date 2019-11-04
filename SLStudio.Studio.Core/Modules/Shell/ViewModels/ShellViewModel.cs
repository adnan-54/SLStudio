using Caliburn.Micro;
using SLStudio.Studio.Core.Framework;
using SLStudio.Studio.Core.Framework.Services;
using SLStudio.Studio.Core.Framework.Themes;
using SLStudio.Studio.Core.Modules.MainMenu;
using SLStudio.Studio.Core.Modules.Shell.Services;
using SLStudio.Studio.Core.Modules.Shell.Views;
using SLStudio.Studio.Core.Modules.StatusBar;
using SLStudio.Studio.Core.Modules.ToolBars;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows;

namespace SLStudio.Studio.Core.Modules.Shell.ViewModels
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<IDocument>.Collection.OneActive, IShell
    {
        public event EventHandler ActiveDocumentChanging;
        public event EventHandler ActiveDocumentChanged;

        [ImportMany(typeof(IModule))]
        private IEnumerable<IModule> _modules;

        [Import]
        private IThemeManager _themeManager;

        [Import]
        private IMenu _mainMenu;

        [Import]
        private IToolBars _toolBars;

        [Import]
        private IStatusBar _statusBar;

        [Import]
        private ILayoutItemStatePersister _layoutItemStatePersister;

        private IShellView _shellView;
        private bool _closing;

        public IMenu MainMenu
        {
            get { return _mainMenu; }
        }

        public IToolBars ToolBars
        {
            get { return _toolBars; }
        }

        public IStatusBar StatusBar
        {
            get { return _statusBar; }
        }

        private ILayoutItem _activeLayoutItem;
        public ILayoutItem ActiveLayoutItem
        {
            get { return _activeLayoutItem; }
            set
            {
                if (ReferenceEquals(_activeLayoutItem, value))
                    return;

                _activeLayoutItem = value;

                if (value is IDocument)
                    ActivateItem((IDocument)value);

                NotifyOfPropertyChange(() => ActiveLayoutItem);
            }
        }

        private readonly BindableCollection<ITool> _tools;
        public IObservableCollection<ITool> Tools
        {
            get { return _tools; }
        }

        public IObservableCollection<IDocument> Documents
        {
            get { return Items; }
        }

        private bool _showFloatingWindowsInTaskbar;
        public bool ShowFloatingWindowsInTaskbar
        {
            get { return _showFloatingWindowsInTaskbar; }
            set
            {
                _showFloatingWindowsInTaskbar = value;
                NotifyOfPropertyChange(() => ShowFloatingWindowsInTaskbar);
                if (_shellView != null)
                    _shellView.UpdateFloatingWindows();
            }
        }

        public virtual string StateFile
        {
            get { return @".\ApplicationState.bin"; }
        }

        public bool HasPersistedState
        {
            get { return File.Exists(StateFile); }
        }

        public ShellViewModel()
        {
            ((IActivate)this).Activate();

            _tools = new BindableCollection<ITool>();
        }

        protected override void OnViewLoaded(object view)
        {
            foreach (var module in _modules)
                foreach (var globalResourceDictionary in module.GlobalResourceDictionaries)
                    Application.Current.Resources.MergedDictionaries.Add(globalResourceDictionary);

            foreach (var module in _modules)
                module.PreInitialize();
            foreach (var module in _modules)
                module.Initialize();

            if (_themeManager.CurrentTheme == null)
            {
                if (!_themeManager.SetCurrentTheme(Properties.Settings.Default.ThemeName))
                {
                    Properties.Settings.Default.ThemeName = (string)Properties.Settings.Default.Properties["ThemeName"].DefaultValue;
                    Properties.Settings.Default.Save();
                    if (!_themeManager.SetCurrentTheme(Properties.Settings.Default.ThemeName))
                    {
                        throw new InvalidOperationException("unable to load application theme");
                    }
                }
            }

            _shellView = (IShellView)view;
            if (!_layoutItemStatePersister.LoadState(this, _shellView, StateFile))
            {
                foreach (var defaultDocument in _modules.SelectMany(x => x.DefaultDocuments))
                    OpenDocument(defaultDocument);
                foreach (var defaultTool in _modules.SelectMany(x => x.DefaultTools))
                    ShowTool((ITool)IoC.GetInstance(defaultTool, null));
            }

            foreach (var module in _modules)
                module.PostInitialize();

            base.OnViewLoaded(view);
        }

        public void ShowTool<TTool>()
            where TTool : ITool
        {
            ShowTool(IoC.Get<TTool>());
        }

        public void ShowTool(ITool model)
        {
            if (Tools.Contains(model))
                model.IsVisible = true;
            else
                Tools.Add(model);
            model.IsSelected = true;
            ActiveLayoutItem = model;
        }

        public void OpenDocument(IDocument model)
        {
            ActivateItem(model);
        }

        public void CloseDocument(IDocument document)
        {
            DeactivateItem(document, true);
        }

        private bool _activateItemGuard = false;

        public override void ActivateItem(IDocument item)
        {
            if (_closing || _activateItemGuard)
                return;

            _activateItemGuard = true;

            try
            {
                if (ReferenceEquals(item, ActiveItem))
                    return;

                RaiseActiveDocumentChanging();

                var currentActiveItem = ActiveItem;

                base.ActivateItem(item);

                RaiseActiveDocumentChanged();
            }
            finally
            {
                _activateItemGuard = false;
            }
        }

        private void RaiseActiveDocumentChanging()
        {
            ActiveDocumentChanging?.Invoke(this, EventArgs.Empty);
        }

        private void RaiseActiveDocumentChanged()
        {
            ActiveDocumentChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnActivationProcessed(IDocument item, bool success)
        {
            if (!ReferenceEquals(ActiveLayoutItem, item))
                ActiveLayoutItem = item;

            base.OnActivationProcessed(item, success);
        }

        public override void DeactivateItem(IDocument item, bool close)
        {
            RaiseActiveDocumentChanging();

            base.DeactivateItem(item, close);

            RaiseActiveDocumentChanged();
        }

        protected override void OnDeactivate(bool close)
        {
            _closing = true;

            _layoutItemStatePersister.SaveState(this, _shellView, StateFile);

            base.OnDeactivate(close);
        }

        public void Close()
        {
            Application.Current.MainWindow.Close();
        }
    }
}