using SimpleInjector;
using SLStudio.Core;

namespace SLStudio.NotificationCenter
{
    internal class Module : StudioModule
    {
        protected override void Register(Container container)
        {
            container.RegisterService<INotificationService, DefaultNotificationService>();
            container.RegisterSingleton<INotificationsCenter, NotificationsCenterViewModel>();
        }
    }
}