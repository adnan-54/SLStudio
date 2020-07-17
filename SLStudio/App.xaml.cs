using SLStudio.Core;
using System.Windows;

namespace SLStudio
{
    public partial class App : Application
    {
        private async void OnStartup(object sender, StartupEventArgs e)
        {
            await Bootstrapper.Run(Dispatcher, e.Args);
        }
    }
}