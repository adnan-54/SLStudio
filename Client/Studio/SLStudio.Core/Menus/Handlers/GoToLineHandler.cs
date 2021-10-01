using System.Threading.Tasks;

namespace SLStudio.Core.Menus.Handlers
{
    internal class GoToLineHandler : MenuCommandHandler
    {
        public override bool CanExecute(IMenuItem menu, object parameter)
        {
            return false;
        }

        public override Task Execute(IMenuItem menu, object parameter)
        {
            return Task.CompletedTask;
        }
    }
}