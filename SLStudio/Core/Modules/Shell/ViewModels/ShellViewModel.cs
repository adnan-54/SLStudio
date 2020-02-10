using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;

namespace SLStudio.Core.Modules.Shell.ViewModels
{
    internal class ShellViewModel : Screen, IShell
    {
        private readonly IEventAggregator eventAggregator;
        private readonly IDialogCoordinator dialogCoordinator;
        private readonly ILogger logger;

        public ShellViewModel(IEventAggregator eventAggregator, ILoggingFactory loggingFactory, IDialogCoordinator dialogCoordinator)
        {
            this.eventAggregator = eventAggregator;
            this.dialogCoordinator = dialogCoordinator;
            logger = loggingFactory.GetLoggerFor<ShellViewModel>();
        }

        public void LogSomething()
        {
            for (int i = 0; i < 100; i++)
                logger.Debug($"debug {i}");
        }
    }
}