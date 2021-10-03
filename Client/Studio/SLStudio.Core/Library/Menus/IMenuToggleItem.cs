using System;
using System.Windows.Input;

namespace SLStudio.Core.Menus
{
    internal class MenuToggleItem : MenuItem, IMenuToggleItem
    {
        public MenuToggleItem(string path, int index, string displayName, string toolTip, object iconSource, KeyGesture shortcut, bool isVisible, bool isEnabled, bool isChecked)
            : base(path, index, displayName, toolTip, iconSource, shortcut, isVisible, isEnabled)
        {
            IsChecked = isChecked;
        }

        public bool IsChecked
        {
            get => GetProperty(() => IsChecked);
            set
            {
                if (IsChecked == value)
                    return;
                SetProperty(() => IsChecked, value);
            }
        }

        public bool Toggle()
        {
            IsChecked = !IsChecked;
            return IsChecked;
        }
    }

    public interface IMenuToggleItem : IMenuItem
    {
        bool IsChecked { get; set; }

        bool Toggle();
    }
}