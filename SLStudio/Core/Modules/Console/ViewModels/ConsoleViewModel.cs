using Caliburn.Micro;
using Humanizer;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using SLStudio.Core.Events;
using SLStudio.Core.Helpers;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace SLStudio.Core.Modules.Console.ViewModels
{
    internal class ConsoleViewModel : Screen, IConsole, IHandle<NewLogRequestedEvent>
    {
        private readonly DispatcherTimer timer;

        private bool wordWrap;

        private string status;
        private double fontSize;
        private TextEditor editor;

        public ConsoleViewModel(IEventAggregator eventAggregator, ICommandLineArguments commandLineArguments)
        {
            eventAggregator.SubscribeOnPublishedThread(this);

            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            timer.Tick += OnTimerTick;

            SaveContentCommand = new CommandHandler(SaveContent, () => true);
            ResetFontSizeCommand = new CommandHandler(ResetFontSize, () => true);
            ShowDebuggingModeOptions = commandLineArguments.DebuggingMode;
            TextDocument = new TextDocument { Text = string.Empty };
            FontSize = Resources.ConsoleSettings.Default.FontSize;
            WordWrap = Resources.ConsoleSettings.Default.WordWrap;

            PropertyChanged += OnPropertyChanged;

            DisplayName = "Console";
        }

        public ICommand SaveContentCommand { get; }

        public ICommand SearchCommand { get; }

        public ICommand ClearTextCommand { get; }

        public bool WordWrap
        {
            get => wordWrap;
            set
            {
                wordWrap = value;
                Resources.ConsoleSettings.Default.WordWrap = wordWrap;
                NotifyOfPropertyChange(() => WordWrap);

                if (WordWrap)
                    Status = Resources.ConsoleResources.WordWrapActivated;
                else
                    Status = Resources.ConsoleResources.WordWrapDeactivated;
            }
        }

        public bool ShowDebuggingModeOptions { get; }



        public ICommand ResetFontSizeCommand { get; }


        public TextDocument TextDocument { get; }

        public string Text => TextDocument.Text;



        public string Status
        {
            get => status;
            set
            {
                if (status == value)
                    return;

                status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }

        public double FontSize
        {
            get => fontSize;
            set
            {
                fontSize = value;
                NotifyOfPropertyChange(() => FontSize);
                Status = string.Format(Resources.ConsoleResources.FontSizeSetToXPx, FontSize);

                Resources.ConsoleSettings.Default.FontSize = FontSize;
                Resources.ConsoleSettings.Default.Save();
            }
        }

        public string FileSize
        {
            get
            {
                if (editor == null || Text == null)
                {
                    return (0).Bytes().Humanize("KB");
                }

                return (Text.Length * sizeof(char) + sizeof(int)).Bytes().Humanize("KB");
            }
        }

        public int? CurrentLine => editor?.Document?.GetLineByOffset(editor.CaretOffset)?.LineNumber;

        public int? CurrentColumn => editor?.TextArea?.Caret.VisualColumn + 1;

        public int? TotalLines => editor?.Document?.LineCount;

        public int? TotalLength => editor?.Document?.TextLength;

        public void AppendLine(string sender, string message)
        {
            TextDocument.Text = $"{Text}<{sender}>: {message}{Environment.NewLine}";
            Status = Resources.ConsoleResources.NewLineAppendedSuccessfully;
        }

        public void ClearText()
        {
            TextDocument.Text = string.Empty;
            Status = Resources.ConsoleResources.TextClearedSuccessfully;
        }

        public void SaveContent()
        {
            if (string.IsNullOrEmpty(Text))
            {
                return;
            }

            using (System.Windows.Forms.SaveFileDialog saveFile = new System.Windows.Forms.SaveFileDialog())
            {
                saveFile.Filter = "txt file (*.txt)|*.txt";
                saveFile.CheckPathExists = true;
                saveFile.FileName = $"slstudio_console_{System.DateTime.Now.ToString().Replace('/', '-').Replace(' ', '_').Replace(':', '-')}";

                if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string text = Text;
                    Task.Run(() =>
                    {
                        File.WriteAllText(saveFile.FileName, text);
                        Status = Resources.ConsoleResources.FileSavedSuccessfully;
                    });
                }
            }
        }

        public void ResetFontSize()
        {
            FontSize = 14;
            Status = Resources.ConsoleResources.FontSizeReset;
        }

        public void OnEditorLoaded(TextEditor editor)
        {
            this.editor = editor;
            editor.TextArea.Caret.PositionChanged += OnCarretPositionChanged;
            editor.TextChanged += OnTextChanged;
        }

        private void OnCarretPositionChanged(object sender, EventArgs e)
        {
            NotifyOfPropertyChange(() => CurrentLine);
            NotifyOfPropertyChange(() => CurrentColumn);
        }

        private void OnTextChanged(object sender, EventArgs e)
        {
            NotifyOfPropertyChange(() => TotalLines);
            NotifyOfPropertyChange(() => TotalLength);
            NotifyOfPropertyChange(() => FileSize);
        }

        public void DebugInserRandomString()
        {
            string randomSender = StringHelpers.RandomClass();
            string randomMessage = StringHelpers.LoremIpsum();
            AppendLine(randomSender, randomMessage);

            Status = Resources.ConsoleResources.RandomStringInsertedSuccessfully;
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            Status = Resources.ConsoleResources.Ready;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Status) && !Status.Equals(Resources.ConsoleResources.Ready, StringComparison.InvariantCultureIgnoreCase))
            {
                timer.Stop();
                timer.Start();
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            timer.Stop();
            Status = Resources.ConsoleResources.Ready;
        }

        public override Task TryCloseAsync(bool? dialogResult = null)
        {
            PropertyChanged -= OnPropertyChanged;
            timer.Tick -= OnTimerTick;
            editor.TextChanged -= OnTextChanged;
            editor.TextArea.Caret.PositionChanged -= OnCarretPositionChanged;
            return base.TryCloseAsync(dialogResult);
        }

        public Task HandleAsync(NewLogRequestedEvent message, CancellationToken cancellationToken)
        {
            AppendLine(message.Sender, message.Message);
            return Task.FromResult(true);
        }
    }
}