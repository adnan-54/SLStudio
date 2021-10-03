using SLStudio.Core;
using SLStudio.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Windows;

namespace SLStudio
{
    internal static class Program
    {
        private static readonly ILogger logger = LogManager.GetLogger(typeof(Program));

        private static Mutex mutex;

        [STAThread]
        public static void Main()
        {
            try
            {
                mutex = new Mutex(true, StudioConstants.GlobalKey);

                if (mutex.WaitOne(TimeSpan.Zero, false))
                    Run();
                else
                    SendToCurrentInstance();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex);
                LogManager.RequestDump();
                throw;
            }
            finally
            {
                mutex?.ReleaseMutex();
            }
        }

        private static void Run()
        {
            StartServer();

            var splashScreen = new SplashScreen("splashscreen.png");
            splashScreen.Show(true);
            App app = new App();
            app.InitializeComponent();
            app.Run();
        }

        private static void StartServer()
        {
            var server = InternalServer.Create();
            server.Start();
            server.MessageRecived += OnMessageMessageRecived;
        }

        private static async void OnMessageMessageRecived(object sender, string e)
        {
            //todo: improve this
            //files should be queued and opened later because we need to exit the method as soon as possible
            //we shouldn't use dispatcher either
            await Application.Current?.Dispatcher?.InvokeAsync(() =>
            {
                var fileService = IoC.Get<IFileService>();
                var files = e.Split(';').ToList();
                files.ForEach(f => fileService?.Open(f));

                if (Application.Current?.MainWindow is null)
                    return;

                if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
                    SystemCommands.RestoreWindow(Application.Current.MainWindow);

                Application.Current.MainWindow.Activate();
            });
        }

        private static void SendToCurrentInstance()
        {
            var args = string.Join(';', Environment.GetCommandLineArgs().Skip(1));
            InternalClient.SendMessage(args);
        }
    }
}