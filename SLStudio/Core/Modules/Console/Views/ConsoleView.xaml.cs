using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Search;
using System.Windows;
using System.Windows.Media;

namespace SLStudio.Core.Modules.Console.Views
{
    /// <summary>
    /// Interaction logic for ConsoleView.xaml
    /// </summary>
    public partial class ConsoleView
    {
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
    }
}