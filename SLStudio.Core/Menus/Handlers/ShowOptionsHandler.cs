using System.Threading.Tasks;
using System.Windows;

namespace SLStudio.Core.Menus.Handlers
{
    internal class ShowOptionsHandler : MenuCommandHandler
    {
        public override Task Execute(IMenuItem menu, object parameter)
        {
            MessageBox.Show(menu.DisplayName);
            return Task.CompletedTask;
        }
    }
}