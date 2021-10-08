using System.Windows;

namespace SLStudio.Core
{
    public partial class ShellView : Window
    {
        public ShellView()
        {
            InitializeComponent();

            Application.Current.MainWindow = this;
        }
    }
}
