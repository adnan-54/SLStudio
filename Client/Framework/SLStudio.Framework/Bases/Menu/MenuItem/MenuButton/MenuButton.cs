using System.Windows.Input;

namespace SLStudio
{
    internal class MenuButton : MenuItem, IMenuButton
    {
        private readonly IMenuButtonHandler handler;

        public MenuButton(IMenuButtonHandler handler)
        {
            this.handler = handler;

            handler.IsExecutingChanged += (_, _) => RaisePropertyChanged(nameof(IsExecuting));
            handler.CanExecuteChanged += (_, _) => RaisePropertyChanged(nameof(CanExecute));
        }

        public KeyGesture Shortcut { get; init; }

        public ICommand Command => handler;

        public bool IsExecuting => handler.IsExecuting;

        public bool CanExecute => IsEnabled && handler.CanExecute(null);

        public void Execute()
        {
            if (CanExecute)
                handler.Execute(null);
        }
    }
}