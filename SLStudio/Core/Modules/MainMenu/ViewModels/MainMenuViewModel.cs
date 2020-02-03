using Caliburn.Micro;

namespace SLStudio.Core.Modules.MainMenu.ViewModels
{
    internal class MainMenuViewModel : Screen, IMainMenu
    {
        private readonly IWindowManager windowManager;

        public MainMenuViewModel(IWindowManager windowManager)
        {
            this.windowManager = windowManager;
        }

        public void OpenConsole()
        {
            var console = IoC.Get<IConsole>();
            windowManager.ShowWindow(console);
        }

        public void OpenOptions()
        {
            var options = IoC.Get<IOptions>();
            windowManager.ShowDialog(options);
        }
    }
}