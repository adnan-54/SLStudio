using DevExpress.Mvvm;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;

namespace SLStudio.Core
{
    internal class FindReplaceService : BindableBase, IFindReplaceService, INotifyDataErrorInfo
    {
        private readonly StudioTextEditor textEditor;
        private readonly SearchResultBackgroundRenderer backgroundRenderer;
        private readonly SearchCache cache;
        private string error;

        public FindReplaceService(StudioTextEditor textEditor)
        {
            this.textEditor = textEditor;
            cache = new(this, textEditor);
            backgroundRenderer = new(cache);

            CurrentSearchRegex = new("");

            this.WhenAnyValue(x => x.FindTerm)
                .DistinctUntilChanged()
                .ObserveOnDispatcher()
                .Subscribe(_ => IsThrottling = true);

            this.WhenAnyValue(x => x.FindTerm)
                .DistinctUntilChanged()
                .Throttle(TimeSpan.FromMilliseconds(500))
                .ObserveOnDispatcher()
                .Subscribe(_ => IsThrottling = false);

            textEditor.TextArea.TextView.BackgroundRenderers.Add(backgroundRenderer);
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => UseRegex && !string.IsNullOrEmpty(error);

        public bool CanFind => !string.IsNullOrEmpty(FindTerm);

        public bool CanReplace => IsReplacing;

        public Regex CurrentSearchRegex { get; set; }

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
                UpdateRegex();
                RefreshBackgroundRenderer();
            }
        }

        public bool MatchWholeWord
        {
            get => GetProperty(() => MatchWholeWord);
            set
            {
                SetProperty(() => MatchWholeWord, value);
                UpdateRegex();
                RefreshBackgroundRenderer();
            }
        }

        public bool UseRegex
        {
            get => GetProperty(() => UseRegex);
            set
            {
                SetProperty(() => UseRegex, value);
                Validate();
                UpdateRegex();
                RefreshBackgroundRenderer();
            }
        }

