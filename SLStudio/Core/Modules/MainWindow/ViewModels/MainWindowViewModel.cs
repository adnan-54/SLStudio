using Caliburn.Micro;
using SLStudio.Core.Framework;

namespace SLStudio.Core.Modules.MainWindow.ViewModels
{
    class MainWindowViewModel : Screen, IMainWindow
    {
        public MainWindowViewModel(IShell shell)
        {
            Shell = shell;

            DisplayName = "SLStudio";
        }

        public IShell Shell { get; }
    }
}
