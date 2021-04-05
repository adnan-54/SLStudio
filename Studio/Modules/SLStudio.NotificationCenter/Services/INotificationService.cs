using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio.NotificationCenter
{
    public interface INotificationService
    {
        public event EventHandler UpdateSucceeded;

        public event EventHandler UpdateFailed;

        IEnumerable<INotification> Notifications { get; }

        IEnumerable<INotificationModule> NotificationModules { get; }

        void RegisterModule(INotificationModule module);

        void UnregisterModule(INotificationModule module);

        Task UpdateNotifications();
    }
}