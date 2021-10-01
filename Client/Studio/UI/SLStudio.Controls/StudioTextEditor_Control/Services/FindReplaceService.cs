using DevExpress.Mvvm;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace SLStudio.Core
{
    internal class FindReplaceService : BindableBase, IFindReplaceService, INotifyDataErrorInfo
    {
        private readonly StudioTextEditor textEditor;
        private readonly FindResultsColorizer resultsColorizer;
        private string error;

        public FindReplaceService(StudioTextEditor textEditor)
        {
            this.textEditor = textEditor;
            resultsColorizer = new FindResultsColorizer(this);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => UseRegex && !string.IsNullOrEmpty(error);

        public bool CanFind => !string.IsNullOrEmpty(FindTerm);

        public bool CanReplace => !string.IsNullOrEmpty(ReplaceTerm);

        public bool IsReplacing
        {
            get => GetProperty(() => IsReplacing);
            set => SetProperty(() => IsReplacing, value);
        }

        public bool MatchCase
        {
            get => GetProperty(() => MatchCase);
            set
            {
                SetProperty(() => MatchCase, value);
                Highlight();
            }
        }

        public bool MatchWholeWord
        {
            get => GetProperty(() => MatchWholeWord);
            set
            {
                SetProperty(() => MatchWholeWord, value);
                Highlight();
            }
        }

        public bool UseRegex
        {
            get => GetProperty(() => UseRegex);
            set
            {
                SetProperty(() => UseRegex, value);

                ValidatePattern();
                Highlight();
            }
        }

        public string FindTerm
        {
            get => GetProperty(() => FindTerm);
            set
            {
                SetProperty(() => FindTerm, value);
                RaisePropertyChanged(() => CanFind);

                Highlight();
            }
        }

        public string ReplaceTerm
        {
            get => GetProperty(() => ReplaceTerm);
            set
            {
                SetProperty(() => ReplaceTerm, value);
                RaisePropertyChanged(() => CanReplace);
            }
        }

        public int Find(string term)
        {
            if (string.IsNullOrEmpty(term))
                return 0;

            return -1;
        }

        public void FindPrevious()
        {
            if (!CanFind)
                return;
        }

        public void FindNext()
        {
            if (!CanFind)
                return;
        }

        public void ReplaceNext()
        {
            if (!CanReplace)
                return;
        }

        public void ReplaceAll()
        {
            if (!CanReplace)
                return;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName == nameof(FindTerm))
                return new string[] { error };

            return Enumerable.Empty<string>();
        }

        private void Highlight()
        {
            if (!ValidatePattern())
            {
                textEditor.TextArea.TextView.LineTransformers.Remove(resultsColorizer);
                return;
            }

            textEditor.TextArea.TextView.LineTransformers.Remove(resultsColorizer);
            textEditor.TextArea.TextView.LineTransformers.Add(resultsColorizer);
        }

        private bool ValidatePattern()
        {
            error = string.Empty;

            if (string.IsNullOrEmpty(FindTerm))
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(FindTerm)));
                return false;
            }

            if (!UseRegex)
            {
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(FindTerm)));
                return true;
            }

            var isValid = PatternIsValid(FindTerm);

            if (!isValid)
            {
                error = "Invalid Regular Expression";
                ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(FindTerm)));
            }

            return isValid;
        }

        private bool PatternIsValid(string pattern)
        {
            if (UseRegex)
            {
                try
                {
                    _ = new Regex(pattern);
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }

        private class FindResultsColorizer : DocumentColorizingTransformer
        {
            private readonly FindReplaceService service;

            public FindResultsColorizer(FindReplaceService service)
            {
                this.service = service;
            }

            protected override void ColorizeLine(DocumentLine line)
            {
                var lineOffset = line.Offset;
                var text = CurrentContext.Document.GetText(line);
                var pattern = service.FindTerm;

                if (!service.UseRegex)
                    pattern = Regex.Escape(pattern);

                var options = RegexOptions.None;

                if (!service.MatchCase)
                    options = RegexOptions.IgnoreCase;

                if (service.MatchWholeWord)
                    pattern = @$"\b({pattern})\b";

                var regex = new Regex(pattern, options);
                var matches = new Regex(pattern, options).Matches(text);

                if (!matches.Any())
                    return;

                foreach (Match match in matches)
                {
                    var startOffset = lineOffset + match.Index;
                    var endOffset = startOffset + match.Length;

                    ChangeLinePart(startOffset, endOffset, element => element.TextRunProperties.SetBackgroundBrush(Brushes.YellowGreen));
                }
            }
        }
    }

    public interface IFindReplaceService
    {
        bool CanFind { get; }
        bool CanReplace { get; }

        bool IsReplacing { get; set; }

        bool MatchCase { get; set; }
        bool MatchWholeWord { get; set; }
        bool UseRegex { get; set; }

        string FindTerm { get; set; }
        string ReplaceTerm { get; set; }

        int Find(string term = null);

        void FindPrevious();

        void FindNext();

        void ReplaceNext();

        void ReplaceAll();
    }
}