using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class ViewToolboxHandler : MenuCommandHandler
    {
        private readonly IShell shell;
        private readonly IToolbox toolbox;

        public ViewToolboxHandler(IShell shell, IToolbox toolbox)
        {
            this.shell = shell;
            this.toolbox = toolbox;
        }

        public override async Task Execute(IMenuItem menu, object parameter)
        {
            await shell.OpenPanel(toolbox);
        }
    }
}