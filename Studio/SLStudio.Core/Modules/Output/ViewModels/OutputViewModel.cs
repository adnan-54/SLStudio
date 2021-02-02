using DevExpress.Mvvm;
using ICSharpCode.AvalonEdit.Document;
using SLStudio.Core.Behaviors;
using SLStudio.Core.Modules.Output.Resources;
using SLStudio.Logging;
using System;

namespace SLStudio.Core.Modules.Output.ViewModels
{
    internal class OutputViewModel : ToolBase, IOutput
    {
        private readonly IUiSynchronization uiSynchronization;

        public OutputViewModel(ILogManager logManager, IUiSynchronization uiSynchronization)
        {
            this.uiSynchronization = uiSynchronization;
            TextDocument = new TextDocument();

            CanSetContent = false;
            DisplayName = OutputResource.Output;

            logManager.LogCompleted += OnLogCompleted;
        }

        public override WorkspaceItemPlacement Placement => WorkspaceItemPlacement.Bottom;

        public TextDocument TextDocument { get; }

        public IAvalonEditSearch AvalonEditSearch => GetService<IAvalonEditSearch>();

        public bool WordWrap
        {
            get => GetProperty(() => WordWrap);
            set => SetProperty(() => WordWrap, value);
        }

        public void AppendLine(string text)
        {
            uiSynchronization.EnsureExecuteOnUiAsync(() => TextDocument.Text = $"{TextDocument.Text}{text}{Environment.NewLine}").FireAndForget();
        }

        public void Clear()
        {
            uiSynchronization.EnsureExecuteOnUiAsync(() => TextDocument.Text = TextDocument.Text = string.Empty).FireAndForget();
        }

        public void ToggleWordWrap()
        {
            WordWrap = !WordWrap;
        }

        public void QuickFind()
        {
            AvalonEditSearch?.Find();
        }

        public void FindNext()
        {
            AvalonEditSearch?.FindNext();
        }

        public void FindPrevious()
        {
            AvalonEditSearch?.FindPrevious();
        }

        private void OnLogCompleted(object sender, LogCompletedEventArgs e)
        {
            AppendLine($"{e.Log}");
        }
    }
}