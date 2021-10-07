using System.Windows.Input;

namespace SLStudio
{
    internal class MenuToggle : MenuItem, IMenuToggle
    {
        private readonly IMenuToggleHandler handler;

        public MenuToggle(IMenuToggleHandler handler)
        {
            this.handler = handler;

            handler.IsTogglingChanged += (_, _) => RaisePropertyChanged(nameof(IsToggling));
            handler.CanToggleChanged += (_, _) => RaisePropertyChanged(nameof(CanToggle));
        }

        public KeyGesture Shortcut { get; init; }

        public bool IsChecked
        {
            get => GetValue<bool>();
            set => SetValue(value, OnToggled);
        }

        public bool IsToggling => handler.IsToggling;

        public bool CanToggle => handler.CanToggle();

        public void Check()
        {
            IsChecked = true;
        }

        public void Uncheck()
        {
            IsChecked = false;
        }

        public void Toggle()
        {
            IsChecked = !IsChecked;
        }

        private void OnToggled()
        {
            handler.Toggle();
        }
    }
}