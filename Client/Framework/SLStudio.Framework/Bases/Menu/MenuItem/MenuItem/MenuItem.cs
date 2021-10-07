using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio
{
    public class MenuItem : BindableBase, IMenuItem
    {
        private readonly BindableCollection<IMenuItem> children;

        public MenuItem()
        {
            children = new BindableCollection<IMenuItem>();
        }

        public IMenuItem Parent
        {
            get => GetValue<IMenuItem>();
            private set => SetValue(value);
        }

        public IEnumerable<IMenuItem> Children => children;

        public int? Index { get; init; }

        public string Path { get; init; }

        public string Title
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        public string ToolTip
        {
            get => GetValue<string>();
            set
            {
                if (SetValue(value))
                    RaisePropertyChanged(nameof(HasToolTip));
            }
        }

        public bool HasToolTip => !string.IsNullOrEmpty(ToolTip);

        public object Icon
        {
            get => GetValue<object>();
            set => SetValue(value);
        }

        public bool IsVisible
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public bool IsEnabled
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public int GetIndex()
        {
            if (Index.HasValue)
                return Index.Value;
            return Parent.Children.Count();
        }

        public string GetParentPath()
        {
            if (Parent != null)
                return Parent.Path;
            if (string.IsNullOrEmpty(Path))
                return string.Empty;
            return Path.Substring(0, Path.LastIndexOf('|') + 1);
        }

        public void Show()
        {
            IsVisible = true;
        }

        public void Hide()
        {
            IsVisible = false;
        }

        public void Enable()
        {
            IsEnabled = true;
        }

        public void Disable()
        {
            IsEnabled = false;
        }

        public void SetParent(IMenuItem parent)
        {
            if (Parent is not null)
                return;

            if (parent is null)
                throw new ArgumentNullException(nameof(parent));
            if (!Path.StartsWith(parent.Path))
                throw new InvalidOperationException("Parent does not match the child path");

            Parent = parent;
        }

        public virtual void AddChild(IMenuItem child)
        {
            if (child is null)
                throw new ArgumentNullException(nameof(child));
            if (!child.Path.StartsWith(Path))
                throw new InvalidOperationException("Child does not match the parent path");

            child.SetParent(this);
            children.Add(child);

            var ordered = children.OrderBy(c => c.GetIndex());
            children.Clear();
            children.AddRange(ordered);
        }

        public override string ToString()
        {
            return Title;
        }
    }
}