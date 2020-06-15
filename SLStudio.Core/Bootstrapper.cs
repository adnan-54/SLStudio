using Caliburn.Micro;
using SLStudio.Core.Logging;
using SLStudio.Core.Properties;
using SLStudio.Core.Services.BootstrapperService;
using SLStudio.Core.Utilities.DependenciesContainer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace SLStudio.Core
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly Container container;
        private static readonly ILogger logger = LogManager.GetLogger(typeof(Bootstrapper));

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
                await container.GetInstance<IWindowManager>().CloseWindow<ISplashScreen>();
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
            logger.Debug("Exiting application");
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
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}