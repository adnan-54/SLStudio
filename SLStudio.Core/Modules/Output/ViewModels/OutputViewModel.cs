using SLStudio.Core.Docking;
using SLStudio.Core.Logging;
using System;

namespace SLStudio.Core.Modules.Output.ViewModels
{
    internal class OutputViewModel : ToolBase, IOutput
    {
        public OutputViewModel()
        {
            DisplayName = "Output";
            LogManager.LoggingService.LogCompleted += OnLogCompleted;
        }

        public override PaneLocation PreferredLocation => PaneLocation.Bottom;

        public string Text
        {
            get => GetProperty(() => Text);
            set => SetProperty(() => Text, value);
        }

        public bool WordWrap
        {
            get => GetProperty(() => WordWrap);
            set => SetProperty(() => WordWrap, value);
        }

        public void AppendLine(string content)
        {
            Text = $"{Text}{content}{Environment.NewLine}";
        }

        public void Clear()
        {
            Text = string.Empty;
        }

        private void OnLogCompleted(object sender, LogCompletedEventArgs e)
        {
            AppendLine(e.Log.ToString());
        }
    }
}