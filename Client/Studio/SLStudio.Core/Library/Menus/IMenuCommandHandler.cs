using System.Threading.Tasks;

namespace SLStudio.Core.Menus
{
    public abstract class MenuCommandHandler : IMenuCommandHandler
    {
        public virtual bool CanExecute(IMenuItem menu, object parameter)
        {
            return true;
        }

        public virtual void OnToggle(IMenuItem menu, object parameter)
        {
        }

        public abstract Task Execute(IMenuItem menu, object parameter);
    }

    public interface IMenuCommandHandler
    {
        public bool CanExecute(IMenuItem menu, object parameter);

        public void OnToggle(IMenuItem menu, object parameter);

        public Task Execute(IMenuItem menu, object parameter);
    }
}