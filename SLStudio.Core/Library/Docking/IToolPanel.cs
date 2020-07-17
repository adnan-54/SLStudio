using AvalonDock;

namespace SLStudio.Core
{
    public abstract class ToolPanelBase : WorkspacePanel, IToolPanel
    {
        public virtual double Width => 300.0;

        public virtual double Height => 300.0;

        public abstract ToolPlacement Placement { get; }

        public override void OnClosed(DocumentClosedEventArgs e)
        {
        }
    }

    public interface IToolPanel : IWorkspacePanel
    {
        public double Width { get; }
        public double Height { get; }
        public ToolPlacement Placement { get; }
    }

    public enum ToolPlacement
    {
        Left,
        Right,
        Bottom
    }
}