using SLStudio.Core;
using SLStudio.NotificationCenter.StatusBar;

namespace SLStudio.NotificationCenter
{
    internal class NotificationsCenterViewModel : ToolBase, INotificationsCenter
    {
        public NotificationsCenterViewModel(IStatusBarManager statusBarManager, RightContentViewModel rightContent)
        {
            StatusBarProvider = new NotificationsStatusBarProvider(rightContent);

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