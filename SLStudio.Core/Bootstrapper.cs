using DevExpress.Mvvm;
using SimpleInjector;
using SLStudio.Core.Modules.ExceptionBox.Views;
using SLStudio.Core.Modules.SplashScreen.Resources;
using SLStudio.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using SplashScreen = SLStudio.Core.Modules.SplashScreen.SplashScreen;

namespace SLStudio.Core
{
    public class Bootstrapper
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<Bootstrapper>();

        private static bool initialized = false;

        private readonly Container container;
        private readonly ICommandLineArguments commandLineArguments;
        private readonly ILanguageManager languageManager;
        private readonly IThemeManager themeManager;
        private readonly IObjectFactory objectFactory;
        private readonly IUiSynchronization uiSynchronization;
        private readonly IAssemblyLoader assemblyLoader;
        private readonly IModuleLoader moduleLoader;
        private readonly IWindowManager windowManager;
        private readonly ISplashScreen splashScreen;

        private Bootstrapper(Dispatcher dispatcher, IEnumerable<string> args)
        {
            ExceptionBox.Initialize();

            container = new Container();
            commandLineArguments = new DefaultCommandLineArguments(args);
            languageManager = new DefaultLanguageManager();
            themeManager = new DefaultThemeManager();
            objectFactory = new DefaultObjectFactory(container);
            uiSynchronization = new DefaultUiSynchronization(dispatcher);
            assemblyLoader = new DefaultAssemblyLoader();
            moduleLoader = new DefaultModuleLoader();
            windowManager = new DefaultWindowManager(objectFactory);
            splashScreen = new SplashScreen();

            IoC.Initialize(container);
            LogManager.Initialize(LogLevel.Info, !commandLineArguments.DebugMode, commandLineArguments.DebugMode);

            Application.Current.Exit += OnExit;
        }

        private async Task Initialize()
        {
            logger.Info("Initializing application");
            splashScreen.Show();

            RegisterDefaults();

            assemblyLoader.Initialize(splashScreen);
            await assemblyLoader.Load();
            moduleLoader.Initialize(splashScreen, container, objectFactory);
            await moduleLoader.Load();

            splashScreen.UpdateStatus(SplashScreenResources.Initializing);

            windowManager.ShowWindow<IShell>();
            splashScreen.Close();
        }

        private void RegisterDefaults()
        {
            container.RegisterInstance(Messenger.Default);
            container.RegisterInstance(commandLineArguments);
            container.RegisterInstance(languageManager);
            container.RegisterInstance(themeManager);
            container.RegisterInstance(objectFactory);
            container.RegisterInstance(uiSynchronization);
            container.RegisterInstance(assemblyLoader);
            container.RegisterInstance(moduleLoader);
            container.RegisterInstance(windowManager);
        }

        private void OnExit(object sender, ExitEventArgs e)
        {
            logger.Info($"Exiting application with code '{e.ApplicationExitCode}'");
        }

        public static Task Run(Dispatcher dispatcher, IEnumerable<string> args)
        {
            if (initialized)
                return Task.FromResult(true);
            initialized = true;

            return new Bootstrapper(dispatcher, args).Initialize();
        }
    }
}