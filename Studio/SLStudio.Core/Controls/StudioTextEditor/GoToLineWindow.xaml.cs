using DevExpress.Mvvm;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Controls;

namespace SLStudio.Core.Controls.StudioTextEditor
{
    public partial class GoToLineWindow : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        private readonly int maxLine;
        private readonly int currentLine;
        private string lineNumber;
        private string error;

        public GoToLineWindow(int maxLine, int currentLine)
        {
            InitializeComponent();
            DataContext = this;
            this.maxLine = maxLine;
            this.currentLine = currentLine;
            LabelContent = string.Format("_Line number (1 - {0}):", maxLine);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => !string.IsNullOrEmpty(error);

        public string LabelContent { get; }

        public string LineNumber
        {
            get => lineNumber;
            set
            {
                if (LineNumber == value)
                    return;

                lineNumber = value;

                Validate();

                PropertyChanged?.Invoke(this, new(nameof(LineNumber)));
                ErrorsChanged?.Invoke(this, new(nameof(LineNumber)));
                PropertyChanged?.Invoke(this, new(nameof(HasErrors)));
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return new string[] { error };
        }

        internal bool TryGetResult(out int result)
        {
            if (int.TryParse(LineNumber, out result))
                return true;

            return result >= 1 && result <= maxLine;
        }

        private void Validate()
        {
            if (!int.TryParse(LineNumber, out var number))
            {
                error = "Value must be a number";
                return;
            }
            if (number < 1 || number > maxLine)
            {
                error = $"The number must be between 1 and {maxLine}";
                return;
            }

            error = string.Empty;
        }

        private void DialogWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            textbox.Text = currentLine.ToString();
            textbox.Focus();
            textbox.SelectAll();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}