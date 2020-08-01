namespace SLStudio.Core
{
    public abstract class ToolPanelBase : PanelBase, IToolPanel
    {
        public abstract ToolPlacement Placement { get; }

        public virtual double Width => 300.0d;

        public virtual double Height => 300.0d;

        public bool IsVisible
        {
            get => GetProperty(() => IsVisible);
            set => SetProperty(() => IsVisible, value);
        }

        public override void Activate()
        {
            base.Activate();
            IsVisible = true;
        }

        public void Hide()
        {
            IsVisible = false;
            IsActive = false;
            IsSelected = false;
        }
    }

    public interface IToolPanel : IPanelItem
    {
        ToolPlacement Placement { get; }

        public double Width { get; }

        public double Height { get; }

        bool IsVisible { get; }

        void Hide();
    }

    public enum ToolPlacement
    {
        Left,
        Right,
        Bottom
    }
}