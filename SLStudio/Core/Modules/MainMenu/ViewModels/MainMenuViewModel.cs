using Caliburn.Micro;

namespace SLStudio.Core.Modules.MainMenu.ViewModels
{
    internal class MainMenuViewModel : Screen, IMainMenu
    {
        private readonly IWindowManager windowManager;
        private readonly IObjectFactory objectFactory;

        public MainMenuViewModel(IWindowManager windowManager, IObjectFactory objectFactory)
        {
            this.windowManager = windowManager;
            this.objectFactory = objectFactory;
        }

        //Tools
        public void OpenConsole()
        {
            var console = objectFactory.Create<IConsole>();
            windowManager.ShowWindow(console);
        }

        public void OpenOptions()
        {
            var options = objectFactory.Create<IOptions>();
            windowManager.ShowDialog(options);
        }
    }
}