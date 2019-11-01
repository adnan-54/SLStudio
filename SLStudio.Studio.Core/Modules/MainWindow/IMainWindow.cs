using SLStudio.Studio.Core.Enums;
using SLStudio.Studio.Core.Modules;
using System.Windows;

namespace SLStudio.Studio.Core.Modules
{
    public interface IMainWindow
    {
        IShell Shell { get; set; }
        MainWindowStatus Status { get; set; }
        WindowState WindowState { get; set; }
        string Title { get; set; }

    }
}
