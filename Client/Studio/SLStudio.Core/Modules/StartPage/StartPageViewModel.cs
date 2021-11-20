namespace SLStudio.Core
{
    internal class StartPageViewModel : WorkspaceDocument, IStartPage
    {
        public StartPageViewModel()
        {
            Header = new DocumentHeader
            {
                Title = "Start Page"
            };
        }

        public override IWorkspaceHeader Header { get; }
    }

    public interface IStartPage : IWorkspaceDocument
    {
    }
}
