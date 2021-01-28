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

        private readonly Container container;
        private readonly ICommandLineArguments commandLineArguments;
        private readonly ILanguageManager languageManager;
        private readonly IThemeManager themeManager;
        private readonly IObjectFactory objectFactory;
        private readonly IUiSynchronization uiSynchronization;
        private readonly IAssemblyLookup assemblyLoader;
        private readonly IModuleLookup moduleLoader;
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
            assemblyLoader = new DefaultAssemblyLookup();
            moduleLoader = new DefaultModuleLookup();
            windowManager = new DefaultWindowManager(objectFactory);
            splashScreen = new SplashScreen();

            IoC.Initialize(container);

            var loggerConfiguration = new LogManagerConfiguration()
            {
                DefaultLogLevel = LogLevel.Info,
                IgnoreDebugLevel = !commandLineArguments.DebugMode,
                LogToConsole = commandLineArguments.DebugMode,
                MaxRetrieveResults = 0
            };

            LogManager.Configure(loggerConfiguration);

            Application.Current.Exit += OnExit;
        }

        private async Task Initialize()
        {
            logger.Info("Initializing application");
            splashScreen.Show();

            ConfigureContainer();
            RegisterDefaults();

            assemblyLoader.Initialize(splashScreen);
            await assemblyLoader.Load();
            moduleLoader.Initialize(splashScreen, container, objectFactory);
            await moduleLoader.Load();

            splashScreen.UpdateStatus(SplashScreenResources.Initializing);

            windowManager.ShowWindow<IShell>();
            splashScreen.Close();
        }

        private void ConfigureContainer()
        {
            container.Options.ResolveUnregisteredConcreteTypes = true;
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

        public static void Run(Dispatcher dispatcher, IEnumerable<string> args)
        {
            new Bootstrapper(dispatcher, args).Initialize().FireAndForget();
        }
    }
}