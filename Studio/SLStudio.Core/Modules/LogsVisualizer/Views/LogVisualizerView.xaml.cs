using SLStudio.Core.Modules.LogsVisualizer.ViewModels;
using System.Windows.Input;

namespace SLStudio.Core.Modules.LogsVisualizer.Views
{
    public partial class LogVisualizerView
    {
        public LogVisualizerView()
        {
            InitializeComponent();
        }

        private void OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && DataContext is LogVisualizerViewModel viewModel)
                viewModel.ViewDetails();
        }
    }
}