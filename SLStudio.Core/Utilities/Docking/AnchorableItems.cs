using DevExpress.Mvvm;
using System;
using System.Windows.Input;

namespace SLStudio.Core.Docking
{
    public interface IDocument : IAnchorableItem
    {
    }

    public abstract class DocumentBase : BindableBase, IDocument
    {
        public virtual string DisplayName
        {
            get => GetProperty(() => DisplayName);
            set => SetProperty(() => DisplayName, value);
        }

        public virtual string ContentId => string.Empty;

        public virtual bool IsSelected
        {
            get => GetProperty(() => IsSelected);
            set => SetProperty(() => IsSelected, value);
        }

        public virtual bool ShouldReopenOnStart => false;

        public virtual Uri IconSource => null;

        public virtual Guid Id => Guid.NewGuid();

        public virtual bool CanClose => true;

        public ICommand CloseCommand => new CommandHandler(Close, () => CanClose);

        public virtual void Close()
        {
        }
    }

    public interface ITool : IAnchorableItem
    {
        PaneLocation PreferredLocation { get; }
        double PreferredWidth { get; }
        double PreferredHeight { get; }

        bool IsVisible { get; set; }
    }

    public abstract class ToolBase : BindableBase, ITool
    {
        public virtual PaneLocation PreferredLocation => PaneLocation.Left;

        public virtual double PreferredWidth => 300.0;

        public virtual double PreferredHeight => 300.0;

        public virtual bool IsVisible
        {
            get => GetProperty(() => IsVisible);
            set => SetProperty(() => IsVisible, value);
        }

        public virtual string DisplayName
        {
            get => GetProperty(() => DisplayName);
            set => SetProperty(() => DisplayName, value);
        }

        public virtual string ContentId => string.Empty;

        public virtual bool IsSelected
        {
            get => GetProperty(() => IsSelected);
            set => SetProperty(() => IsSelected, value);
        }

        public virtual bool ShouldReopenOnStart => false;

        public virtual Uri IconSource => null;

        public virtual Guid Id => Guid.NewGuid();

        public virtual bool CanClose => true;

        public ICommand CloseCommand => new CommandHandler(Close, () => CanClose);

        public virtual void Close()
        {
        }
    }

    public interface IAnchorableItem
    {
        string DisplayName { get; set; }
        string ContentId { get; }
        bool IsSelected { get; set; }
        bool ShouldReopenOnStart { get; }
        Uri IconSource { get; }
        Guid Id { get; }
        ICommand CloseCommand { get; }
    }
}