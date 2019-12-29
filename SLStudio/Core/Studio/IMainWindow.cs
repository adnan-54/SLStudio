namespace SLStudio.Core
{
    public interface IMainWindow
    {
        bool IsBusy { get; set; }
        
        IShell Shell { get; }
    }
}
