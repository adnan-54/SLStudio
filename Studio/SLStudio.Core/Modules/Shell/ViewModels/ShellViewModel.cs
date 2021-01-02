using SLStudio.Core.Modules.StartPage.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace SLStudio.Core.Modules.Shell.ViewModels
{
    internal class ShellViewModel : WindowViewModel, IShell
    {
        private readonly IUiSynchronization uiSynchronization;

        public ShellViewModel(IUiSynchronization uiSynchronization, ICommandLineArguments commandLineArguments, IStatusBar statusBar)
        {
            this.uiSynchronization = uiSynchronization;

            StatusBar = statusBar;
            Documents = new BindableCollection<IDocumentPanel>();
            Tools = new BindableCollection<IToolPanel>();

            DisplayName = commandLineArguments.DebugMode ? "SLStudio - (debug mode)" : "SLStudio";
        }

        public BindableCollection<IDocumentPanel> Documents { get; }

        public BindableCollection<IToolPanel> Tools { get; }

        public IStatusBar StatusBar
        {
            get => GetProperty(() => StatusBar);
            set => SetProperty(() => StatusBar, value);
        }

        public IPanelItem SelectedItem
        {
            get => GetProperty(() => SelectedItem);
            set => SetProperty(() => SelectedItem, value);
        }

        public async Task OpenPanel(IPanelItem item)
        {
            if (item is IDocumentPanel documentPanel && !Documents.Any(d => d == documentPanel))
                Documents.Add(documentPanel);
            else
            if (item is IToolPanel toolPanel && !Tools.Any(t => t == toolPanel))
                Tools.Add(toolPanel);

            await uiSynchronization.EnsureExecuteOnUiAsync(() => item.Activate());
        }

        public async Task ClosePanel(IPanelItem item)
        {
            if (item is IDocumentPanel documentPanel && Documents.Any(d => d == documentPanel))
                await uiSynchronization.EnsureExecuteOnUiAsync(() => Documents.Remove(documentPanel));
            else
            if (item is IToolPanel toolPanel && Tools.Any(t => t == toolPanel))
                await uiSynchronization.EnsureExecuteOnUiAsync(() => toolPanel.Hide());
        }

        public override void OnLoaded()
        {
            if (!Documents.Any())
            {
                var startPage = new StartPageViewModel();
                OpenPanel(startPage).FireAndForget();
            }
        }
    }
}