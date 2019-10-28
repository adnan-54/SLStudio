using Gemini.Framework;
using SLStudio.Studio.Modules.StartPage.ViewModels;
using System.ComponentModel.Composition;
using System.Windows;

namespace SLStudio.Studio.Core
{
    [Export(typeof(IModule))]
    public class Module : ModuleBase
    {
        public override void Initialize()
        {
            MainWindow.WindowState = WindowState.Maximized;

            Shell.OpenDocument(new StartPageViewModel());

            base.Initialize();
        }
    }
}
