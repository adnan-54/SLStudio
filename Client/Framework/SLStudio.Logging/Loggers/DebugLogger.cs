using System.Diagnostics;

namespace SLStudio.Logging
{
    internal class DebugLogger
    {
        private readonly IConfigurationService configurationService;

        public DebugLogger(ILogManager logManager, IConfigurationService configurationService)
        {
            this.configurationService = configurationService;

            logManager.LogCompleted += OnLogCompleted;
        }

        private void OnLogCompleted(object sender, LogCompletedEventArgs e)
        {
            if (configurationService.LogToDebug)
                Debug.WriteLine(e.Log.StringRepresentation);
        }
    }
}