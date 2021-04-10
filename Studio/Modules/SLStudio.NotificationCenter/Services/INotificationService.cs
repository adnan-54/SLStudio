using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio.NotificationCenter
{
    public interface INotificationService
    {
        event EventHandler UpdateStarted;

        event EventHandler UpdateEnded;

        event EventHandler UpdateSucceeded;

        event EventHandler UpdateFailed;

        bool IsUpdating { get; }

        IEnumerable<INotificationModule> Modules { get; }

        IEnumerable<INotification> Notifications { get; }

        Task UpdateNotifications();
    }
}