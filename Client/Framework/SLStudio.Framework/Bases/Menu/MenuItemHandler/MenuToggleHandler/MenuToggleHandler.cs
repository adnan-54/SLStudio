using System.Threading.Tasks;

namespace SLStudio
{
	public abstract class MenuToggleHandler : MenuItemHandler<IMenuToggle>, IMenuToggleHandler
	{
		public virtual Task OnToggle()
		{
			return Task.CompletedTask;
		}
	}
}