        public string FindTerm
        {
            get => GetProperty(() => FindTerm);
            set
            {
                SetProperty(() => FindTerm, value);
                RaisePropertyChanged(() => CanFind);
                Validate();
                UpdateRegex();
                RefreshBackgroundRenderer();
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

        public bool IsThrottling
        {
            get => GetProperty(() => IsThrottling);
            set
            {
                if (value == IsThrottling)
                    return;

                SetProperty(() => IsThrottling, value);

                if (!value)
                {
                    UpdateRegex();
                    RefreshBackgroundRenderer();
                    CacheResults();
                }
            }
        }

        public bool IsSearching
        {
            get => GetProperty(() => IsSearching);
            set => SetProperty(() => IsSearching, value);
        }

        public int ResultsCount => cache.Count;

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

            CacheResults();

            cache?.FindPrevious()?.Select();
        }

        public void FindNext()
        {
            if (!CanFind)
                return;

            ForceCache();

            cache?.FindNext()?.Select();
        }

        public void ReplaceNext()
        {
            if (!CanReplace)
                return;

            ForceCache();

            var current = cache.FindFromOffset(textEditor.CaretOffset);
            if (current is null)
            {
                FindNext();
                return;
            }

            current.Replace(ReplaceTerm);
        }

        public void ReplaceAll()
        {
            if (!CanReplace)
                return;

            ForceCache();

            textEditor.BeginChange();

            foreach (var result in cache)
                result.Replace(ReplaceTerm);

            textEditor.EndChange();
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (propertyName == nameof(FindTerm))
                return new string[] { error };

            return Enumerable.Empty<string>();
        }

        public void ForceCache()
        {
            IsThrottling = true;
            IsThrottling = false;
        }

        public void UpdateRegex()
        {
            if (string.IsNullOrEmpty(FindTerm))
                return;

            var pattern = FindTerm;

            if (!UseRegex)
                pattern = Regex.Escape(pattern);
            else
            if (!PatternIsValid(pattern))
                return;

            var options = RegexOptions.None;

            if (!MatchCase)
                options = RegexOptions.IgnoreCase;

            if (MatchWholeWord)
                pattern = @$"\b({pattern})\b";

            CurrentSearchRegex = new(pattern, options);
        }

        public void ShowBackgroundRenderer()
        {
            backgroundRenderer.Show();
            RefreshBackgroundRenderer();
        }

        public void HideBackgroundRenderer()
        {
            backgroundRenderer.Hide();
            RefreshBackgroundRenderer();
        }

        public void RefreshBackgroundRenderer()
        {
            textEditor.TextArea.TextView.InvalidateLayer(KnownLayer.Selection);
        }

        private bool Validate()
        {
            error = string.Empty;

            if (string.IsNullOrEmpty(FindTerm))
            {
                RaiseErrorChanged();
                return true;
            }

            if (!UseRegex)
            {
                RaiseErrorChanged();
                return true;
            }

            var isValid = PatternIsValid(FindTerm);

            if (!isValid)
                error = "Invalid regex pattern";

            RaiseErrorChanged();

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

        private void RaiseErrorChanged()
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(FindTerm)));
        }

        private void CacheResults()
        {
            IsSearching = true;

            cache.Refresh();
            RaisePropertyChanged(() => ResultsCount);

            IsSearching = false;
        }
    }

    internal class SearchCache : List<SearchResult>
    {
        private readonly IFindReplaceService findReplaceService;
        private readonly StudioTextEditor textEditor;

        public SearchCache(IFindReplaceService findReplaceService, StudioTextEditor textEditor)
        {
            this.findReplaceService = findReplaceService;
            this.textEditor = textEditor;
        }

        public SearchResult FindNext()
        {
            if (textEditor.CaretOffset > 0 && textEditor.CaretOffset < this[0].StartOffset)
                return this[0];

            if (textEditor.CaretOffset > this[Count - 1].EndOffset)
                return this[0];

            var nearest = FindNearest();

            if (nearest is null)
                nearest = this.FirstOrDefault(r => r.StartOffset > textEditor.CaretOffset);

            if (nearest is null)
                return null;

            if (nearest.StartOffset >= textEditor.CaretOffset)
                return nearest;

            var nearestIndex = IndexOf(nearest);
            var targetIndex = nearestIndex + 1 >= Count ? 0 : nearestIndex + 1;

            return this[targetIndex];
        }

        public SearchResult FindPrevious()
        {
            if (textEditor.CaretOffset > 0 && textEditor.CaretOffset < this[0].StartOffset)
                return this[Count - 1];

            if (textEditor.CaretOffset > this[Count - 1].EndOffset)
                return this[Count - 1];

            var nearest = FindNearest();

            if (nearest is null)
                nearest = this.LastOrDefault(r => r.StartOffset > textEditor.CaretOffset);

            if (nearest is null)
                return null;

            if (textEditor.CaretOffset > nearest.EndOffset)
                return nearest;

            var nearestIndex = IndexOf(nearest);
            var targetIndex = nearestIndex - 1 >= 0 ? nearestIndex - 1 : Count - 1;

            return this[targetIndex];
        }

        public SearchResult FindFromOffset(int offset)
        {
            return this.SingleOrDefault(r => r.IsValid && offset >= r.StartOffset && offset <= r.EndOffset);
        }

        public SearchResult FindNearest()
        {
            if (Count == 0)
                return null;

            if (Count == 1)
                return this[0];

            var caretOffset = textEditor.CaretOffset;
            var firstResult = this[0];
            var lastResult = this[Count - 1];
            var firstStartOffset = firstResult.StartOffset;
            var lastEndOffset = lastResult.EndOffset;

            if (caretOffset < firstStartOffset)
                return lastResult;
            if (caretOffset > lastEndOffset)
                return firstResult;

            var leftResult = this.LastOrDefault(r => r.EndOffset <= caretOffset);
            var rightResult = this.FirstOrDefault(r => r.StartOffset >= caretOffset);

            if (leftResult is null || rightResult is null)
                return FindFromOffset(textEditor.CaretOffset);

            var separator = textEditor.Document.GetText(leftResult.EndOffset, rightResult.StartOffset - leftResult.EndOffset);

            var pivot = leftResult.EndOffset + separator.Length / 2;

            if (caretOffset >= pivot)
                return rightResult;

            return leftResult;
        }

        public void Refresh()
        {
            Clear();

            if (string.IsNullOrEmpty(findReplaceService.FindTerm))
                return;

            var matches = findReplaceService.CurrentSearchRegex.Matches(textEditor.Text).Select(match => new SearchResult(textEditor, match));
            AddRange(matches);
        }
    }

    internal class SearchResult : TextSegment
    {
        private readonly StudioTextEditor textEditor;

        public SearchResult(StudioTextEditor textEditor, Match match)
        {
            this.textEditor = textEditor;
            Match = match;
            Line = textEditor.Document.GetLineByOffset(match.Index);
            LineOffset = Line.Offset;
            OriginalText = match.Value;
            CurrentText = match.Value;
            StartOffset = match.Index;
            RelativeStartOffset = StartOffset - LineOffset;
            EndOffset = StartOffset + match.Length;
            RelativeEndOffset = EndOffset - LineOffset;
            Length = match.Length;
        }

        public Match Match { get; }

        public DocumentLine Line { get; }

        public int LineOffset { get; }

        public string OriginalText { get; }

        public string CurrentText { get; private set; }

        public int RelativeStartOffset { get; }

        public int RelativeEndOffset { get; }

        public bool IsValid => OriginalText.Equals(CurrentText, StringComparison.CurrentCultureIgnoreCase);

        public void BringToView()
        {
            if (!IsValid)
                return;

            textEditor.ScrollTo(Line.LineNumber, RelativeEndOffset);
        }

        public void Select()
        {
            if (!IsValid)
                return;

            BringToView();
            textEditor.Select(StartOffset, Length);
            textEditor.CaretOffset = EndOffset;
        }

        public void Replace(string text)
        {
            if (!IsValid)
                return;

            BringToView();
            textEditor.Document.Replace(StartOffset, Length, text);
            textEditor.Select(StartOffset, Length);
            CurrentText = text;
        }
    }

    public interface IFindReplaceService
    {
        bool CanFind { get; }
        bool CanReplace { get; }

        Regex CurrentSearchRegex { get; }

        bool IsReplacing { get; set; }

        bool MatchCase { get; set; }
        bool MatchWholeWord { get; set; }
        bool UseRegex { get; set; }

        string FindTerm { get; set; }
        string ReplaceTerm { get; set; }

        bool IsSearching { get; }

        int ResultsCount { get; }

        int Find(string term = null);

        void FindPrevious();

        void FindNext();

        void ReplaceNext();

        void ReplaceAll();

        void ForceCache();

        void UpdateRegex();

        void ShowBackgroundRenderer();

        void HideBackgroundRenderer();

        void RefreshBackgroundRenderer();
    }
}