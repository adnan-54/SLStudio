using Caliburn.Micro;
using System.ComponentModel.Composition;
using System.Windows;

namespace SLStudio.Core.Modules.MainWindow.ViewModels
{
    [Export(typeof(IMainWindow))]
    public class MainWindowViewModel : Conductor<IShell>, IMainWindow, IPartImportsSatisfiedNotification
    {
        [Import]
        private IShell shell;
        public IShell Shell
        {
            get { return shell; }
        }

        [Import]
        private IResourceManager resourceManager;

        [Import]
        private ICommandKeyGestureService commandKeyGestureService;
        
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

        private double width = 1280.0;
        public double Width
        {
            get { return width; }
            set
            {
                width = value;
                NotifyOfPropertyChange(() => Width);
            }
        }

        private double height = 800.0;
        public double Height
        {
            get { return height; }
            set
            {
                height = value;
                NotifyOfPropertyChange(() => Height);
            }
        }

        void IPartImportsSatisfiedNotification.OnImportsSatisfied()
        {
            ActivateItem(shell);
        }

        protected override void OnViewLoaded(object view)
        {
            commandKeyGestureService.BindKeyGestures((UIElement)view);
            base.OnViewLoaded(view);
        }
    }
}
