using Caliburn.Micro;
using SLStudio.Core.Modules.Options.ViewModels;

namespace SLStudio.Core.Modules.MainWindow.ViewModels
{
    internal class MainWindowViewModel : Screen, IMainWindow
    {
        private readonly IWindowManager windowManager;
        private readonly IEventAggregator eventAggregator;

        public MainWindowViewModel(IWindowManager windowManager, IEventAggregator eventAggregator, IToolbar toolbar, IShell shell, IStatusBar statusBar)
        {
            this.windowManager = windowManager;
            this.eventAggregator = eventAggregator;
            Toolbar = toolbar;
            Shell = shell;
            StatusBar = statusBar;

            IsBusy = false;

            DisplayName = "SLStudio";
        }

        private bool isBusy;

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        private IToolbar toolbar;

        public IToolbar Toolbar
        {
            get => toolbar;
            set
            {
                toolbar = value;
                NotifyOfPropertyChange(() => Toolbar);
            }
        }

        public IShell Shell { get; }

        private IStatusBar statusBar;

        public IStatusBar StatusBar
        {
            get => statusBar;
            set
            {
                statusBar = value;
                NotifyOfPropertyChange(() => StatusBar);
            }
        }

        public void ShowOptions()
        {
            windowManager.ShowDialog(new OptionsViewModel());
        }
    }
}