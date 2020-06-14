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
        public OutputViewModel(IMessenger messenger)
        {
            messenger.Register<LogCompletedEvent>(this, OnLogCompleted);
            DisplayName = "Output";
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

        private void OnLogCompleted(LogCompletedEvent e)
        {
            Application.Current?.Dispatcher.Invoke(() =>
            {
                AppendLine(e.Log.ToString());
            });
        }
    }
}