using SLStudio.Core.Modules.StartPage.ViewModels;
using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class StartPageHandler : MenuCommandHandler
    {
        private readonly IShell shell;

        public StartPageHandler(IShell shell)
        {
            this.shell = shell;
        }

        public override async Task Execute(IMenuItem menu, object parameter)
        {
            await shell.OpenWorkspace<IStartPage>();
        }
    }
}