using System;
using System.Windows.Input;

namespace SLStudio
{
    public interface IMenuButtonHandler : IMenuItemHandler<IMenuButton>, ICommand
    {
        event EventHandler IsExecutingChanged;

        bool IsExecuting { get; }
    }
}
