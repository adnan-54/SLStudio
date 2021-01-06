using SLStudio.Core.Menus.Handlers;
using SLStudio.Core.Resources;
using System.Windows.Input;

namespace SLStudio.Core.Menus
{
    internal class CoreMenuConfiguration : MenuConfiguration
    {
        public override void Create()
        {
            //file
            Item("file", 0, StudioResources.File);

            Item("file/new", 0, StudioResources.New);
            Item<CreateNewFileHandler>("file/new/newFile", 0, StudioResources.NewFileMenu, iconSource: "NewFile", shortcut: new KeyGesture(Key.N, ModifierKeys.Control));

            Item("file/open", 1, "Open", iconSource: "NewFile");
            Item<OpenFileHandler>("file/open/openFile", 0, "Open...", iconSource: "NewFile", shortcut: new KeyGesture(Key.O, ModifierKeys.Control));

            Separator("file/separator", 2);

            Item<SaveFileHandler>("file/save", 3, "Save", iconSource: "NewFile", shortcut: new KeyGesture(Key.S, ModifierKeys.Control));
            Item<SaveFileAsHandler>("file/saveAs", 4, "Save As", iconSource: "NewFile", shortcut: new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));
            Item<SaveAllFilesHandler>("file/saveAll", 5, "Save All", iconSource: "NewFile", shortcut: new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Alt));

            //view
            Item("view", 1, StudioResources.View);

            Item<ViewOutputHandler>("view/output", 0, StudioResources.Output, iconSource: "Output");
            Item<ViewToolboxHandler>("view/toolbox", 1, StudioResources.Toolbox, iconSource: "Toolbox");

            //tool
            Item("tool", 2, StudioResources.MenuTools);

            Item<ShowOptionsHandler>("tool/options", 999, StudioResources.MenuOptions, iconSource: "Settings");

            //window
            Item("window", 3, StudioResources.Window);

            //help
            Item("help", 4, StudioResources.Help);

            Item<ViewLogsHandler>("help/logs", 0, StudioResources.menu_Logs, iconSource: "Log");
        }

        public CoreMenuConfiguration(IMenuItemFactory menuItemFactory) : base(menuItemFactory)
        {
        }
    }
}