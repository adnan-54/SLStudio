using SLStudio.Core.Modules.StartPage.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SLStudio.Core.Modules.Shell.ViewModels
{
    internal class ShellViewModel : WindowViewModel, IShell
    {
        private readonly IObjectFactory objectFactory;
        private readonly IUiSynchronization uiSynchronization;
        private readonly IRecentFilesRepository recentFilesRepository;

        public ShellViewModel(IObjectFactory objectFactory, IUiSynchronization uiSynchronization,
                              ICommandLineArguments commandLineArguments, IRecentFilesRepository recentFilesRepository, IStatusBar statusBar)
        {
            this.objectFactory = objectFactory;
            this.uiSynchronization = uiSynchronization;
            this.recentFilesRepository = recentFilesRepository;
            StatusBar = statusBar;
            Documents = new BindableCollection<IDocumentItem>();
            Tools = new BindableCollection<IToolItem>();

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
                {
                    EnsureAddWorkspace(item);
                    item.Activate();
                }
            });
        }

        public Task CloseWorkspaces(params IWorkspaceItem[] workspaces)
        {
            return uiSynchronization.EnsureExecuteOnUiAsync(() =>
            {
                foreach (var item in workspaces)
                    EnsureRemoveWorkspace(item);
            });
        }

        public override void OnLoaded()
        {
            if (!Documents.Any())
                OpenWorkspace<IStartPage>().FireAndForget();
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
                Tools.Remove(tool);
            else
            if (item is IDocumentItem document && Documents.Contains(document))
                Documents.Remove(document);
        }
    }
}