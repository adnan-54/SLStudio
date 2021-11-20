namespace SLStudio
{
    public interface IDocumentHeader : IWorkspaceHeader
    {
        object IconSource { get; }

        string ToolTip { get; }

        bool Pinned { get; }
    }
}
