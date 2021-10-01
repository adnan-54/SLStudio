using DevExpress.Mvvm;
using SLStudio.Core.Controls.StudioTextEditor.Resources;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;

namespace SLStudio.Core
{
    public class FindAndReplaceViewModel : BindableBase
    {
        private static readonly Regex currentWordRegex = new(@"([a-z0-9_])\w+", RegexOptions.IgnoreCase);

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
            FindReplace.FindTerm = GetSearchText();
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
                return string.Format(TextEditorResources.label_searchResult_format_singular, result);

            return string.Format(TextEditorResources.label_searchResult_format_plural, result);
        }

        private string GetSearchText()
        {
            if (textEditor.SelectionLength > 0)
                return textEditor.SelectedText;

            var caretOffset = textEditor.CaretOffset;
            var currentLine = textEditor.Document.GetLineByOffset(caretOffset);
            var currentLineText = textEditor.Document.GetText(currentLine.Offset, currentLine.Length);
            caretOffset -= currentLine.Offset;

            if (currentLine != null)
            {
                var matches = currentWordRegex.Matches(currentLineText);
                Match targetWord = null;
                if (caretOffset == currentLine.Length)
                    targetWord = matches.FirstOrDefault(m => caretOffset >= m.Index && caretOffset <= (m.Index + m.Length));
                else
                    targetWord = matches.FirstOrDefault(m => caretOffset >= m.Index && caretOffset < (m.Index + m.Length));

                if (targetWord != null)
                    return targetWord.Value;
            }

            if (caretOffset >= currentLineText.Length)
                return currentLineText[^1].ToString();

            return currentLineText[caretOffset].ToString();
        }

        private void FindService_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FindReplace.IsSearching))
                SearchResult = BuildSearchResult();
        }
    }
}