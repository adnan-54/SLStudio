using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace SLStudio
{
    internal class ApplicationInfo : StudioService, IApplicationInfo
    {
        private readonly Application application;

        public ApplicationInfo(Application application)
        {
            this.application = application;
            application.Startup += OnApplicationStartup;
        }

        public IEnumerable<string> StartupArguments { get; private set; }

        public Dispatcher Dispatcher => application?.Dispatcher;

        private void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            application.Startup -= OnApplicationStartup;
            StartupArguments = e.Args;
        }
    }
}