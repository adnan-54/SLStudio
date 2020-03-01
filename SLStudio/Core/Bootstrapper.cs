using Caliburn.Micro;
using MahApps.Metro;
using SLStudio.Core.Services.BootstrapperService;
using SLStudio.Core.Utilities.DependenciesContainer;
using SLStudio.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace SLStudio.Core
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly Container container;

        public Bootstrapper()
        {
            container = new Container();

            ApplyLanguage();
            Initialize();
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            var commandLineParser = IoC.Get<ICommandLineArguments>();
            commandLineParser.ParseArguments(e.Args);

            var windowManager = IoC.Get<IWindowManager>();

            ApplyTheme();

            try
            {
                await windowManager.ShowWindow<ISplashScreen>();

                var bootstrapper = IoC.Get<IBootstrapperService>();
                await bootstrapper.Initialize();
                await windowManager.ShowWindow<IShell>();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                await windowManager.CloseWindow<ISplashScreen>();
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

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var originalSender = e.Exception.InnerException?.TargetSite.ReflectedType.Name;
            var title = $"({originalSender}) | {e.Exception.Message}";
            var logger = IoC.Get<ILoggingFactory>()?.GetLoggerFor<Bootstrapper>();

            if (logger != null)
                logger?.Fatal(e.Exception.ToString(), title);
            else
                MessageBox.Show(e.Exception.ToString(), $"Fatal: {title}", MessageBoxButton.OK, MessageBoxImage.Error);

            e.Handled = true;
            base.OnUnhandledException(sender, e);
        }

        private void ApplyTheme()
        {
            var baseColorScheme = Settings.Default.BaseColorScheme;
            var colorScheme = Settings.Default.ColorScheme;
            var themeResource = ThemeManager.Themes.FirstOrDefault(theme => theme.BaseColorScheme == baseColorScheme && theme.ColorScheme == colorScheme);

            if (themeResource != null)
                ThemeManager.ChangeTheme(Application.Current, themeResource);
        }

        private void ApplyLanguage()
        {
            CultureInfo culture = new CultureInfo(Settings.Default.LanguageCode);
            culture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }
    }
}