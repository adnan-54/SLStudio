using DevExpress.Mvvm;

namespace SLStudio
{
    public abstract class MenuItemHandler<TMenuItem> : BindableBase, IMenuItemHandler<TMenuItem>
        where TMenuItem : class, IMenuItem
    {
        public TMenuItem Menu { get; private set; }

        void IMenuItemHandler<TMenuItem>.SetMenu(TMenuItem menu)
        {
            if (Menu is not null)
                return;
            Menu = menu;
        }
    }
}
