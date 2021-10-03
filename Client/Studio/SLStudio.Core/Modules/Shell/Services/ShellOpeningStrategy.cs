using System;
using System.Linq;
using System.Windows;

namespace SLStudio.Core
{
    internal class ShellOpeningStrategy
    {
        private readonly IShell shell;
        private readonly IApplicationInfo applicationInfo;

        public ShellOpeningStrategy(IShell shell, IApplicationInfo applicationInfo)
        {
            this.shell = shell;
            this.applicationInfo = applicationInfo;

            shell.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, EventArgs e)
        {
            shell.Loaded -= OnLoaded;

            var application = applicationInfo.Application;

            var shellWindow = application.Windows.OfType<ShellView>().FirstOrDefault();
            shellWindow.Owner = null;

            application.MainWindow = shellWindow;

            application.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }
    }
}