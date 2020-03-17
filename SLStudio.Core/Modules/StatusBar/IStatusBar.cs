namespace SLStudio.Core
{
    public interface IStatusBar
    {
        bool IsBusy { get; set; }
        string Status { get; set; }
    }
}
