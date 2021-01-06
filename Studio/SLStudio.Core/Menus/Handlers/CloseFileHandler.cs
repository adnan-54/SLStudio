using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class CloseFileHandler : MenuCommandHandler
    {
        private readonly IShell shell;

        public CloseFileHandler(IShell shell)
        {
            this.shell = shell;
        }

        public override Task Execute(IMenuItem menu, object parameter)
        {
            return Task.CompletedTask;
        }
    }
}