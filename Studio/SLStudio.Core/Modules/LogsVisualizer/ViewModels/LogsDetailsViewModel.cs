using SLStudio.Core.Modules.LogsVisualizer.Resources;
using System.Data;

namespace SLStudio.Core.Modules.LogsVisualizer.ViewModels
{
    internal class LogsDetailsViewModel : WindowViewModel
    {
        public LogsDetailsViewModel(DataRowView dataRow)
        {
            Id = dataRow.Row["id"].ToString();
            Sender = dataRow.Row["sender"].ToString();
            Level = dataRow.Row["level"].ToString();
            Date = dataRow.Row["date"].ToString();
            Title = dataRow.Row["title"].ToString();
            Message = dataRow.Row["message"].ToString();
            StackTrace = dataRow.Row["stacktrace"].ToString();
            CalledFrom = $"{dataRow.Row["callerfile"]}: line {dataRow.Row["callerline"]}, {dataRow.Row["callermember"]}";

            DisplayName = LogsVisualizerResources.window_title_Details;
        }

        public string Id { get; }
        public string Sender { get; }
        public string Level { get; }
        public string Date { get; }
        public string Title { get; }
        public string Message { get; }
        public string StackTrace { get; }
        public string CalledFrom { get; }
    }
}