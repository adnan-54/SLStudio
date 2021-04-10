using SLStudio.Core;

namespace SLStudio.NotificationCenter.StatusBar
{
    internal class NotificationsStatusBarProvider : StatusBarProvider
    {
        private readonly RightContentViewModel rightContent;

        public NotificationsStatusBarProvider(RightContentViewModel rightContent)
        {
            this.rightContent = rightContent;
        }

        protected override void Setup()
        {
            AddRightContent(rightContent);
        }
    }
}