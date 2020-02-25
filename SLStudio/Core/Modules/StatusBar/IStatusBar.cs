namespace SLStudio.Core
{
    public interface IStatusBar
    {
        bool IsBusy { get; set; }
        string IsBusyStatus { get; set; }
        string Status { get; set; }
        int? Line { get; set; }
        int? Column { get; set; }
    }
}
