using DevExpress.Mvvm.UI;
using ICSharpCode.AvalonEdit.Folding;
using System;
using System.Reactive.Linq;
using System.Windows;

namespace SLStudio.Core.Behaviors
{
    public class LanguageDefinitionBehavior : ServiceBaseGeneric<StudioTextEditor>, ILanguageDefinitionBehavior
    {
        private readonly ILanguageDefinitionManager languageDefinitionManager;
        private readonly IUiSynchronization uiSynchronization;
        private ILanguageDefinition languageDefinition;
        private IDisposable foldingThrottle;

        public LanguageDefinitionBehavior()
        {
            languageDefinitionManager = IoC.Get<ILanguageDefinitionManager>();
            uiSynchronization = IoC.Get<IUiSynchronization>();
        }

        public string Extension { get; set; }

        public FoldingManager FoldingManager { get; private set; }

        public void UpdateFolding()
        {
            if (languageDefinition?.FoldingStrategy == null || FoldingManager == null)
                return;

            var foldings = languageDefinition.FoldingStrategy.CreateNewFoldings(AssociatedObject.Document, out int firstErrorOffset);
            FoldingManager.UpdateFoldings(foldings, firstErrorOffset);
        }

        public void UpdateIndentation()
        {
            if (languageDefinition?.IndentationStrategy == null)
                return;

            AssociatedObject.TextArea.IndentationStrategy?.IndentLines(AssociatedObject.Document, 1, AssociatedObject.Document.LineCount);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += OnLoaded;
        }

        protected override void OnDetaching()
        {
            foldingThrottle?.Dispose();
            base.OnDetaching();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Loaded -= OnLoaded;

            languageDefinition ??= languageDefinitionManager.GetByExtension(Extension);

            ApplyIndentationStrategy();
            ApplyFoldingStrategy();
            ApplySyntaxHighlighting();
        }

        private void ApplyIndentationStrategy()
        {
            if (languageDefinition?.IndentationStrategy == null)
                return;

            AssociatedObject.TextArea.IndentationStrategy = languageDefinition.IndentationStrategy;
        }

        private void ApplyFoldingStrategy()
        {
            if (languageDefinition?.FoldingStrategy == null)
                return;

            FoldingManager = FoldingManager.Install(AssociatedObject.TextArea);

            foldingThrottle?.Dispose();
            foldingThrottle = Observable.FromEventPattern(AssociatedObject, nameof(AssociatedObject.TextChanged))
                                        .Select(x => AssociatedObject.Text)
                                        .Throttle(TimeSpan.FromMilliseconds(500))
                                        .DistinctUntilChanged()
                                        .Subscribe(x => uiSynchronization.EnsureExecuteOnUiAsync(() => UpdateFolding()));
            uiSynchronization.EnsureExecuteOnUiAsync(() => UpdateFolding());
        }

        private void ApplySyntaxHighlighting()
        {
            if (languageDefinition?.SyntaxHighlighting == null)
                return;

            AssociatedObject.SyntaxHighlighting = languageDefinition.SyntaxHighlighting;
        }
    }

    public interface ILanguageDefinitionBehavior
    {
        string Extension { get; }

        void UpdateFolding();

        void UpdateIndentation();
    }
}