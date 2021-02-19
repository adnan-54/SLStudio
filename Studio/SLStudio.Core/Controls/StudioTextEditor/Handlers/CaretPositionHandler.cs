using ICSharpCode.AvalonEdit.Editing;
using System;
using System.Windows;

namespace SLStudio.Core
{
    internal class CaretPositionHandler : ITextEditorHandler
    {
        private readonly StudioTextEditor textEditor;
        private readonly Caret caret;

        public CaretPositionHandler(StudioTextEditor textEditor)
        {
            this.textEditor = textEditor;
            caret = textEditor.TextArea.Caret;

            textEditor.Loaded += OnLoaded;
            textEditor.Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            caret.PositionChanged += Caret_OnPositionChanged;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            caret.PositionChanged -= Caret_OnPositionChanged;
        }

        private void Caret_OnPositionChanged(object sender, EventArgs e)
        {
            textEditor.CurrentLine = caret.Line;
            textEditor.CurrentColumn = caret.Column;
        }
    }
}