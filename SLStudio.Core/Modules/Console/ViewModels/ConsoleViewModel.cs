using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Search;
using SLStudio.Core.Logging;
using System;

namespace SLStudio.Core.Modules.Console.ViewModels
{
    internal class ConsoleViewModel : ViewModel, IConsole
    {
        private bool editorLoaded;

        public ConsoleViewModel()
        {
            editorLoaded = false;
            TextDocument = new TextDocument();
            LogManager.LoggingService.LogCompleted += OnLogCompleted;
            DisplayName = "Console";
        }

        public TextDocument TextDocument { get; }

        public bool WordWrap
        {
            get => GetProperty(() => WordWrap);
            set => SetProperty(() => WordWrap, value);
        }

        public void EditorOnLoad(TextEditor editor)
        {
            if (editorLoaded)
                return;
            editorLoaded = true;

            SearchPanel.Install(editor.TextArea);
        }

        public void ClearAll()
        {
            TextDocument.Text = string.Empty;
        }

        private void OnLogCompleted(object sender, LogCompletedEventArgs e)
        {
            TextDocument.Text = $"{TextDocument.Text}{e.Log}{Environment.NewLine}";
        }
    }

    internal interface IConsole
    {
    }
}