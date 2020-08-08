using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class CreateNewFileHandler : MenuCommandHandler
    {
        private readonly IWindowManager windowManager;

        public CreateNewFileHandler(IWindowManager windowManager)
        {
            this.windowManager = windowManager;
        }

        public override Task Execute(IMenuItem menu, object parameter)
        {
            windowManager.ShowDialog<INewFileDialog>();
            return Task.CompletedTask;
        }
    }
}