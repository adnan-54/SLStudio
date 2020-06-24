using DevExpress.Mvvm.UI;
using System.Windows.Controls;

namespace SLStudio.Core.Menus
{
    internal class MenuManagerService : ServiceBaseGeneric<Menu>, IMenuManager
    {
        protected override void OnAttached()
        {
            base.OnAttached();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
        }
    }

    public interface IMenuManager
    {
    }
}