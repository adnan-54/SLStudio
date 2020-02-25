using Caliburn.Micro;

namespace SLStudio.Core.Modules.StatusBar.ViewModels
{
    internal class StatusBarViewModel : PropertyChangedBase, IStatusBar
    {
        private bool isBusy;
        private string isBusyStatus;
        private string status;
        private int? line;
        private int? column;

        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                IsBusyStatus = string.Empty;
                NotifyOfPropertyChange(() => IsBusy);
            }
        }

        public string IsBusyStatus
        {
            get => isBusyStatus;
            set
            {
                isBusyStatus = value;
                NotifyOfPropertyChange(() => IsBusyStatus);
            }
        }

        public string Status
        {
            get => status;
            set
            {
                status = value;
                NotifyOfPropertyChange(() => Status);
            }
        }

        public bool HaveLinesOrColumns => Line != null || Column != null;

        public int? Line
        {
            get => line;
            set
            {
                line = value;
                NotifyOfPropertyChange(() => Line);
                NotifyOfPropertyChange(() => HaveLinesOrColumns);
            }
        }

        public int? Column
        {
            get => column;
            set
            {
                column = value;
                NotifyOfPropertyChange(() => Column);
                NotifyOfPropertyChange(() => HaveLinesOrColumns);
            }
        }
    }
}