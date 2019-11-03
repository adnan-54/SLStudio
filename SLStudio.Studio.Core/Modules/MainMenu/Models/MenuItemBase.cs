using Caliburn.Micro;
using System.Collections;
using System.Collections.Generic;

namespace SLStudio.Studio.Core.Modules.MainMenu.Models
{
    public class MenuItemBase : PropertyChangedBase, IEnumerable<MenuItemBase>
	{
		public static MenuItemBase Separator
		{
			get { return new MenuItemSeparator(); }
		}

		public IObservableCollection<MenuItemBase> Children { get; private set; }

		protected MenuItemBase()
		{
			Children = new BindableCollection<MenuItemBase>();
		}

		public void Add(params MenuItemBase[] menuItems)
		{
			menuItems.Apply(Children.Add);
		}

		public IEnumerator<MenuItemBase> GetEnumerator()
		{
			return Children.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}