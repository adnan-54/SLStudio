using SLStudio.Core;
using SLStudio.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SLStudio.NotificationCenter
{
    internal class DefaultNotificationService : INotificationService
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<DefaultNotificationService>();

        private readonly IObjectFactory objectFactory;

        public DefaultNotificationService(IObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
        }

        public event EventHandler UpdateStarted;

        public event EventHandler UpdateEnded;

        public event EventHandler UpdateSucceeded;

        public event EventHandler UpdateFailed;

        public bool IsUpdating { get; set; }

        public IEnumerable<INotificationModule> Modules { get; private set; }

        public IEnumerable<INotification> Notifications { get; private set; }

        public async Task UpdateNotifications()
        {
            UpdateStarted?.Invoke(this, EventArgs.Empty);
            IsUpdating = true;

            try
            {
                Notifications = await FetchNotifications();
                UpdateSucceeded?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                logger.Warn(ex);
                Notifications = Enumerable.Empty<INotification>();

                UpdateFailed?.Invoke(this, EventArgs.Empty);
            }

            IsUpdating = false;
            UpdateEnded?.Invoke(this, EventArgs.Empty);
        }

        private IEnumerable<INotificationModule> CreateModulesCache()
        {
            var types = GetType().Assembly.GetTypes().Where(t => t.IsClass && t.GetCustomAttribute<NotificationModuleAttribute>(false) != null).ToList();

            return types.Select(type => objectFactory.Create<INotificationModule>(type)).ToList();
        }

        private async Task<IEnumerable<INotification>> FetchNotifications()
        {
            var notifications = new List<INotification>();

            foreach (var notificationModule in Modules)
                notifications.AddRange(await notificationModule.FetchNotifications());

            return notifications.OrderBy(n => n.PublicationDate);
        }
    }
}