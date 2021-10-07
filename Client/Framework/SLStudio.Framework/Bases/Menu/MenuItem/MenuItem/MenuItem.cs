using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio
{
    internal class MenuItem : BindableBase, IMenuItem
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
            if (Path.Count(c => c == '|') == 1)
                return null;
            var splitted = Path.Split('|', options: StringSplitOptions.RemoveEmptyEntries).SkipLast(1);
            return $"{string.Join('|', splitted)}|";
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

            var parentPath = GetParentPath();
            if (parentPath is null)
                throw new InvalidOperationException("Cannot set a parent in a root item");
            if (!parent.Path.Equals(parentPath))
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