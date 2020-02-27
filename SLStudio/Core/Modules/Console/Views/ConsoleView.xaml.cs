using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Search;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace SLStudio.Core.Modules.Console.Views
{
    /// <summary>
    /// Interaction logic for ConsoleView.xaml
    /// </summary>
    public partial class ConsoleView
    {
        private const double FONT_MAX_SIZE = 64;
        private const double FONT_MIN_SIZE = 5;

        private SearchPanel searchPanel;

        public ConsoleView()
        {
            InitializeComponent();
        }

        private void OnTextChanged(object sender, System.EventArgs e)
        {
            if (sender is TextEditor textEditor)
                textEditor.ScrollToEnd();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is TextEditor editor)
            {
                searchPanel = SearchPanel.Install(editor.TextArea);
                searchPanel.MarkerBrush = (Brush)Application.Current.TryFindResource("MahApps.Brushes.AccentBase");
            }
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            if (searchPanel == null)
                return;

            if (searchPanel.IsClosed)
            {
                searchPanel.Open();
                searchPanel.Focus();
                searchPanel.Reactivate();
            }
            else
                searchPanel.Close();
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                UpdateFontSize(e.Delta > 0);
                e.Handled = true;
            }
        }

        public void UpdateFontSize(bool increase)
        {
            double currentSize = TextEditor.FontSize;

            if (increase)
            {
                if (currentSize < FONT_MAX_SIZE)
                {
                    double newSize = Math.Min(FONT_MAX_SIZE, currentSize + 1);
                    TextEditor.FontSize = newSize;
                }
            }
            else
            {
                if (currentSize > FONT_MIN_SIZE)
                {
                    double newSize = Math.Max(FONT_MIN_SIZE, currentSize - 1);
                    TextEditor.FontSize = newSize;
                }
            }
        }

    }
}