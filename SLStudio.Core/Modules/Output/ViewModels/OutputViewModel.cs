using DevExpress.Mvvm;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Search;
using SLStudio.Core.Docking;
using SLStudio.Core.Events;
using System;
using System.Windows;

namespace SLStudio.Core.Modules.Output.ViewModels
{
    internal class OutputViewModel : ToolBase, IOutput
    {
        private bool editorLoaded;

        public OutputViewModel(IMessenger messenger)
        {
            TextDocument = new TextDocument();
            editorLoaded = false;
            messenger.Register<LogCompletedEvent>(this, OnLogCompleted);
            DisplayName = "Output";
        }

        public override PaneLocation PreferredLocation => PaneLocation.Bottom;

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

        public void AppendLine(string content)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                TextDocument.Text = $"{TextDocument.Text}{content}{Environment.NewLine}";
            });
        }

        public void Clear()
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                TextDocument.Text = string.Empty;
            });
        }

        private void OnLogCompleted(LogCompletedEvent e)
        {
            AppendLine(e.Log.ToString());
        }
    }
}