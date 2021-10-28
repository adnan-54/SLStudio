namespace SLStudio
{

    public interface IWorkspaceHeader
    {
        string Title { get; }

        string ToolTip { get; }

        object Icon { get; }
    }
}
