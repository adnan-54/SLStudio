using ICSharpCode.AvalonEdit.Document;
using SLStudio.Core.Behaviors;
using SLStudio.Core.Modules.Output.Resources;
using SLStudio.Logging;
using System;

namespace SLStudio.Core.Modules.Output.ViewModels
{
    internal class OutputViewModel : ToolPanelBase, IOutput
    {
        private IAvalonEditSearch avalonEditSearch;

        public OutputViewModel()
        {
            DisplayName = OutputResource.Output;
            TextDocument = new TextDocument();
            LogManager.LogCompleted += OnLogCompleted;
        }

        public override ToolPlacement Placement => ToolPlacement.Bottom;

        public TextDocument TextDocument { get; }

        public void OnLoaded()
        {
            avalonEditSearch = GetService<IAvalonEditSearch>();
        }

        public bool WordWrap
        {
            get => GetProperty(() => WordWrap);
            set => SetProperty(() => WordWrap, value);
        }

        public void AppendLine(string text)
        {
            TextDocument.Text = $"{TextDocument.Text}{text}{Environment.NewLine}";
        }

        public void Clear()
        {
            TextDocument.Text = string.Empty;
        }

        public void ToggleWordWrap()
        {
            WordWrap = !WordWrap;
        }

        public void QuickFind()
        {
            avalonEditSearch?.Find();
        }

        public void FindNext()
        {
            avalonEditSearch?.FindNext();
        }

        public void FindPrevious()
        {
            avalonEditSearch?.FindPrevious();
        }

        private void OnLogCompleted(object sender, LogCompletedEventArgs e)
        {
            AppendLine($"{e.Log}");
        }
    }
}