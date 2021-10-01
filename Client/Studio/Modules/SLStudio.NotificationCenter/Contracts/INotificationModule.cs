using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio.NotificationCenter
{
    public interface INotificationModule
    {
        string Name { get; }

        Task<IEnumerable<INotification>> FetchNotifications();
    }
}