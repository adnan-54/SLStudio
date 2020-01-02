using MahApps.Metro.Controls;
using System.Windows.Controls;

namespace SLStudio.Core.Modules.Console.Views
{
    public partial class ConsoleView : MetroWindow
    {
        public ConsoleView()
        {
            InitializeComponent();
        }

        private void ConsoleTextChanged(object sender, TextChangedEventArgs e)
        {
            Console.ScrollToEnd();
        }
    }
}