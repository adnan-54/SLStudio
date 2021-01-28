using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class ViewOutputHandler : MenuCommandHandler
    {
        private readonly IShell shell;

        public ViewOutputHandler(IShell shell)
        {
            this.shell = shell;
        }

        public override async Task Execute(IMenuItem menu, object parameter)
        {
            await shell.OpenWorkspace<IOutput>();
        }
    }
}