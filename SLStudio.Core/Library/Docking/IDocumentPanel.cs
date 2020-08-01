using AvalonDock;

namespace SLStudio.Core
{
    public class DocumentPanelBase : PanelBase, IDocumentPanel
    {
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

    public interface IDocumentPanel : IPanelItem
    {
        string ToolTip { get; }

        void OnClosing(DocumentClosingEventArgs e);

        void OnClosed(DocumentClosedEventArgs e);
    }
}