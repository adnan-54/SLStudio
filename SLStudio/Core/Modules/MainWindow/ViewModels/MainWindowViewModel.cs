using Caliburn.Micro;

namespace SLStudio.Core.Modules.MainWindow.ViewModels
{
    internal class MainWindowViewModel : Screen, IMainWindow
    {
        public MainWindowViewModel()
        {
            DisplayName = "SLStudio";
        }
    }
}