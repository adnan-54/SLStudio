using Caliburn.Micro;
using MahApps.Metro;
using SLStudio.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace SLStudio.Core
{
    public class Bootstrapper : BootstrapperBase
    {
        private readonly SimpleContainer container = new SimpleContainer();

        public Bootstrapper()
        {
            PreInitialize();
            Initialize();
        }

        private void PreInitialize()
        {
            string themeAccent = Settings.Default.ThemeAccent;
            string themeBase = Settings.Default.ThemeBase;
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(themeAccent), ThemeManager.GetAppTheme(themeBase));

            CultureInfo culture = new CultureInfo(Settings.Default.LanguageCode);
            culture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        protected override void Configure()
        {
            container.Instance(container);
            container.Singleton<ISplashScreenService, SplashScreenService>();
            container.Singleton<IBootstrapperService, BootstrapperService>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            base.OnStartup(sender, e);

            var bootstrapper = IoC.Get<IBootstrapperService>();
            bootstrapper.Initialize();
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
    }
}
