using SLStudio;
using System;
using System.Windows;

namespace WpfApp1
{
    internal static class Program
    {
        [STAThread]
        public static void Main(params string[] args)
        {
            ISplashScreenView splashScreen = new SplashScreen();
            splashScreen.Show();
            var app = new App();
            var bootstrapper = Bootstrapper.CreateBootstrapper(app);

            var splashScreenService = bootstrapper.GetService<ISplashScreen>();
            splashScreenService.SetView(splashScreen);
            splashScreenService.UpdateStatus("aoo bubina");
            Application.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            app.Run();
        }
    }
}
