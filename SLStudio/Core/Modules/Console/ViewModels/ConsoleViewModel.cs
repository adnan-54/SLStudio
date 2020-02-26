using Caliburn.Micro;
using ICSharpCode.AvalonEdit.Document;
using SLStudio.Core.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SLStudio.Core.Modules.Console.ViewModels
{
    internal class ConsoleViewModel : Screen, IConsole, IHandle<NewLogRequestedEvent>
    {
        private bool wordWrap;

        public ConsoleViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.SubscribeOnPublishedThread(this);

            TextDocument = new TextDocument { Text = string.Empty };

            DisplayName = "Console";
        }

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

        public void AppendLine(string sender, string message)
        {
            TextDocument.Text = $"{Text}<{sender}>: {message}{Environment.NewLine}";
        }

        public void ClearText()
        {
            TextDocument.Text = string.Empty;
        }

        public Task HandleAsync(NewLogRequestedEvent message, CancellationToken cancellationToken)
        {
            AppendLine(message.Sender, message.Message);
            return Task.FromResult(true);
        }
    }
}
