using System.Windows;
using System.Windows.Media;

namespace SLStudio.Controls.StudioTextEditor_Control
{
    internal class RendersHandler
    {
        private readonly StudioTextEditor textEditor;

        public RendersHandler(StudioTextEditor textEditor)
        {
            this.textEditor = textEditor;

            textEditor.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            textEditor.Loaded -= OnLoaded;

            SetupCurrentLineHighlighter();
        }

        private void SetupCurrentLineHighlighter()
        {
            textEditor.TextArea.Options.HighlightCurrentLine = true;

            WpfHelpers.TryFindResource("Border", out Brush borderBrush);
            var backgorund = new SolidColorBrush(new Color() { A = 0 });
            var border = new Pen(borderBrush, 2.5);

            backgorund.Freeze();
            border.Freeze();

            textEditor.TextArea.TextView.CurrentLineBackground = backgorund;
            textEditor.TextArea.TextView.CurrentLineBorder = border;
        }
    }
}