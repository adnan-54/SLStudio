namespace SLStudio.Core
{
    public interface IStatusBar
    {
        string Status { get; set; }
        object Content { get; set; }
    }
}