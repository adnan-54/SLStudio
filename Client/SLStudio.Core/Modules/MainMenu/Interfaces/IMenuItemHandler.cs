using System.Windows.Input;

namespace SLStudio;

public interface IMenuItemHandler<TMenuItem> : ICommand
    where TMenuItem : class, IMenuItem
{
    public IStudioCommand? Command { get; }

    public TMenuItem? Menu { get; }

    internal void AttachMenu(TMenuItem menu);

    internal void AttachCommand(IStudioCommand command);
}

