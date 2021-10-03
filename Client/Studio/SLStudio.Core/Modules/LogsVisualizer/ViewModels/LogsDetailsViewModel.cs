using SLStudio.Core.Modules.LogsVisualizer.Resources;
using SLStudio.Logging;

namespace SLStudio.Core.Modules.LogsVisualizer.ViewModels
{
    internal class LogsDetailsViewModel : WindowViewModel
    {
        private readonly Log log;

        public LogsDetailsViewModel(Log log)
        {
            this.log = log;

            DisplayName = LogsVisualizerResources.window_title_Details;
        }

        public int Id => log.Id;
        public string Sender => log.Sender;
        public string Level => log.Level;
        public string Date => log.Date;
        public string Title => log.Title;
        public string Message => log.Message;
        public string StackTrace => log.StackTrace;
        public string CalledFrom => $"{log.CallerFile}: line {log.CallerLine}, {log.CallerMember}";
    }
}