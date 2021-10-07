using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;

namespace SLStudio
{
    internal class ApplicationInfo : Service, IApplicationInfo
    {
        public ApplicationInfo(Application application)
        {
            Application = application;
        }

        public Application Application
        {
            get;
        }

        public Dispatcher Dispatcher => Application.Dispatcher;

        public Window CurrentWindow => GetCurrentWindow();

        public void MergeResource(Uri uri)
        {
            Application.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = uri });
        }

        private Window GetCurrentWindow()
        {
            var target = Application.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive);

            if (target is null)
                target = Application.MainWindow;

            return target;
        }
    }
}