using SLStudio;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            while (true)
            {
                var stopwatch = Stopwatch.StartNew();

                var container = Initializer.Initialize(this);
                var objectFactory = container.GetService<IObjectFactory>();
                var obj = objectFactory.Create<object>();

                stopwatch.Stop();
                Debug.WriteLine($"Elapsed: {stopwatch.ElapsedMilliseconds}");
            }

            //var splashScreen = container.GetInstance<ISplashScreen>();
            //splashScreen.SetView<MainWindow>();
            //splashScreen.Show();
            //var assembliesLoader = container.GetInstance<IAssemblyLoader>();
            //assembliesLoader.LoadAssemblies();
            //var moduleLoader = container.GetInstance<IModuleLoader>();
            //await moduleLoader.LoadModules();
            //splashScreen.UpdateStatus("Fodaseeeeeeeeeeee");
        }
    }
}