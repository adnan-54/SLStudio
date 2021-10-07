using System.Threading.Tasks;

namespace SLStudio
{
	public interface IMenuItemHandler<TMenuItem>
		where TMenuItem : class, IMenuItem
	{
		TMenuItem Menu { get; }

		bool CanExecute(object parameter);

		Task Execute(object parameter);

		void SetMenu(TMenuItem menu);
	}
}
