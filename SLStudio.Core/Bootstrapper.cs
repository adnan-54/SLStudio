using Caliburn.Micro;
using SLStudio.Core.Properties;
using SLStudio.Core.Services.BootstrapperService;
using SLStudio.Core.Utilities.DependenciesContainer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SLStudio.Core
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly Container container;
        private ILogger logger;

        public Bootstrapper()
        {
            container = new Container();
            ApplyCulture();
            Initialize();
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                container.GetInstance<ICommandLineArguments>().ParseArguments(e.Args);
                logger = container.GetInstance<ILoggingFactory>().GetLoggerFor<Bootstrapper>();
                logger.Debug("Initializing application");

                ApplyTheme();

                await container.GetInstance<IWindowManager>().ShowWindow<ISplashScreen>();
                await container.GetInstance<IBootstrapperService>().Initialize();
                await container.GetInstance<IWindowManager>().ShowWindow<IShell>();
            }
            catch (Exception ex)
            {
                logger.Fatal(ex);
                Application.Current.Shutdown();
            }
            finally
            {
                var windowManager = container.GetInstance<IWindowManager>();
                if (windowManager != null)
                    await windowManager.CloseWindow<ISplashScreen>();
                else
                {
                    if (logger != null)
                        logger.Fatal("DefaultWindowManger not found");

                    Application.Current.Shutdown();
                }
            }
        }

        protected override void Configure()
        {
            var coreModule = new Module();
            coreModule.Register(container);
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            if (logger != null)
                logger.Debug("Exiting application");
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var originalSender = e.Exception.FindOriginalSource();
            var title = $"({originalSender}) | {e.Exception.Message}";

            if (logger != null)
                logger.Fatal(e.Exception.ToString(), title);
            else
                MessageBox.Show(e.Exception.ToString(), $"Fatal: {title}", MessageBoxButton.OK, MessageBoxImage.Error);

            e.Handled = true;
            base.OnUnhandledException(sender, e);
        }

        private void ApplyTheme()
        {
            var themeManager = container.GetInstance<IThemeManager>();
            var theme = themeManager.AvaliableThemes.First();
            themeManager.SetTheme(theme);
        }

        private void ApplyCulture()
        {
            if (string.IsNullOrEmpty(Settings.Default.LanguageCode))
                return;

            var culture = new CultureInfo(Settings.Default.LanguageCode);
            //culture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}