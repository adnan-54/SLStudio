using MahApps.Metro.Controls;

namespace SLStudio.WPF
{
    /// <summary>
    /// Interaction logic for CreateNewSolutionView.xaml
    /// </summary>
    public partial class CreateNewSolutionView : MetroWindow
    {
        public CreateNewSolutionView()
        {
            InitializeComponent();
            this.DataContext = new CreateNewSolutionViewModel();
        }
    }
}
