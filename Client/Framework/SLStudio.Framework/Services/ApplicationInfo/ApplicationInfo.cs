using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace SLStudio
{
    internal class ApplicationInfo : IApplicationInfo
    {
        public ApplicationInfo(Application application)
        {
            Application = application;
            Application.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        public Application Application { get; }

        public Dispatcher Dispatcher => Application.Dispatcher;

        public Window MainWindow => Application.MainWindow;

        public Window CurrentWindow => GetCurrentWindow();

        public void MergeResource(Uri uri)
        {
            Application.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = uri });
        }

        private Window GetCurrentWindow()
        {
            var target = Application.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);
            return target is null or ISplashScreen ? MainWindow : target;
        }
    }
}