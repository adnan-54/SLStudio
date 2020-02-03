namespace SLStudio.Core
{
    public interface IMainWindow
    {
        IMainMenu MainMenu { get; }
        IToolbar Toolbar { get; }
        IShell Shell { get; }
        IStatusBar StatusBar { get; }
    }
}