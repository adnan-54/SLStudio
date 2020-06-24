namespace SLStudio.Core
{
    public interface IShell : IWindow
    {
        BindableCollection<IDocumentPanel> Documents { get; }

        BindableCollection<IToolPanel> Tools { get; }

        IWorkspacePanel SelectedItem { get; }

        void OpenPanel(IWorkspacePanel item);

        void ClosePanel(IWorkspacePanel item);
    }
}