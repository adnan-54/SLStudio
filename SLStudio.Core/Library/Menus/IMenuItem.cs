using DevExpress.Mvvm;
using System;
using System.Windows.Input;

namespace SLStudio.Core.Menus
{
    internal class MenuItem : BindableBase, IMenuItem
    {
        public MenuItem()
        {
            IsVisible = true;
            IsEnabled = true;
            Children = new BindableCollection<IMenuItem>();
        }

        public int Index
        {
            get => GetProperty(() => Index);
            set => SetProperty(() => Index, value);
        }

        public string Path
        {
            get => GetProperty(() => Path);
            set => SetProperty(() => Path, value);
        }

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

        public Uri IconSource
        {
            get => GetProperty(() => IconSource);
            set => SetProperty(() => IconSource, value);
        }

        public KeyGesture Shortcut { get; internal set; }

        public ICommand Command { get; internal set; }

        public object Parameter { get; internal set; }

        public IMenuItem Parent { get; internal set; }

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
        int Index { get; set; }
        string Path { get; }
        string ToolTip { get; }
        bool IsEnabled { get; }
        bool IsVisible { get; }
        Uri IconSource { get; }
        KeyGesture Shortcut { get; }
        ICommand Command { get; }
        object Parameter { get; }

        IMenuItem Parent { get; }
        IObservableCollection<IMenuItem> Children { get; }

        void Enable();

        void Disable();

        void Show();

        void Hide();
    }
}