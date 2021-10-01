namespace SLStudio.Core.Modules.StatusBar.ViewModels
{
    internal class LeftContentViewModel : IStatusBarContent
    {
        public LeftContentViewModel(IStatusBar statusBar)
        {
            StatusBar = statusBar;

            Id = int.MinValue + 1;
        }

        public IStatusBar StatusBar { get; }

        public int Id { get; }
    }
}