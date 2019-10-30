using Caliburn.Micro;
using Gemini.Framework;
using Gemini.Framework.Services;
using SLStudio.Studio.Modules.Pages.StartPage.ViewModels;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Windows;

namespace SLStudio.Studio
{
    [Export(typeof(IModule))]
    public class Module : ModuleBase
    {
        private readonly IResourceManager resourceManager;

        [ImportingConstructor]
        public Module(IResourceManager resourceManager)
        {
            this.resourceManager = resourceManager;
        }

        public override void Initialize()
        {
            MainWindow.WindowState = WindowState.Maximized;
            MainWindow.Icon = resourceManager.GetBitmap("appIcon.ico", Assembly.GetExecutingAssembly().GetAssemblyName());
            MainWindow.Title = "";

            Shell.OpenDocument(new StartPageViewModel());

            base.Initialize();
        }
    }
}
