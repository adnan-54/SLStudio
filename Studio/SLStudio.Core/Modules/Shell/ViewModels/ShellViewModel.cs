using GongSolutions.Wpf.DragDrop;
using SLStudio.Core.Docking;
using SLStudio.Core.Modules.StartPage.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    internal class ShellViewModel : WindowViewModel, IShell
    {
        private readonly IObjectFactory objectFactory;
        private readonly IUiSynchronization uiSynchronization;
        private readonly IOutput output;
        private IDockingService dockingService;

        public ShellViewModel(IObjectFactory objectFactory, IUiSynchronization uiSynchronization,
                              ICommandLineArguments commandLineArguments, IOutput output, IStatusBar statusBar)
        {
            this.objectFactory = objectFactory;
            this.uiSynchronization = uiSynchronization;
            this.output = output;
            StatusBar = statusBar;

            Documents = new BindableCollection<IDocumentItem>();
            Tools = new BindableCollection<IToolItem>();
            DragDropHandler = new ShellDragDropHandler();

            DisplayName = commandLineArguments.DebugMode ? "SLStudio - (debug mode)" : "SLStudio";
        }

        public IReadOnlyCollection<IWorkspaceItem> Workspaces => Documents.Cast<IWorkspaceItem>().Concat(Tools).ToList();

        public BindableCollection<IDocumentItem> Documents { get; }

        public BindableCollection<IToolItem> Tools { get; }

        public IWorkspaceItem ActiveWorkspace
        {
            get => GetProperty(() => ActiveWorkspace);
            set => SetProperty(() => ActiveWorkspace, value);
        }

        public IStatusBar StatusBar
        {
            get => GetProperty(() => StatusBar);
            set => SetProperty(() => StatusBar, value);
        }

        public IDropTarget DragDropHandler { get; }

        public async Task<T> AddWorkspace<T>() where T : class, IWorkspaceItem
        {
            var workspace = objectFactory.Create<T>();
            await AddWorkspaces(workspace);

            return workspace;
        }

        public async Task<T> OpenWorkspace<T>() where T : class, IWorkspaceItem
        {
            var workspace = objectFactory.Create<T>();
            await OpenWorkspaces(workspace);

            return workspace;
        }

        public Task AddWorkspaces(params IWorkspaceItem[] workspaces)
        {
            return uiSynchronization.EnsureExecuteOnUiAsync(() =>
            {
                foreach (var item in workspaces)
                    EnsureAddWorkspace(item);
            });
        }

        public Task OpenWorkspaces(params IWorkspaceItem[] workspaces)
        {
            return uiSynchronization.EnsureExecuteOnUiAsync(() =>
            {
                foreach (var item in workspaces)
                    EnsureAddWorkspace(item);

                ActiveWorkspace = workspaces.LastOrDefault();
                ActiveWorkspace?.Activate();
                dockingService?.FocusItem(ActiveWorkspace);
            });
        }

        public Task CloseWorkspaces(params IWorkspaceItem[] workspaces)
        {
            return uiSynchronization.EnsureExecuteOnUiAsync(() =>
            {
                foreach (var item in workspaces)
                    EnsureRemoveWorkspace(item);

                dockingService?.FocusItem();
            });
        }

        protected override void OnLoaded()
        {
            dockingService = GetService<IDockingService>();

            if (!Documents.Any())
                OpenWorkspace<IStartPage>().FireAndForget();
            output.Clear();
        }

        private void EnsureAddWorkspace(IWorkspaceItem item)
        {
            if (item is IToolItem tool && !Tools.Contains(tool))
                Tools.Add(tool);
            else
            if (item is IDocumentItem document && !Documents.Contains(document))
                Documents.Add(document);
        }

        private void EnsureRemoveWorkspace(IWorkspaceItem item)
        {
            if (item is IToolItem tool && Tools.Contains(tool))
            {
                dockingService?.CloseFromId(tool.Id);
                tool.Close();
            }
            else
            if (item is IDocumentItem document && Documents.Contains(document))
            {
                document.Close();
                dockingService?.CloseFromId(document.Id);
                Documents.Remove(document);
            }
        }
    }

    public interface IShell : IWindow
    {
        IReadOnlyCollection<IWorkspaceItem> Workspaces { get; }

        IWorkspaceItem ActiveWorkspace { get; }

        Task<T> AddWorkspace<T>() where T : class, IWorkspaceItem;

        Task<T> OpenWorkspace<T>() where T : class, IWorkspaceItem;

        Task AddWorkspaces(params IWorkspaceItem[] workspaces);

        Task OpenWorkspaces(params IWorkspaceItem[] workspaces);

        Task CloseWorkspaces(params IWorkspaceItem[] workspaces);
    }
}