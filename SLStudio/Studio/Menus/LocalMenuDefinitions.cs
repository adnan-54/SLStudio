using Gemini.Framework.Menus;
using Gemini.Modules.MainMenu;
using SLStudio.Studio.Menus.Definition;
using System.ComponentModel.Composition;

namespace SLStudio.Studio.Menus
{
    public static class LocalMenuDefinitions
    {
        [Export]
        public static MenuItemDefinition ViewOutputMenuItem = new CommandMenuItemDefinition<ViewToolboxDefinition>(MenuDefinitions.ViewToolsMenuGroup, 1);
    }
}
