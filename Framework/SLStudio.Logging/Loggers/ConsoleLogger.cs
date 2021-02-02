using System;

namespace SLStudio.Logging
{
    internal class ConsoleLogger
    {
        private readonly IConfigurationService configurationService;

        public ConsoleLogger(ILogManager logManager, IConfigurationService configurationService)
        {
            this.configurationService = configurationService;

            logManager.LogCompleted += OnLogCompleted;
        }

        private void OnLogCompleted(object sender, LogCompletedEventArgs e)
        {
            if (configurationService.LogToConsole)
                Console.WriteLine(e.Log.StringRepresentation);
        }
    }
}