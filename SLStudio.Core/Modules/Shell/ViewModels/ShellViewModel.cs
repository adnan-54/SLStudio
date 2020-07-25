using SLStudio.Core.Modules.StartPage.ViewModels;
using System.Linq;

namespace SLStudio.Core.Modules.Shell.ViewModels
{
    internal class ShellViewModel : WindowViewModel, IShell
    {
        private readonly ICommandLineArguments commandLineArguments;
        private readonly IFileService fileService;

        public ShellViewModel(IStatusBar statusBar, ICommandLineArguments commandLineArguments, IObjectFactory objectFactory, IRecentFilesRepository recentFilesRepository)
        {
            this.commandLineArguments = commandLineArguments;

            StatusBar = statusBar;
            Documents = new BindableCollection<IDocumentPanel>();
            Tools = new BindableCollection<IToolPanel>();

            fileService = new DefaultFileService(this, objectFactory, recentFilesRepository);
            ServiceContainer.RegisterService(fileService);

            DisplayName = DebugMode ? "SLStudio (debug mode)" : "SLStudio";
        }

        public bool DebugMode => commandLineArguments.DebugMode;

        public BindableCollection<IDocumentPanel> Documents { get; }

        public BindableCollection<IToolPanel> Tools { get; }

        public IStatusBar StatusBar
        {
            get => GetProperty(() => StatusBar);
            set => SetProperty(() => StatusBar, value);
        }

        public IWorkspacePanel SelectedItem
        {
            get => GetProperty(() => SelectedItem);
            set => SetProperty(() => SelectedItem, value);
        }

        public void OpenPanel(IWorkspacePanel item)
        {
            if (item is IDocumentPanel documentPanel)
            {
                if (!Documents.Any(d => d == documentPanel))
                    Documents.Add(documentPanel);
                documentPanel.Activate();
            }
            else
            if (item is IToolPanel toolPanel)
            {
                if (!Tools.Any(t => t == toolPanel))
                    Tools.Add(toolPanel);
                toolPanel.Activate();
            }
        }

        public void ClosePanel(IWorkspacePanel item)
        {
            if (item is IDocumentPanel documentPanel && Documents.Any(d => d == documentPanel))
                Documents.Remove(documentPanel);
            else
            if (item is IToolPanel toolPanel && Tools.Any(t => t == toolPanel))
                toolPanel.Close();
        }

        public override void OnLoaded()
        {
            if (!Documents.Any())
            {
                var startPage = new StartPageViewModel();
                Documents.Add(startPage);
                startPage.Activate();
            }
        }
    }
}