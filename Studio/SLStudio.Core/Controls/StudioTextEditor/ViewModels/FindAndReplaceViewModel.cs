using DevExpress.Mvvm;
using System;
using System.ComponentModel;

namespace SLStudio.Core
{
    public class FindAndReplaceViewModel : BindableBase
    {
        private readonly StudioTextEditor textEditor;

        public FindAndReplaceViewModel(StudioTextEditor textEditor, IFindReplaceService findReplace)
        {
            this.textEditor = textEditor;
            FindReplace = findReplace;
            (FindReplace as INotifyPropertyChanged).PropertyChanged += FindService_OnPropertyChanged;
        }

        public event EventHandler FocusRequested;

        public bool IsOpen
        {
            get => GetProperty(() => IsOpen);
            set => SetProperty(() => IsOpen, value);
        }

        public string SearchResult
        {
            get => GetProperty(() => SearchResult);
            set => SetProperty(() => SearchResult, value);
        }

        public IFindReplaceService FindReplace { get; }

        public void Show()
        {
            IsOpen = true;
            FindReplace.ShowBackgroundRenderer();
            FocusRequested?.Invoke(this, EventArgs.Empty);
        }

        public void ShowFind()
        {
            Show();
            FindReplace.IsReplacing = false;
        }

        public void ShowFindReplace()
        {
            Show();
            FindReplace.IsReplacing = true;
        }

        public void Close()
        {
            IsOpen = false;
            FindReplace.HideBackgroundRenderer();
        }

        public void ToggleMatchCase()
        {
            FindReplace.MatchCase = !FindReplace.MatchCase;
        }

        public void ToggleWholeWord()
        {
            FindReplace.MatchWholeWord = !FindReplace.MatchWholeWord;
        }

        public void ToggleRegex()
        {
            FindReplace.UseRegex = !FindReplace.UseRegex;
        }

        private string BuildSearchResult()
        {
            var result = Math.Abs(FindReplace.ResultsCount);

            if (result == 0)
                return string.Empty;
            if (result == 1)
                return string.Format("{0} result found", result);

            return string.Format("{0} results found", result);
        }

        private string GetSearchText()
        {
            if (textEditor.SelectionLength > 0)
                return textEditor.SelectedText;
            af

            //regex pattern: ([a-z1-9_])\w+
            //flags: case insensitive
            var caretOffset = textEditor.CaretOffset;
            var currentLine = textEditor.Document.GetLineByOffset(caretOffset);

            if (currentLine != null)
            {
                var currentLineText = textEditor.Document.GetText(currentLine.Offset, currentLine.Length);
            }

            return string.Empty;
        }

        private void FindService_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FindReplace.IsSearching))
                SearchResult = BuildSearchResult();
        }
    }
}