using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace SLStudio.Core.Modules.StatusBar.ViewModels
{
    internal class StatusBarViewModel : ViewModel, IStatusBar
    {
        private readonly DispatcherTimer timer;

        public StatusBarViewModel()
        {
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            timer.Tick += OnTickTick;
            PropertyChanged += OnPropertyChanged;

            Status = Resources.StatusBarResources.Ready;
        }

        public bool IsBusy
        {
            get => GetProperty(() => IsBusy);
            set => SetProperty(() => IsBusy, value);
        }

        public string Status
        {
            get => GetProperty(() => Status);
            set => SetProperty(() => Status, value);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Status) && !Status.Equals(Resources.StatusBarResources.Ready, StringComparison.InvariantCultureIgnoreCase))
            {
                timer.Stop();
                timer.Start();
            }
        }

        private void OnTickTick(object sender, EventArgs e)
        {
            timer.Stop();
            Status = Resources.StatusBarResources.Ready;
        }
    }
}
