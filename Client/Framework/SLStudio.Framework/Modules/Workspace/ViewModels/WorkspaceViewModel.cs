using System.Collections.Generic;
using System.Linq;

namespace SLStudio
{
    internal class WorkspaceViewModel : ViewModel, IWorkspace
    {
        private readonly IObjectFactory objectFactory;
        private readonly BindableCollection<IWorkspaceDocument> documents;
        private readonly BindableCollection<IWorkspaceTool> tools;

        public WorkspaceViewModel(IObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
            documents = new();
            tools = new();
        }

        public IEnumerable<IWorkspaceItem> Workspaces => Documents.Cast<IWorkspaceItem>().Concat(Tools).ToList();

        public IEnumerable<IWorkspaceTool> Tools => tools;

        public IEnumerable<IWorkspaceDocument> Documents => documents;

        public IWorkspaceDocument LastFocusedDocument => Documents.FirstOrDefault(d => d.LastFocusedDocument);

        public IWorkspaceItem SelectedWorkspace
        {
            get => GetValue<IWorkspaceItem>();
            set => SetValue(value);
        }

        TWorkspace IWorkspace.Show<TWorkspace>()
        {
            var workspace = objectFactory.Create<TWorkspace>();
            ShowCore(workspace);
            workspace.Activate();
            return workspace;
        }

        public void Show(params IWorkspaceItem[] workspaces)
        {
            foreach (var workspace in workspaces)
                ShowCore(workspace);

            workspaces.Last().Activate();
        }

        public void Close(IWorkspaceItem workspace)
        {
            CloseCore(workspace);
        }

        public void Close(params IWorkspaceItem[] workspaces)
        {
            foreach (var workspace in workspaces)
                CloseCore(workspace);
        }

        private void ShowCore(IWorkspaceItem workspace)
        {
            if (workspace is IWorkspaceDocument document)
                AddDocument(document);
            else if (workspace is IWorkspaceTool tool)
                AddTool(tool);

            workspace.Show();
        }

        private void AddDocument(IWorkspaceDocument document)
        {
            if (!documents.Contains(document))
                documents.Add(document);
        }

        private void AddTool(IWorkspaceTool tool)
        {
            if (!tools.Contains(tool))
                tools.Add(tool);
        }

        private void CloseCore(IWorkspaceItem workspace)
        {
            if (workspace is IWorkspaceDocument document)
                documents.Remove(document);
            workspace.Close();
        }
    }
}
