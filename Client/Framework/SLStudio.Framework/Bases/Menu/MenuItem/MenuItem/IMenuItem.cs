﻿using System.Collections.Generic;

namespace SLStudio
{
    public interface IMenuItem
    {
        IMenuItem Parent { get; }

        IEnumerable<IMenuItem> Children { get; }

        int? Index { get; }

        string Path { get; }

        string Title { get; }

        string ToolTip { get; }

        bool HasToolTip { get; }

        object Icon { get; }

        bool IsVisible { get; }

        bool IsEnabled { get; }

        int GetIndex();

        string GetParentPath();

        void Show();

        void Hide();

        void Enable();

        void Disable();

        void SetParent(IMenuItem parent);

        void AddChild(IMenuItem child);
    }
}