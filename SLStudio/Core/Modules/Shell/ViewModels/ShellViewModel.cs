using Caliburn.Micro;

namespace SLStudio.Core.Modules.Shell.ViewModels
{
    internal class ShellViewModel : Conductor<object>, IShell
    {
        public ShellViewModel()
        {
            DisplayName = "Slstudio";
        }
    }
}
