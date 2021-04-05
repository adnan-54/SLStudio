using SLStudio.Core;

namespace SLStudio.NotificationCenter
{
    internal class NotificationsStatusBarProvider : StatusBarProvider
    {
        private readonly StatusBarRightViewModel rightContent;

        public NotificationsStatusBarProvider()
        {
            rightContent = new StatusBarRightViewModel();
        }

        protected override void Setup()
        {
            AddRightContent(rightContent);
        }
    }
}