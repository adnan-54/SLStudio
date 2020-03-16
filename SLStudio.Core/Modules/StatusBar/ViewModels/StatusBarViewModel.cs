namespace SLStudio.Core.Modules.StatusBar.ViewModels
{
    internal class StatusBarViewModel : ViewModel, IStatusBar
    {
        public StatusBarViewModel()
        {
            Status = Resources.StatusBarResources.Ready;
        }

        public string Status
        {
            get => GetProperty(() => Status);
            set => SetProperty(() => Status, value);
        }
    }
}
