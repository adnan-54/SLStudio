using SLStudio.Core.Menus.Handlers;
using SLStudio.Core.Menus.Resources;
using SLStudio.Core.Resources;
using System.Windows.Input;

namespace SLStudio.Core.Menus
{
    internal class CoreMenuConfiguration : MenuConfiguration
    {
        public CoreMenuConfiguration(IMenuItemFactory menuItemFactory) : base(menuItemFactory)
        {
            SetResourceContext(typeof(MenuResources));
        }

        public override void Create()
        {
            //file
            Item("file");

            Item("file/new");
            Item<CreateNewFileHandler>("file/new/newFile", iconSource: "NewFile", shortcut: new KeyGesture(Key.N, ModifierKeys.Control));

            Item("file/open");
            Item<OpenFileHandler>("file/open/openFile", iconSource: "OpenFolder", shortcut: new KeyGesture(Key.O, ModifierKeys.Control));
            Item<StartPageHandler>("file/startPage", iconSource: "ShowStartPage");

            Separator("file/separator1");

            Item<CloseFileHandler>("file/close", shortcut: new KeyGesture(Key.W, ModifierKeys.Control));

            Separator("file/separator2");

            Item<SaveFileHandler>("file/save", iconSource: "Save", shortcut: new KeyGesture(Key.S, ModifierKeys.Control));
            Item<SaveFileAsHandler>("file/saveAs", iconSource: "SaveAs", shortcut: new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift));
            Item<SaveAllFilesHandler>("file/saveAll", iconSource: "SaveAll", shortcut: new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Alt));

            //edit
            Item("edit");

            Item("edit/goto");
            Item<OpenFileHandler>("edit/goto/gotoline", shortcut: new KeyGesture(Key.G, ModifierKeys.Control));

            //view
            Item("view");

            Item<ViewOutputHandler>("view/output", iconSource: "Output");
            Item<ViewToolboxHandler>("view/toolbox", iconSource: "Toolbox");

            //tool
            Item("tools");

            Item<ShowOptionsHandler>("tools/options", 999, iconSource: "Settings");

            //window
            Item("window");

            //help
            Item("help");

            Item<ViewLogsHandler>("help/logs", iconSource: "Log");
        }
    }
}