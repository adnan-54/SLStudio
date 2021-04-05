using DevExpress.Mvvm;
using SLStudio.Core;
using System;
using System.Linq;

namespace SLStudio.NotificationCenter
{
    internal class StatusBarRightViewModel : BindableBase, IStatusBarContent
    {
        private readonly INotificationService notificationService;

        public StatusBarRightViewModel(INotificationService notificationService)
        {
            this.notificationService = notificationService;
            notificationService.UpdateSucceeded += OnUpdateSucceeded;
            notificationService.UpdateFailed += OnUpdateFailed;

            Id = int.MaxValue;
        }

        public int Id { get; }

        public bool HasNotification => NotificationCount > 0;

        public int NotificationCount
        {
            get => GetProperty(() => NotificationCount);
            set
            {
                SetProperty(() => NotificationCount, value);
                RaisePropertyChanged(() => HasNotification);
            }
        }

        public bool FailedToUpdate
        {
            get => GetProperty(() => FailedToUpdate);
            set => SetProperty(() => FailedToUpdate, value);
        }

        public void Show()
        {
        }

        private void OnUpdateSucceeded(object sender, EventArgs e)
        {
            FailedToUpdate = false;
            NotificationCount = notificationService.Notifications.Count();
        }

        private void OnUpdateFailed(object sender, EventArgs e)
        {
            FailedToUpdate = true;
            NotificationCount = 0;
        }
    }
}