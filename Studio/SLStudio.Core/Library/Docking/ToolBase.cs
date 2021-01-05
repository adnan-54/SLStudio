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

            Width = 300.0d;
            Height = 300.0d;
        }

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

        public virtual double Width
        {
            get => GetProperty(() => Width);
            set => SetProperty(() => Width, value);
        }

        public virtual double Height
        {
            get => GetProperty(() => Height);
            set => SetProperty(() => Height, value);
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

        public double Width { get; set; }

        public double Height { get; set; }

        void SetContent(IToolContent content);
    }
}