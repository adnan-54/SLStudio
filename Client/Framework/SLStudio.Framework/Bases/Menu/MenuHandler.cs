using System.Threading.Tasks;

namespace SLStudio
{
    public abstract class MenuHandler<TItem> where TItem : IMenuItem
    {
        protected virtual bool CanExecute(TItem item, object parameter)
        {
            return true;
        }

        protected abstract Task Execute(TItem item, object parameter);
    }
}