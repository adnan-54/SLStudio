using Caliburn.Micro;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using SLStudio.Core.Events;
using SLStudio.Core.Helpers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLStudio.Core.Modules.Console.ViewModels
{
    internal class ConsoleViewModel : Screen, IConsole, IHandle<NewLogRequestedEvent>
    {
        private TextEditor editor;
        private bool wordWrap;
        private string status;

        public ConsoleViewModel(IEventAggregator eventAggregator, ICommandLineArguments commandLineArguments)
        {
            eventAggregator.SubscribeOnPublishedThread(this);
            ShowDebuggingModeOptions = commandLineArguments.DebuggingMode;
            TextDocument = new TextDocument { Text = string.Empty };
            DisplayName = "Console";
        }

        public bool ShowDebuggingModeOptions { get; }

        public TextDocument TextDocument { get; }

        public string Text => TextDocument.Text;

        public bool WordWrap
        {
            get => wordWrap;
            set
            {
                wordWrap = value;
                NotifyOfPropertyChange(() => WordWrap);
            }
        }

        public string Status
        {
            get => status;
            set
            {
                status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }

        public int? CurrentLine
        {
            get
            {
                if (editor == null)
                    return null;

                return editor.Document.GetLineByOffset(editor.CaretOffset).LineNumber;
            }
        }

        public int? CurrentColumn
        {
            get
            {
                if (editor == null)
                    return null;

                return editor.TextArea.Caret.VisualColumn + 1;
            }
        }

        public void AppendLine(string sender, string message)
        {
            TextDocument.Text = $"{Text}<{sender}>: {message}{Environment.NewLine}";
        }

        public void ClearText()
        {
            TextDocument.Text = string.Empty;
        }

        public void DebugInserRandomString()
        {
            var randomSender = StringHelpers.RandomClass();
            var randomMessage = StringHelpers.LoremIpsum();
            AppendLine(randomSender, randomMessage);
        }

        public Task HandleAsync(NewLogRequestedEvent message, CancellationToken cancellationToken)
        {
            AppendLine(message.Sender, message.Message);
            return Task.FromResult(true);
        }

        public void OnEditorLoaded(TextEditor editor)
        {
            this.editor = editor;
            editor.TextArea.Caret.PositionChanged += OnCarretPositionChanged;
        }

        private void OnCarretPositionChanged(object sender, EventArgs e)
        {
            NotifyOfPropertyChange(() => CurrentLine);
            NotifyOfPropertyChange(() => CurrentColumn);
        }

        public override Task TryCloseAsync(bool? dialogResult = null)
        {
            editor.TextArea.Caret.PositionChanged -= OnCarretPositionChanged;
            return base.TryCloseAsync(dialogResult);
        }
    }
}
