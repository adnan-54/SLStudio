using Caliburn.Micro;
using SLStudio.Core.Docking;
using SLStudio.Core.Modules.StartPage.ViewModels;
using System.Linq;

namespace SLStudio.Core.Modules.Shell.ViewModels
{
    internal class ShellViewModel : ViewModel, IShell
    {
        public ShellViewModel(IMainMenu mainMenu, IToolBar toolBar, IStatusBar statusBar, ICommandLineArguments commandLineArguments)
        {
            MainMenu = mainMenu;
            ToolBar = toolBar;
            StatusBar = statusBar;

            Documents = new BindableCollection<IDocument>();
            Tools = new BindableCollection<ITool>();

            DisplayName = "SLStudio";
            if (commandLineArguments.DebugMode)
                DisplayName += " (debug)";

            Activate(new StartPageViewModel());
        }

        public IMainMenu MainMenu
        {
            get => GetProperty(() => MainMenu);
            set => SetProperty(() => MainMenu, value);
        }

        public IToolBar ToolBar
        {
            get => GetProperty(() => ToolBar);
            set => SetProperty(() => ToolBar, value);
        }

        public IStatusBar StatusBar
        {
            get => GetProperty(() => StatusBar);
            set => SetProperty(() => StatusBar, value);
        }

        public IDocument CurrentDocument
        {
            get => GetProperty(() => CurrentDocument);
            set => SetProperty(() => CurrentDocument, value);
        }

        public BindableCollection<IDocument> Documents { get; }

        public BindableCollection<ITool> Tools { get; }

        public void Activate(object item)
        {
            if (item is ITool tool)
            {
                if (!Tools.Any(t => t == tool))
                    Tools.Add(tool);
                tool.IsVisible = true;
                tool.IsSelected = true;
            }
            else if (item is IDocument document)
            {
                if (!Documents.Any(d => d == document))
                {
                    Documents.Add(document);
                }
                CurrentDocument = document;
                document.IsSelected = true;
            }
        }
    }
}