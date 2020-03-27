using SLStudio.Core.Modules.Logger.ViewModels;

namespace SLStudio.Core.Modules.MainMenu.ViewModels
{
    internal class MainMenuViewModel : ViewModel, IMainMenu
    {
        private readonly IWindowManager windowManager;

        public MainMenuViewModel(IWindowManager windowManager)
        {
            this.windowManager = windowManager;
        }

        public void ViewLogs()
        {
            windowManager.ShowDialog<ILogsView>();
        }
    }
}
