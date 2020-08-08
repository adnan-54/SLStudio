using AvalonDock;

namespace SLStudio.Core
{
    public class DocumentPanelBase : PanelBase, IDocumentPanel
    {
        public IToolboxContent ToolboxContent
        {
            get => GetProperty(() => ToolboxContent);
            protected set => SetProperty(() => ToolboxContent, value);
        }

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

    public interface IDocumentPanel : IPanelItem, IHaveToolbox
    {
        string ToolTip { get; }

        void OnClosing(DocumentClosingEventArgs e);

        void OnClosed(DocumentClosedEventArgs e);
    }
}