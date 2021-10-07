using System.Threading.Tasks;

namespace SLStudio
{
	public abstract class MenuItemHandler<TMenuItem> : IMenuItemHandler<TMenuItem>
		where TMenuItem : class, IMenuItem
	{
		public TMenuItem Menu { get; private set; }

		public virtual bool CanExecute(object parameter)
		{
			return true;
		}

		public abstract Task Execute(object parameter);

		void IMenuItemHandler<TMenuItem>.SetMenu(TMenuItem menu)
		{
			if (Menu is not null)
				return;
			Menu = menu;
		}
	}
}
