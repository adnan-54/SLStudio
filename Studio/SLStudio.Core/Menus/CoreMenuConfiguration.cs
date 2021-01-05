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
        public static string FileSeparator = $"{File}/separator";
        public static string FileSave = $"{File}/save";
        public static string FileSaveAs = $"{File}/saveAs";
        public static string FileSaveAll = $"{File}/saveAll";

        public static string View = "view";
        public static string ViewOutput = $"{View}/output";
        public static string ViewToolBox = $"{View}/toolbox";

        public static string Tools = "tools";
        public static string ToolsOptions = $"{Tools}/options";

        public static string Window = "window";

        public static string Help = "help";
        public static string HelpLogs = $"{Help}/logs";

        public override void Create()
        {
            Item(File, 0, StudioResources.File);
            Item(FileNew, 0, StudioResources.New);
            Item<CreateNewFileHandler>(FileNewFile, 0, StudioResources.NewFileMenu, iconSource: "NewFile", shortcut: new KeyGesture(Key.N, ModifierKeys.Control));
            Separator(FileSeparator, 1);
            Item<SaveFileHandler>(FileSave, 2, "Save", iconSource: "NewFile", shortcut: new KeyGesture(Key.S, ModifierKeys.Control));
            Item<SaveFileAsHandler>(FileSaveAs, 3, "Save As", iconSource: "NewFile", shortcut: new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));
            Item<SaveAllFilesHandler>(FileSaveAll, 4, "Save All", iconSource: "NewFile", shortcut: new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Alt));

            Item(View, 1, StudioResources.View);
            Item<ViewOutputHandler>(ViewOutput, 0, StudioResources.Output, iconSource: "Output");
            Item<ViewToolboxHandler>(ViewToolBox, 1, StudioResources.Toolbox, iconSource: "Toolbox");

            Item(Tools, 2, StudioResources.MenuTools);
            Item<ShowOptionsHandler>(ToolsOptions, 999, StudioResources.MenuOptions, iconSource: "Settings");

            Item(Window, 3, StudioResources.Window);

            Item(Help, 4, StudioResources.Help);
            Item<ViewLogsHandler>(HelpLogs, 0, StudioResources.menu_Logs, iconSource: "Log");
        }

        public CoreMenuConfiguration(IMenuItemFactory menuItemFactory) : base(menuItemFactory)
        {
        }
    }
}