namespace SLStudio.Core
{
    public interface IToolPanel : IPanelItem
    {
        ToolPlacement Placement { get; }

        IToolContent Content { get; }

        public double Width { get; }

        public double Height { get; }

        bool IsVisible { get; }

        void Hide();

        void SetContent(IToolContent content);
    }

    public enum ToolPlacement
    {
        Left,
        Right,
        Bottom
    }
}