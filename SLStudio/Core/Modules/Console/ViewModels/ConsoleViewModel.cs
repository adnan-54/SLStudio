using Caliburn.Micro;
using SLStudio.Core.Events;
using System.Threading;
using System.Threading.Tasks;

namespace SLStudio.Core.Modules.Console.ViewModels
{
    internal class ConsoleViewModel : Screen, IConsole, IHandle<NewLogRequestedEvent>
    {
        private string text = string.Empty;
        private bool textWrapping;

        public ConsoleViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.SubscribeOnPublishedThread(this);
            DisplayName = "SLStudio - Console";
        }

        public string Text
        {
            get => text;
            set
            {
                text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }

        public bool TextWrapping
        {
            get => textWrapping;
            set
            {
                textWrapping = value;
                NotifyOfPropertyChange(() => TextWrapping);
            }
        }

        public string GetText() => Text;

        public void AppendLine(string sender, string message) => Text = $"{text}>{sender}: {message}\n";

        public void Clear() => Text = string.Empty;

        public void ToggleTextWrapping() => TextWrapping = !TextWrapping;

        public Task HandleAsync(NewLogRequestedEvent message, CancellationToken cancellationToken)
        {
            AppendLine(message.Sender, message.Message);
            return Task.FromResult(true);
        }
    }
}