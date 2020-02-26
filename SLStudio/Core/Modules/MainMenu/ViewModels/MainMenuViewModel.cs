using Caliburn.Micro;
using System.Windows.Input;

namespace SLStudio.Core.Modules.MainMenu.ViewModels
{
    internal class MainMenuViewModel : PropertyChangedBase, IMainMenu
    {
        private readonly IWindowManager windowManager;
        private readonly IObjectFactory objectFactory;

        public MainMenuViewModel(IWindowManager windowManager, IObjectFactory objectFactory)
        {
            this.windowManager = windowManager;
            this.objectFactory = objectFactory;
        }

        //View
        public ICommand ViewConsoleCommand => new CommandHandler(OpenConsole, () => true);

        private void OpenConsole()
        {
            var view = objectFactory.Create<IConsole>();
            windowManager.ShowWindowAsync(view).FireAndForget();
        }
    }
}
