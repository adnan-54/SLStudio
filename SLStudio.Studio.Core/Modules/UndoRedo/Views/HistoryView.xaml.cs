using SLStudio.Studio.Core.Modules.UndoRedo.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SLStudio.Studio.Core.Modules.UndoRedo.Views
{
    /// <summary>
    /// Interaction logic for HistoryView.xaml
    /// </summary>
    public partial class HistoryView : UserControl
    {
        public HistoryView()
        {
            InitializeComponent();
        }

        private void HistoryItemMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var viewModel = (HistoryViewModel)DataContext;
            var itemViewModel = (HistoryItemViewModel)((FrameworkElement)sender).DataContext;
            viewModel.UndoOrRedoTo(itemViewModel, true);
        }
    }
}
