using DevExpress.Mvvm;
using SLStudio.Core.Modules.StatusBar.Resources;
using SLStudio.Core.Modules.StatusBar.Services;

namespace SLStudio.Core.Modules.StatusBar.ViewModels
{
    internal class StatusBarViewModel : ViewModelBase, IStatusBar, IStatusBarHost
    {
        public StatusBarViewModel(IStatusBarManager statusBarManager)
        {
            StatusBarManager = statusBarManager;
            StatusBarProvider = new InternalStatusBarContentProvider(this);
            StatusBarManager.AddHost(this);
            Status = StatusBarResources.Ready;
        }

        public IStatusBarManager StatusBarManager { get; }

        public IStatusBarProvider StatusBarProvider { get; }

        public string Status
        {
            get => GetProperty(() => Status);
            set => SetProperty(() => Status, value);
        }
    }
}