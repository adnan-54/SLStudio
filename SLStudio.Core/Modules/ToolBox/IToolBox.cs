namespace SLStudio.Core
{
    public interface IToolbox : IToolPanel
    {
        IToolboxContent ToolboxContent { get; }

        void SetContent(IPanelItem host);
    }
}