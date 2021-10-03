using SLStudio.Core.Modules.StatusBar.ViewModels;

namespace SLStudio.Core.Modules.StatusBar.Services
{
    internal class InternalStatusBarContentProvider : StatusBarProvider
    {
        private readonly LeftContentViewModel status;

        public InternalStatusBarContentProvider(IStatusBar statusBar)
        {
            status = new LeftContentViewModel(statusBar);
        }

        protected override void Setup()
        {
            AddLeftContent(status);
        }
    }
}