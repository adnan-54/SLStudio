using Caliburn.Micro;
using SLStudio.Studio.Core.Enums;
using System.ComponentModel.Composition;
using System.Windows;

namespace SLStudio.Studio.Core.Modules.MainWindow.ViewModels
{
    [Export(typeof(IMainWindow))]
    class MainWindowViewModel : Conductor<IShell>, IMainWindow, IPartImportsSatisfiedNotification
    {

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

        public MainWindowStatus Status { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

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
