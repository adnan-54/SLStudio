using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class ViewToolboxHandler : MenuCommandHandler
    {
        private readonly IShell shell;

        public ViewToolboxHandler(IShell shell)
        {
            this.shell = shell;
        }

        public override async Task Execute(IMenuItem menu, object parameter)
        {
            await shell.OpenWorkspace<IToolbox>();
        }
    }
}