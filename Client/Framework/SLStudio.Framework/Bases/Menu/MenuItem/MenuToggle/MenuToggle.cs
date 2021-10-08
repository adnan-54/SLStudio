﻿using System.Windows.Input;

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

        public bool CanToggle => IsEnabled && handler.CanToggle();

        public void Check()
        {
            if (CanToggle)
                IsChecked = true;
        }

        public void Uncheck()
        {
            if (CanToggle)
                IsChecked = false;
        }

        public void Toggle()
        {
            if (CanToggle)
                IsChecked = !IsChecked;
        }

        private void OnToggled()
        {
            if (CanToggle)
                handler.Toggle();
        }
    }
}