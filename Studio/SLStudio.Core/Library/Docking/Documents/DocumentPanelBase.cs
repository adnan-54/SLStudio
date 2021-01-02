using AvalonDock;

namespace SLStudio.Core
{
    public class DocumentPanelBase : PanelItemBase, IDocumentPanel
    {
        public IToolContentProvider ToolContentProvider { get; protected set; }

        public string ToolTip
        {
            get => GetProperty(() => ToolTip);
            set => SetProperty(() => ToolTip, value);
        }

        public virtual void OnClosing(DocumentClosingEventArgs e)
        {
        }

        public virtual void OnClosed(DocumentClosedEventArgs e)
        {
        }
    }
}