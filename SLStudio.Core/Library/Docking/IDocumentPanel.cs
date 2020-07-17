using AvalonDock;

namespace SLStudio.Core
{
    public abstract class DocumentPanelBase : WorkspacePanel, IDocumentPanel
    {
        public string ToolTip
        {
            get => GetProperty(() => ToolTip);
            set => SetProperty(() => ToolTip, value);
        }

        public bool IsDirty
        {
            get => GetProperty(() => IsDirty);
            set => SetProperty(() => IsDirty, value);
        }

        public override void OnClosed(DocumentClosedEventArgs e)
        {
        }
    }

    public interface IDocumentPanel : IWorkspacePanel
    {
        string ToolTip { get; set; }
        bool IsDirty { get; set; }
    }
}