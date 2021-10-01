using ICSharpCode.AvalonEdit.Editing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SLStudio.Core
{
    internal class LeftMarginsHandler
    {
        private readonly StudioTextEditor textEditor;

        public LeftMarginsHandler(StudioTextEditor textEditor)
        {
            this.textEditor = textEditor;

            textEditor.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            textEditor.Loaded -= OnLoaded;

            SetupLeftMargins();
        }

        private void SetupLeftMargins()
        {
            foreach (var item in textEditor.TextArea.LeftMargins.ToList())
            {
                if (item is LineNumberMargin)
                    textEditor.TextArea.LeftMargins.Remove(item);
            }

            var leftMargin = new StudioTextEditorLeftMargin(CreateLineNumberMargin());
            textEditor.TextArea.LeftMargins.Insert(0, leftMargin);
        }

        private LineNumberMargin CreateLineNumberMargin()
        {
            LineNumberMargin lineNumbers = new LineNumberMargin();
            WpfHelpers.TryFindResource("Focused", out SolidColorBrush foregroundColor);
            lineNumbers.SetValue(Control.ForegroundProperty, foregroundColor);
            lineNumbers.SetValue(AbstractMargin.TextViewProperty, textEditor.TextArea.TextView);

            return lineNumbers;
        }
    }
}