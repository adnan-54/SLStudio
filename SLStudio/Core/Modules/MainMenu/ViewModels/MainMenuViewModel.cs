using Caliburn.Micro;
using SLStudio.Core.Modules.Logs.ViewModels;
using System;
using System.Linq;

namespace SLStudio.Core.Modules.MainMenu.ViewModels
{
    internal class MainMenuViewModel : Screen, IMainMenu
    {
        private readonly IWindowManager windowManager;
        private readonly IObjectFactory objectFactory;
        private readonly ILoggingService loggingService;

        public MainMenuViewModel(IWindowManager windowManager, IObjectFactory objectFactory, ILoggingService loggingService)
        {
            this.windowManager = windowManager;
            this.objectFactory = objectFactory;
            this.loggingService = loggingService;
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

        //Help
        public bool CanOpenLogs => loggingService.LogFileExists;
        public void OpenLogs()
        {
            var model = objectFactory.Create<LogsViewModel>();
            windowManager.ShowDialog(model);
        }

        public void UpdateCanExecute()
        {
            var properties = GetType().GetProperties().Where(prop => prop.Name.StartsWith("Can", StringComparison.InvariantCultureIgnoreCase));

            foreach (var property in properties)
                NotifyOfPropertyChange(property.Name);
        }
    }
}