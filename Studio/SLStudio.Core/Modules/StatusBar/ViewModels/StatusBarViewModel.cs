using DevExpress.Mvvm;
using SLStudio.Core.Modules.StatusBar.Resources;

namespace SLStudio.Core.Modules.StatusBar.ViewModels
{
    internal class StatusBarViewModel : ViewModelBase, IStatusBar
    {
        public StatusBarViewModel()
        {
            Status = StatusBarResources.Ready;
        }

        public string Status
        {
            get => GetProperty(() => Status);
            set => SetProperty(() => Status, value);
        }

        public object Content
        {
            get => GetProperty(() => Content);
            set => SetProperty(() => Content, value);
        }

        public bool PopupIsOpen
        {
            get => GetProperty(() => PopupIsOpen);
            set => SetProperty(() => PopupIsOpen, value);
        }

        public void ShowPopup()
        {
            PopupIsOpen = !PopupIsOpen;
        }
    }
}