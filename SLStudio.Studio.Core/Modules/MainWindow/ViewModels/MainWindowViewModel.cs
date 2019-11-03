using Caliburn.Micro;
using SLStudio.Studio.Core.Enums;
using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Media;

namespace SLStudio.Studio.Core.Modules.MainWindow.ViewModels
{
    [Export(typeof(IMainWindow))]
    class MainWindowViewModel : Conductor<IShell>, IMainWindow, IPartImportsSatisfiedNotification
    {
        public MainWindowViewModel()
        {
            UpdateStatusColor();
        }

        [Import]
        private IShell shell;
        public IShell Shell
        {
            get { return shell; }
            set
            {
                shell = value;
                NotifyOfPropertyChange(() => Shell);
            }
        }

        private Brush statusColor;
        public Brush StatusColor
        {
            get { return statusColor; }
            set
            {
                statusColor = value;
                NotifyOfPropertyChange(() => StatusColor);

            }
        }

        private WindowState windowState = WindowState.Maximized;
        public WindowState WindowState
        {
            get { return windowState; }
            set
            {
                windowState = value;
                NotifyOfPropertyChange(() => WindowState);
            }
        }

        private string title = "SLStudio";
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

        private MainWindowStatus status = MainWindowStatus.Idle;
        public MainWindowStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                NotifyOfPropertyChange(() => Status);
                UpdateStatusColor();
            }
        }

        private void UpdateStatusColor()
        {
            Color color = new Color();
            switch(status)
            {
                case (MainWindowStatus.Busy):
                {
                    color.R = 104;
                    color.G = 33;
                    color.B = 122;
                    color.A = 255;

                    StatusColor = new SolidColorBrush(color);
                    break;
                }
                case (MainWindowStatus.Running):
                {
                    color.R = 202;
                    color.G = 81;
                    color.B = 0;
                    color.A = 255;

                    StatusColor = new SolidColorBrush(color);
                    break;
                }
                default:
                {
                    color.R = 0;
                    color.G = 122;
                    color.B = 204;
                    color.A = 255;

                    StatusColor = new SolidColorBrush(color);
                    break;
                }
            }
        }

        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            ActivateItem(shell);
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
        }
    }
}
