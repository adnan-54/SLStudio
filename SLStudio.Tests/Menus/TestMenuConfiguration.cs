using SLStudio.Core.Menus;
using System.Windows.Input;

namespace SLStudio.Tests.Menus
{
    internal class TestMenuConfiguration : MenuConfiguration
    {
        public TestMenuConfiguration(IMenuItemFactory menuItemFactory) : base(menuItemFactory)
        {
        }

        public override void Create()
        {
        }
    }
}