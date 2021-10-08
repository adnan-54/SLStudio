using System.Collections.Generic;

namespace SLStudio
{
    public interface IMenuItem
    {
        IMenuItem Parent { get; }

        IEnumerable<IMenuItem> Children { get; }

        int? Index { get; }

        string Path { get; }

        string ParentPath { get; }

        bool IsRootItem { get; }

        int HierarchicalLevel { get; }

        string Title { get; }

        string ToolTip { get; }

        object Icon { get; }

        bool IsVisible { get; }

        bool IsEnabled { get; }

        int GetIndex();

        void Show();

        void Hide();

        void Enable();

        void Disable();

        void SetParent(IMenuItem parent);

        void AddChild(IMenuItem child);
    }
}