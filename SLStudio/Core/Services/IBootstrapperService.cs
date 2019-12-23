using Caliburn.Micro;
using MahApps.Metro;
using SLStudio.Core.Framework;
using SLStudio.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace SLStudio.Core
{
    class BootstrapperService : IBootstrapperService
    {
        private readonly SimpleContainer container;
        private readonly ISplashScreenService splashScreen;
        private readonly List<IModule> modules;
        
        private bool isInitialized;
        
        public BootstrapperService(SimpleContainer container, ISplashScreenService splashScreen)
        {
            this.container = container;
            this.splashScreen = splashScreen;
            
            modules = new List<IModule>();
            isInitialized = false;
        }

        public IList<IModule> Modules => modules;

        public async Task Initialize()
        {
            if (isInitialized)
                return;

            LoadUserSettings();
            splashScreen.Show();

            await LoadModulesAsync();

            isInitialized = true;

            var windowManager = IoC.Get<IWindowManager>();
            var mainWindow = IoC.Get<IMainWindow>();
            
            splashScreen.Hide();
            windowManager.ShowWindow(mainWindow);
            splashScreen.Close();
        }

        private void LoadUserSettings()
        {
            string themeAccent = Settings.Default.ThemeAccent;
            string themeBase = Settings.Default.ThemeBase;
            ThemeManager.ChangeAppStyle(Application.Current, ThemeManager.GetAccent(themeAccent), ThemeManager.GetAppTheme(themeBase));

            CultureInfo culture = new CultureInfo(Settings.Default.LanguageCode);
            culture.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        private async Task LoadModulesAsync()
        {
            await Task.Run(() =>
            {
                GetType().Assembly.GetTypes().Where(type => type.IsClass && type.Name.Equals("Module") && type.GetInterface(nameof(IModule)) != null)
                .ToList()
                .ForEach(type =>
                {
                    var instance = Activator.CreateInstance(type) as IModule;
                    modules.Add(instance);
                });

                var orderedModules = modules.OrderByDescending(p => p.ModulePriority).ToList();
                modules.Clear();
                modules.AddRange(orderedModules);
                orderedModules.Clear();

                foreach (var module in modules)
                {
                    if (module != null && module.ShouldBeLoaded)
                    {
                        splashScreen.UpdateStatus(module.ModuleName);
                        module.Register(container);
                    }
                }
            });
        }
    }

    public interface IBootstrapperService
    {
        IList<IModule> Modules { get; }
        Task Initialize();
    }
}
