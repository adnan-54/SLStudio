using Caliburn.Micro;

namespace SLStudio.Core.Modules.Shell.ViewModels
{
    internal class ShellViewModel : Screen, IShell
    {
        private readonly ILogger logger;

        public ShellViewModel(ILoggingFactory loggingFactory)
        {
            logger = loggingFactory.GetLoggerFor<ShellViewModel>();
        }

        public void Test()
        {
            throw new System.Exception();
        }
    }
}