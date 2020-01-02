using Caliburn.Micro;
using System.Threading.Tasks;

namespace SLStudio.Core.Modules.Console.ViewModels
{
    class ConsoleViewModel : Screen, IConsole
    {
        public ConsoleViewModel()
        {
            DisplayName = "SLStudio - Console";
        }

        private string text = string.Empty;
        public string Text
        {
            get => text;
            set
            {
                text = value;
                NotifyOfPropertyChange(() => Text);
            }
        }

        private bool textWrapping;
        public bool TextWrapping
        {
            get => textWrapping;
            set
            {
                textWrapping = value;
                NotifyOfPropertyChange(() => TextWrapping);
            }
        }

        public void AppendLine(string sender, string message)
        {
            if (string.IsNullOrEmpty(Text.Trim()))
                Text = $">{sender}: {message}";
            else
                Text = $"{text}\n>{sender}: {message}";
        }

        public void Clear()
        {
            Text = string.Empty;
        }

        public void ToggleTextWrapping()
        {
            TextWrapping = !TextWrapping;
        }
    }
}
