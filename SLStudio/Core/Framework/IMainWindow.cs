using System.Windows.Media;

namespace SLStudio.Core.Framework
{
    public interface IMainWindow
    {
        MainWindowState StudioState { get; set; }
        
        Brush StateColor { get; set; }
        
        bool IsBusy { get; set; }
        
        IShell Shell { get; }
    }

    public enum MainWindowState
    {
        Idle,
        Running,
        Busy
    }
}
