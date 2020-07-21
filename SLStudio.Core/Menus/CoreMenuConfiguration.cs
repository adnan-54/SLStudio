using SLStudio.Core.Menus.Handlers;
using System.Windows.Input;

namespace SLStudio.Core.Menus
{
    internal class CoreMenuConfiguration : MenuConfiguration
    {
        public static string File = "file";

        public static string View = "view";
        public static string ViewOutput = $"{View}/output";

        public static string Tools = "tools";
        public static string ToolsOptions = $"{Tools}/options";

        public static string Window = "window";
        public static string Help = "help";

        public override void Create()
        {
            Item(File, 0, "File");

            Item(View, 1, "View");
            Item<ViewOutputHandler>(ViewOutput, 0, "Output", shortcut: new MultiKeyGesture(new KeyGesturePart(Key.W, ModifierKeys.Control), new KeyGesturePart(Key.O)));

            Item(Tools, 2, Resources.Language.Language.MenuTools);
            Item<ShowOptionsHandler>(ToolsOptions, 999, Resources.Language.Language.MenuOptions, iconSource: "SettingsOutline");

            Item(Window, 3, "Window");
            Item(Help, 4, "Help");
        }

        public CoreMenuConfiguration(IMenuItemFactory menuItemFactory) : base(menuItemFactory)
        {
        }
    }
}