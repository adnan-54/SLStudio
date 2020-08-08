using SLStudio.Core.Menus.Handlers;
using SLStudio.Core.Resources;
using System.Windows.Input;

namespace SLStudio.Core.Menus
{
    internal class CoreMenuConfiguration : MenuConfiguration
    {
        public static string File = "file";
        public static string FileNew = $"{File}/new";
        public static string FileNewFile = $"{FileNew}/newFile";

        public static string View = "view";
        public static string ViewOutput = $"{View}/output";
        public static string ViewToolBox = $"{View}/toolbox";

        public static string Tools = "tools";
        public static string ToolsOptions = $"{Tools}/options";

        public static string Window = "window";
        public static string Help = "help";

        public override void Create()
        {
            Item(File, 0, StudioResources.File);
            Item(FileNew, 0, StudioResources.New);
            Item<CreateNewFileHandler>(FileNewFile, 0, StudioResources.NewFileMenu, iconSource: "NewFile", shortcut: new KeyGesture(Key.N, ModifierKeys.Control));

            Item(View, 1, StudioResources.View);
            Item<ViewOutputHandler>(ViewOutput, 0, StudioResources.Output, iconSource: "Output");
            Item<ViewToolboxHandler>(ViewToolBox, 1, StudioResources.Toolbox, iconSource: "Toolbox");

            Item(Tools, 2, StudioResources.MenuTools);
            Item<ShowOptionsHandler>(ToolsOptions, 999, StudioResources.MenuOptions, iconSource: "Settings");

            Item(Window, 3, StudioResources.Window);

            Item(Help, 4, StudioResources.Help);
        }

        public CoreMenuConfiguration(IMenuItemFactory menuItemFactory) : base(menuItemFactory)
        {
        }
    }
}