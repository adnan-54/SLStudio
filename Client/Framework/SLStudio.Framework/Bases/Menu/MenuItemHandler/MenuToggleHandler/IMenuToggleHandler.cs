using System;

namespace SLStudio
{
    public interface IMenuToggleHandler : IMenuItemHandler<IMenuToggle>
    {
        event EventHandler CanToggleChanged;

        event EventHandler IsTogglingChanged;

        bool IsToggling { get; }

        bool CanToggle();

        void Toggle();
    }
}