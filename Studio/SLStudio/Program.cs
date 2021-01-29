using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;

namespace SLStudio
{
    internal static class Program
    {
        private static Mutex mutex;

        [STAThread()]
        public static void Main()
        {
            try
            {
                mutex = new Mutex(true, SLStudioConstants.GlobalKey);

                if (mutex.WaitOne(TimeSpan.FromMilliseconds(100), false))
                    Run();
                else
                    SendToCurrentInstance();
            }
            finally
            {
                if (mutex != null)
                    mutex.ReleaseMutex();
            }
        }

        private static void Run()
        {
            var splashScreen = new SplashScreen("splashscreen.png");
            splashScreen.Show(true);
            App app = new App();
            app.Startup += AppStartup;
            app.InitializeComponent();
            app.Run();
        }

        private static void AppStartup(object sender, StartupEventArgs e)
        {
            var server = InternalServer.Create();
            server.Start();
            server.MessageRecived += (s, e) => Debug.WriteLine($"sender: {s}, date:{DateTime.Now}, args:{e}");
        }

        private static void SendToCurrentInstance()
        {
            var args = string.Join(';', Environment.GetCommandLineArgs().Skip(1));
            InternalClient.SendMessage(args);
        }
    }
}