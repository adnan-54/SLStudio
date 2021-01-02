using SLStudio.Core.Library.Docking.Tools.DefaultContent;

namespace SLStudio.Core
{
    public abstract class ToolPanelBase : PanelItemBase, IToolPanel
    {
        protected ToolPanelBase()
        {
            DefaultContent = new DefaultContentViewModel();
            CanSetContent = true;
            SetContent(DefaultContent);
        }

        public abstract ToolPlacement Placement { get; }

        protected virtual IToolContent DefaultContent { get; }

        public IToolContent Content
        {
            get => GetProperty(() => Content);
            private set => SetProperty(() => Content, value);
        }

        public virtual double Width => 300.0d;

        public virtual double Height => 300.0d;

        public bool IsVisible
        {
            get => GetProperty(() => IsVisible);
            set => SetProperty(() => IsVisible, value);
        }

        protected bool CanSetContent { get; set; }

        public override void Activate()
        {
            IsVisible = true;
            base.Activate();
        }

        public void Hide()
        {
            IsActive = false;
            IsSelected = false;
            IsVisible = false;
        }

        public virtual void SetContent(IToolContent content)
        {
            content ??= DefaultContent;

            if (!CanSetContent || Content == content)
                return;

            Content = content;
        }
    }
}