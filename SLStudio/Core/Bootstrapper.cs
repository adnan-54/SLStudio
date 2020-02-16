using Caliburn.Micro;
using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using SLStudio.Core.Modules.MainWindow.ViewModels;
using SLStudio.Core.Modules.SplashScreen.ViewModels;
using SLStudio.Core.Services.BootstrapperService;
using SLStudio.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
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

            ApplyUserLanguage();
            Initialize();
        }

        private static void ApplyUserLanguage()
        {
            CultureInfo culture = new CultureInfo(Settings.Default.LanguageCode);
            culture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            ApplyUserTheme();

            var bootstrapperService = IoC.Get<IBootstrapperService>();
            await bootstrapperService.Initialize();

            await DisplayRootViewFor<IMainWindow>();

            var splashScreen = IoC.Get<ISplashScreen>();
            splashScreen.Close();
        }

        private void ApplyUserTheme()
        {
            string themeAccent = Settings.Default.ThemeAccent;
            string themeBase = Settings.Default.ThemeBase;
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(themeAccent), ThemeManager.GetAppTheme(themeBase));
        }

        protected override void Configure()
        {
            container.Instance(container);
            container.Singleton<IWindowManager, WindowManager>();
            container.Singleton<IEventAggregator, EventAggregator>();
            container.Singleton<IDialogCoordinator, DialogCoordinator>();
            container.Singleton<IBootstrapperService, DefaultBootstrapperService>();
            container.Singleton<IMainWindow, MainWindowViewModel>();
            container.Singleton<ISplashScreen, SplashScreenViewModel>();
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
            base.OnUnhandledException(sender, e);
            var logger = IoC.Get<ILoggingFactory>()?.GetLoggerFor<Bootstrapper>();

#if DEBUG
            logger?.Error(e.Exception);
            e.Handled = true;
#else
            logger?.Fatal(e.Exception);
            Application.Current.Shutdown();
#endif
        }
    }
}