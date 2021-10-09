using SLStudio.Core;
using System;
using System.Windows;

namespace SLStudio
{
    internal abstract class Startup
    {
        private readonly App application;

        protected Startup()
        {
            application = new App();
            SplashScreen = new SplashScreen();
            Args = Environment.GetCommandLineArgs();
        }

        public SplashScreen SplashScreen { get; }

        public string[] Args { get; }

        public abstract ErrorHandler ErrorHandler { get; }

        public int Start()
        {
            //todo: configure logger
            //todo: load user culture

            SplashScreen.Show();
            return Run();
        }

        protected int RunApplication()
        {
            application.Startup += OnApplicationStartup;
            return application.Run(SplashScreen);
        }

        protected abstract int Run();

        private async void OnApplicationStartup(object sender, StartupEventArgs e)
        {
            await Bootstrapper.Run(application, SplashScreen);
        }

        public static Startup GetStartup()
        {
            if (SharedConstants.DebugCompiled)
                return new DebugStartup();
            return new ReleaseStartup();
        }
    }
}