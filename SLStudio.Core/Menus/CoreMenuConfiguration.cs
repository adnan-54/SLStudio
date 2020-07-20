using SLStudio.Core.Menus.Handlers;
using SLStudio.Core.Resources.Language;

namespace SLStudio.Core.Menus
{
    internal class CoreMenuConfiguration : MenuConfiguration
    {
        public static string Tools = "tools";
        public static string ToolsOptions = $"{Tools}/options";

        public override void Create()
        {
            Item(Tools, 0, Language.MenuTools);
            Item<ShowOptionsHandler>(ToolsOptions, 999, Language.MenuOptions, iconSource: "SettingsOutline");
        }

        public CoreMenuConfiguration(IMenuItemFactory menuItemFactory) : base(menuItemFactory)
        {
        }
    }
}