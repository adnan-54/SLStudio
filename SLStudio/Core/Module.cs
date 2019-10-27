using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using Gemini.Framework.Themes;
using Gemini.Modules.Output;
using SLStudio.Modules.Startup.ViewModels;
using SLStudio.Properties;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows;
using LogManager = SLStudio.Logging.LogManager;

namespace SLStudio.Core
{
    [Export(typeof(IModule))]
    class Module: ModuleBase
    {
        private readonly IResourceManager resourceManager;
        private readonly IThemeManager themeManager;
        private readonly IOutput output;

        [ImportingConstructor]
        public Module(IResourceManager resourceManager, IThemeManager themeManager, IOutput output)
        {
            this.resourceManager = resourceManager;
            this.themeManager = themeManager;
            this.output = output;
        }

        public override void PreInitialize()
        {
            InitializeModules();
        }

        public override void Initialize()
        {
            SetupMainWindow();
            SetupMainMenu();
            SetupShell();
        }

        public override void PostInitialize()
        {
            SetupThemeManager();
        }

        private void InitializeModules()
        {
            LogManager.Initialize(output);
        }

        private void SetupMainWindow()
        {
            MainWindow.WindowState = WindowState.Maximized;
            MainWindow.Title = Resources.MainWindow.Title;
            MainWindow.Icon = resourceManager.GetBitmap("appIcon.ico", Assembly.GetExecutingAssembly().GetAssemblyName());
        }

        private void SetupMainMenu()
        {
            MainMenu.Clear();

        }

        private void SetupShell()
        {
            SetupShellToolBars();
            SetupShellStatusBar();

            if(Settings.Default.showStartPage)
                Shell.OpenDocument(new StartPageViewModel());
        }

        private void SetupShellToolBars()
        {
            Shell.ToolBars.Visible = true;
            Shell.ToolBars.Items.Clear();
        }

        private void SetupShellStatusBar()
        {
            Shell.StatusBar.AddItem(Resources.MainWindow.StatusBar, new GridLength(1, GridUnitType.Star));
        }
        
        private void SetupThemeManager()
        {

        }
    }
}
