using DevExpress.Mvvm;
using SLStudio.Core.Controls.StudioTextEditor.Views;
using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Linq;
using SLStudio.Core.Controls.StudioTextEditor.Resources;

namespace SLStudio.Core
{
    internal class GoToLineViewModel : BindableBase, INotifyDataErrorInfo
    {
        private readonly StudioTextEditor textEditor;
        private GoToLineView view;
        private string error;

        public GoToLineViewModel(StudioTextEditor textEditor)
        {
            this.textEditor = textEditor;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool IsValid => string.IsNullOrEmpty(error);

        public bool HasErrors => !IsValid;

        public int LastLine => textEditor.LineCount;

        public string LineNumber
        {
            get => GetProperty(() => LineNumber);
            set
            {
                SetProperty(() => LineNumber, value);

                Validate();
                RaisePropertyChanged(() => IsValid);
                ErrorsChanged?.Invoke(this, new(nameof(LineNumber)));
            }
        }

        public void Show()
        {
            LineNumber = $"{textEditor.CurrentLine}";

            var currentWindow = Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive);
            view = new GoToLineView
            {
                DataContext = this,
                Owner = currentWindow
            };

            view.ShowDialog();
        }

        public void Ok()
        {
            if (HasErrors || !TryGetResult(out var targetLine))
                return;

            textEditor.CurrentColumn = 0;
            textEditor.CurrentLine = targetLine;
            textEditor.ScrollToLine(targetLine);
            view.DialogResult = true;
        }

        public void Cancel()
        {
            view.DialogResult = false;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName == nameof(LineNumber))
                return new string[] { error };
            return Enumerable.Empty<string>();
        }

        public void OnLoaded()
        {
            view.textbox.Text = $"{textEditor.CurrentLine}";
            view.textbox.Focus();
            view.textbox.SelectAll();
        }

        private bool TryGetResult(out int result)
        {
            if (int.TryParse(LineNumber, out result))
                return true;

            return result >= 1 && result <= LastLine;
        }

        private void Validate()
        {
            if (!int.TryParse(LineNumber, out var number))
            {
                error = TextEditorResources.validation_valueMustBeNumber;
                return;
            }
            else
            if (number < 1 || number > LastLine)
            {
                error = string.Format(TextEditorResources.validation_numberMustBeInRange_format, LastLine);
                return;
            }

            error = string.Empty;
        }
    }
}