using Caliburn.Micro;
using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using SLStudio.Core.Modules.Shell.ViewModels;
using SLStudio.Core.Modules.SplashScreen;
using SLStudio.Core.Modules.SplashScreen.ViewModels;
using SLStudio.Core.Services.BootstrapperService;
using SLStudio.Core.Utilities.CommandLinesArguments;
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
        private readonly SimpleContainer container;

        public Bootstrapper()
        {
            container = new SimpleContainer();

            LoadUserLanguage();
            Initialize();
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            IoC.Get<ICommandLineArguments>()?.ParseArguments(e.Args);

            LoadUserTheme();

            await IoC.Get<IBootstrapperService>().Initialize();
            await DisplayRootViewFor<IShell>();
            IoC.Get<ISplashScreen>().Close();
        }

        private void LoadUserLanguage()
        {
            CultureInfo culture = new CultureInfo(Settings.Default.LanguageCode);
            culture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        private void LoadUserTheme()
        {
            var baseColorScheme = Settings.Default.BaseColorScheme;
            var colorScheme = Settings.Default.ColorScheme;
            var themeResource = ThemeManager.Themes.FirstOrDefault(theme => theme.BaseColorScheme == baseColorScheme && theme.ColorScheme == colorScheme);

            if (themeResource != null)
                ThemeManager.ChangeTheme(Application.Current, themeResource);
        }

        protected override void Configure()
        {
            container.Instance(container);
            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.Singleton<IDialogCoordinator, DialogCoordinator>();
            container.Singleton<IBootstrapperService, DefaultBootstrapperService>();
            container.Singleton<ISplashScreen, SplashScreenViewModel>();
            container.Singleton<IShell, ShellViewModel>();
            container.Singleton<ICommandLineArguments, CommandLineArguments>();
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
            logger?.Fatal(e.Exception.ToString(), title);
            e.Handled = logger != null;

            base.OnUnhandledException(sender, e);
        }
    }
}