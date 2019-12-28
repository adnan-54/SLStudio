using Caliburn.Micro;
using SLStudio.Core.Framework;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SLStudio.Core.Modules.MainWindow.ViewModels
{
    class MainWindowViewModel : Screen, IMainWindow
    {
        public MainWindowViewModel(IToolbar toolbar, IShell shell, IStatusBar statusBar)
        {
            Toolbar = toolbar;
            Shell = shell;
            StatusBar = statusBar;

            WindowState = WindowState.Maximized;
            StudioState = MainWindowState.Idle;
            IsBusy = false;
            
            DisplayName = "SLStudio";
        }

        private WindowState windowState;
        public WindowState WindowState
        {
            get => windowState;
            set
            {
                windowState = value;
                NotifyOfPropertyChange(() => WindowState);
            }
        }

        private MainWindowState studioState;
        public MainWindowState StudioState
        {
            get => studioState;
            set
            {
                studioState = value;
                NotifyOfPropertyChange(() => StudioState);
                UpdateStatusColor();
            }
        }

        private Brush stateColor;
        public Brush StateColor
        {
            get => stateColor;
            set
            {
                stateColor = value;
                NotifyOfPropertyChange(() => StateColor);
            }
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

        private void UpdateStatusColor()
        {
            switch (StudioState)
            {
                case MainWindowState.Idle:
                    StateColor = new SolidColorBrush(Color.FromRgb(0, 122, 204));
                    break;
                case MainWindowState.Running:
                    StateColor = new SolidColorBrush(Color.FromRgb(202, 81, 0));
                    break;
                default:
                    StateColor = new SolidColorBrush(Color.FromRgb(104, 33, 122));
                    break;
            }
        }

        public void Test()
        {
            MessageBox.Show("Testado");
        }
    }
}
