using SLStudio.Logging;
using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SLStudio
{
    public abstract class MenuToggleHandler : MenuItemHandler<IMenuToggle>, IMenuToggleHandler
    {
        private static readonly ILogger logger = LogManager.GetLogger<MenuToggleHandler>();

        private event EventHandler IsTogglingChanged;

        event EventHandler IMenuToggleHandler.CanToggleChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        event EventHandler IMenuToggleHandler.IsTogglingChanged
        {
            add => IsTogglingChanged += value;
            remove => IsTogglingChanged -= value;
        }

        public bool IsToggling
        {
            get => GetValue<bool>();
            private set => SetValue(value, RaiseIsTogglingChanged);
        }

        bool IMenuToggleHandler.CanToggle()
        {
            return !IsToggling && CanToggle();
        }

        async void IMenuToggleHandler.Toggle()
        {
            if (!CanToggle() || IsToggling)
                return;

            try
            {
                IsToggling = true;
                await Toggle();
            }
            catch (Exception ex)
            {
                logger.Warn(ex);
            }
            finally
            {
                IsToggling = false;
            }
        }

        protected virtual bool CanToggle()
        {
            return true;
        }

        protected abstract Task Toggle();

        bool IMenuToggleHandler.IsToggling { get; }

        private void RaiseIsTogglingChanged()
        {
            IsTogglingChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
