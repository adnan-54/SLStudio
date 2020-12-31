using AvalonDock;

namespace SLStudio.Core
{
    public interface IDocumentPanel : IPanelItem
    {
        IToolContentProvider ToolContentProvider { get; }

        string ToolTip { get; }

        void OnClosing(DocumentClosingEventArgs e);

        void OnClosed(DocumentClosedEventArgs e);
    }
}