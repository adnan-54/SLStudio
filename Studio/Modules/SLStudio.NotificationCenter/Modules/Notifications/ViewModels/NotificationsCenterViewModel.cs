using SLStudio.Core;

namespace SLStudio.NotificationCenter
{
    internal class NotificationsCenterViewModel : ToolBase, INotificationsCenter
    {
        public NotificationsCenterViewModel(IStatusBarManager statusBarManager)
        {
            StatusBarProvider = new NotificationsStatusBarProvider();

            DisplayName = "Notifications";

            statusBarManager.AddHost(this);
        }

        public override WorkspaceItemPlacement Placement => WorkspaceItemPlacement.Right;

        public IStatusBarProvider StatusBarProvider { get; }
    }

    internal interface INotificationsCenter : IToolItem, IStatusBarHost
    {
    }
}