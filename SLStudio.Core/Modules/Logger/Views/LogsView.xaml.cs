using SLStudio.Core.Modules.Logger.ViewModels;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace SLStudio.Core.Modules.Logger.Views
{
    public partial class LogsView
    {
        public LogsView()
        {
            InitializeComponent();
        }

        private void OnRowPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left && DataContext is LogsViewModel viewModel)
            {
                viewModel.ViewLogDetails();
                e.Handled = true;
            }
        }

        private void OnRowPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter && DataContext is LogsViewModel viewModel)
            {
                viewModel.ViewLogDetails();
                e.Handled = true;
            }
        }

        private void OnDataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selector = sender as Selector;
            if (selector is DataGrid dataGrid && selector.SelectedItem != null && dataGrid.SelectedIndex >= 0)
                dataGrid.ScrollIntoView(selector.SelectedItem);
        }
    }
}
