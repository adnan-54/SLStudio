using System.Collections.Generic;
using System.Linq;

namespace SLStudio
{
    internal class WorkspaceViewModel : ViewModel, IWorkspace
    {
        private readonly IObjectFactory objectFactory;
        private readonly BindableCollection<IWorkspaceDocument> documents;
        private readonly BindableCollection<IWorkspaceTool> tools;
        private IWorkspaceService workspaceService;

        public WorkspaceViewModel(IObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
            documents = new();
            tools = new();
        }

        public IEnumerable<IWorkspaceItem> Workspaces => Documents.Cast<IWorkspaceItem>().Concat(Tools).ToList();

        public IEnumerable<IWorkspaceDocument> Documents => documents;

        public IEnumerable<IWorkspaceTool> Tools => tools;

        public IWorkspaceDocument LastFocusedDocument => Documents.FirstOrDefault(d => d.IsLastFocusedDocument);

        public IWorkspaceItem SelectedWorkspace
        {
            get => GetValue<IWorkspaceItem>();
            set => SetValue(value);
        }

        protected IWorkspaceService WorkspaceService => workspaceService ??= GetService<IWorkspaceService>();

        TWorkspace IWorkspace.Show<TWorkspace>()
        {
            var workspace = objectFactory.Create<TWorkspace>();
            return ShowCore(workspace) as TWorkspace;
        }

        public IEnumerable<IWorkspaceItem> Show(params IWorkspaceItem[] workspaces)
        {
            foreach (var workspace in workspaces)
                yield return ShowCore(workspace);
        }

        public void Close(params IWorkspaceItem[] workspaces)
        {
        }

        private IWorkspaceItem ShowCore(IWorkspaceItem workspace)
        {
            return workspace;
        }
    }
}
