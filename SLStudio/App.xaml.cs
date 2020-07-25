using SLStudio.Core;
using System.Windows;

namespace SLStudio
{
    public partial class App : Application
    {
        private void OnStartup(object sender, StartupEventArgs e)
        {
            Bootstrapper.Run(Dispatcher, e.Args);
        }
    }
}