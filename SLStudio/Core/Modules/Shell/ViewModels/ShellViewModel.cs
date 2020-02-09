using Caliburn.Micro;
using System.Threading.Tasks;

namespace SLStudio.Core.Modules.Shell.ViewModels
{
    internal class ShellViewModel : Screen, IShell
    {
        private readonly IEventAggregator eventAggregator;
        private readonly ILogger logger;

        public ShellViewModel(IEventAggregator eventAggregator, ILoggingFactory loggingFactory)
        {
            this.eventAggregator = eventAggregator;
            logger = loggingFactory.GetLoggerFor<ShellViewModel>();
        }

        public async void LogSomething()
        {
            await Task.Run(async () =>
            {
                for (int i = 0; i < 100; i++)
                    logger.Debug($"debug {i}");
            });
        }
    }
}