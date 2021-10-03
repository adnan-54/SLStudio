using DevExpress.Mvvm;
using System;
using System.Windows.Input;

namespace SLStudio.Core.Menus
{
    internal class MenuItem : BindableBase, IMenuItem
    {
        public MenuItem()
        {
            Children = new BindableCollection<IMenuItem>();
        }

        public MenuItem(string path, int index, string displayName, string toolTip, object iconSource, KeyGesture shortcut, bool isVisible, bool isEnabled) : this()
        {
            Path = path;
            Index = index;
            DisplayName = displayName;
            ToolTip = toolTip;
            IconSource = iconSource;
            Shortcut = shortcut;
            IsVisible = isVisible;
            IsEnabled = isEnabled;
        }

        public MenuItem(string path, int index) : this()
        {
            Path = path;
            Index = index;
        }

        public int Index { get; }

        public string Path { get; }

        public string DisplayName
        {
            get => GetProperty(() => DisplayName);
            set => SetProperty(() => DisplayName, value);
        }

        public string ToolTip
        {
            get => GetProperty(() => ToolTip);
            set => SetProperty(() => ToolTip, value);
        }

        public bool IsEnabled
        {
            get => GetProperty(() => IsEnabled);
            set => SetProperty(() => IsEnabled, value);
        }

        public bool IsVisible
        {
            get => GetProperty(() => IsVisible);
            set => SetProperty(() => IsVisible, value);
        }

        public object IconSource
        {
            get => GetProperty(() => IconSource);
            set => SetProperty(() => IconSource, value);
        }

        public KeyGesture Shortcut { get; }

        public ICommand Command { get; internal set; }

        public object Parameter { get; set; }

        public IMenuItem Parent { get; set; }

        public IObservableCollection<IMenuItem> Children { get; }

        public void Disable()
        {
            IsEnabled = false;
        }

        public void Enable()
        {
            IsEnabled = true;
        }

        public void Hide()
        {
            IsVisible = false;
        }

        public void Show()
        {
            IsVisible = true;
        }
    }

    public interface IMenuItem : IHaveDisplayName
    {
        int Index { get; }
        string Path { get; }
        string ToolTip { get; set; }
        bool IsEnabled { get; }
        bool IsVisible { get; }
        object IconSource { get; set; }
        KeyGesture Shortcut { get; }
        ICommand Command { get; }
        object Parameter { get; set; }

        IMenuItem Parent { get; set; }
        IObservableCollection<IMenuItem> Children { get; }

        void Enable();

        void Disable();

        void Show();

        void Hide();
    }
}