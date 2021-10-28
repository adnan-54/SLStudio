using DevExpress.Mvvm.UI;
using System.Windows;

namespace SLStudio
{
    internal class WindowService : ServiceBaseGeneric<Window>, IWindowService
    {
        private readonly IUiSynchronization uiSynchronization;

        public WindowService(IUiSynchronization uiSynchronization)
        {
            this.uiSynchronization = uiSynchronization;
        }

        public void Activate()
        {
            uiSynchronization.Execute(() =>
            {
                if (AssociatedObject.WindowState == WindowState.Minimized)
                    Restore();

                AssociatedObject.Focus();
            });
        }

        public void Maximize()
        {
            SystemCommands.MaximizeWindow(AssociatedObject);
        }

        public void Restore()
        {
            SystemCommands.RestoreWindow(AssociatedObject);
        }

        public void Minimize()
        {
            SystemCommands.MinimizeWindow(AssociatedObject);
        }

        public void Close()
        {
            SystemCommands.CloseWindow(AssociatedObject);
        }
    }
}