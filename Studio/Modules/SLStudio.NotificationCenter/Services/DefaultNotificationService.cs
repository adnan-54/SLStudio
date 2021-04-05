using SLStudio.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLStudio.NotificationCenter
{
    internal class DefaultNotificationService : INotificationService
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<DefaultNotificationService>();

        private readonly object lockObject = new();
        private readonly List<INotification> notifications;
        private readonly List<INotificationModule> notificationModules;

        public DefaultNotificationService()
        {
            notificationModules = new List<INotificationModule>();
            notifications = new List<INotification>();
        }

        public event EventHandler UpdateSucceeded;

        public event EventHandler UpdateFailed;

        public IEnumerable<INotification> Notifications => notifications;

        public IEnumerable<INotificationModule> NotificationModules => notificationModules;

        public void RegisterModule(INotificationModule module)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));

            lock (lockObject)
            {
                if (notificationModules.Contains(module))
                    return;

                notificationModules.Add(module);
            }
        }

        public void UnregisterModule(INotificationModule module)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));

            lock (lockObject)
            {
                if (!notificationModules.Contains(module))
                    return;

                notificationModules.Remove(module);
            }
        }

        public async Task UpdateNotifications()
        {
            try
            {
                var newNotifications = new List<INotification>();

                foreach (var notificationModule in NotificationModules)
                    newNotifications.AddRange(await notificationModule.FetchNotifications());

                notifications.Clear();
                notifications.AddRange(newNotifications.OrderBy(n => n.PublicationDate));

                UpdateSucceeded?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                logger.Warn(ex);
                notifications.Clear();

                UpdateFailed?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}