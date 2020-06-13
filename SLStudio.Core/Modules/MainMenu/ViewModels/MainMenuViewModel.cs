using Caliburn.Micro;
using SLStudio.Core.Modules.Console.ViewModels;
using SLStudio.Core.Modules.Logger.ViewModels;

namespace SLStudio.Core.Modules.MainMenu.ViewModels
{
    internal class MainMenuViewModel : ViewModel, IMainMenu
    {
        private readonly IWindowManager windowManager;
        private readonly IOutput output;

        public MainMenuViewModel(IWindowManager windowManager, IOutput output)
        {
            this.windowManager = windowManager;
            this.output = output;
        }

        public void ViewLogs()
        {
            windowManager.ShowDialog<ILogsView>();
        }

        public void ViewConsole()
        {
            windowManager.ShowWindow<IConsole>();
        }

        public void ViewOutput()
        {
            IoC.Get<IShell>()?.Activate(output);
        }
    }
}