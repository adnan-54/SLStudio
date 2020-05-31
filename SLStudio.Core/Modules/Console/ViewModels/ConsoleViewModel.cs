using DevExpress.Mvvm;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Search;
using SLStudio.Core.Events;
using System;
using System.Windows;

namespace SLStudio.Core.Modules.Console.ViewModels
{
    internal class ConsoleViewModel : ViewModel, IConsole
    {
        private bool editorLoaded = false;

        public ConsoleViewModel(IMessenger messenger)
        {
            TextDocument = new TextDocument();

            messenger.Register<LogCompletedEvent>(this, OnLogCompleted);

            DisplayName = "Console";
        }

        public TextDocument TextDocument { get; }

        public bool WordWrap
        {
            get => GetProperty(() => WordWrap);
            set => SetProperty(() => WordWrap, value);
        }

        public void OnLogCompleted(LogCompletedEvent e)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                TextDocument.Text += $"{e.Log}{Environment.NewLine}";
            });
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
    }

    internal interface IConsole
    {
    }
}