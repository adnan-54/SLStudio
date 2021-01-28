using SLStudio.Core.Library.Docking.Tools.DefaultContent;

namespace SLStudio.Core
{
    public abstract class ToolBase : WorkspaceItem, IToolItem
    {
        protected ToolBase()
        {
            DefaultContent = new DefaultContentViewModel();
            CanSetContent = true;
            SetContent(DefaultContent);

            PreferredWidth = 300.0d;
            PreferredHeight = 300.0d;
        }

        public virtual double PreferredWidth { get; }

        public virtual double PreferredHeight { get; }

        public bool IsVisible
        {
            get => GetProperty(() => IsVisible);
            set => SetProperty(() => IsVisible, value);
        }

        public IToolContent Content
        {
            get => GetProperty(() => Content);
            private set => SetProperty(() => Content, value);
        }

        public override bool IsClosed
        {
            get => base.IsClosed;
            set
            {
                base.IsClosed = value;
                IsVisible = !value;
            }
        }

        protected virtual bool CanSetContent { get; set; }

        protected virtual IToolContent DefaultContent { get; }

        public virtual void SetContent(IToolContent content)
        {
            content ??= DefaultContent;

            if (CanSetContent && Content != content)
            {
                Content = content;
                OnContentChanged();
            }
        }

        protected virtual void OnContentChanged()
        {
        }
    }

    public interface IToolItem : IWorkspaceItem
    {
        IToolContent Content { get; }

        public double PreferredWidth { get; }

        public double PreferredHeight { get; }

        void SetContent(IToolContent content);
    }
}