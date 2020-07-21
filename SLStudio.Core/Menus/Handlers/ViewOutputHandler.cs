using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class ViewOutputHandler : MenuCommandHandler
    {
        private readonly IShell shell;
        private readonly IOutput output;

        public ViewOutputHandler(IShell shell, IOutput output)
        {
            this.shell = shell;
            this.output = output;
        }

        public override Task Execute(IMenuItem menu, object parameter)
        {
            shell.OpenPanel(output);
            return Task.CompletedTask;
        }
    }
}