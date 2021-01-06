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
        private IAvalonEditSearch avalonEditSearch;

        public OutputViewModel(IUiSynchronization uiSynchronization)
        {
            this.uiSynchronization = uiSynchronization;
            TextDocument = new TextDocument();

            CanSetContent = false;
            DisplayName = OutputResource.Output;

            LogManager.LogCompleted += OnLogCompleted;
        }

        public override WorkspaceItemPlacement Placement => WorkspaceItemPlacement.Bottom;

        public TextDocument TextDocument { get; }

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

        public void OnLoaded()
        {
            avalonEditSearch = GetService<IAvalonEditSearch>();
        }

        private void OnLogCompleted(object sender, LogCompletedEventArgs e)
        {
            AppendLine($"{e.Log}");
        }
    }
}