namespace SLStudio.Core
{
    public interface IStatusBarHost
    {
        IStatusBarProvider StatusBarProvider { get; }
    }
}