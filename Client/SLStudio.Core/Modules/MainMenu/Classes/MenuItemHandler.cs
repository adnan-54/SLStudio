using System.Windows.Input;

namespace SLStudio;

public abstract class MenuItemHandler<TMenuItem> : IMenuItemHandler<TMenuItem>
    where TMenuItem : class, IMenuItem
{
    public event EventHandler? CanExecuteChanged;

    public IStudioCommand? Command { get; private set; }

    public TMenuItem? Menu { get; private set; }

    public bool CanExecute(object? parameter)
    {
        EnsureValidState();
        OnUpdate().FireAndForget();
        return Command!.CanExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        EnsureValidState();
        Command!.Execute(parameter);
    }

    protected virtual Task OnUpdate()
    {
        return Task.CompletedTask;
    }

    private void EnsureValidState()
    {
        if (Command is null)
            throw new Exception($"{nameof(Command)} has not been set");
        if (Menu is null)
            throw new Exception($"{nameof(Menu)} has not been set");
    }

    private void RaiseCanExecutedChanged(object? sender, EventArgs e)
    {
        CanExecuteChanged?.Invoke(sender, e);
    }

    void IMenuItemHandler<TMenuItem>.AttachMenu(TMenuItem menu)
    {
        if (Menu is not null)
            throw new Exception($"{nameof(Menu)} has been already attached");
        Menu = menu;
        CommandManager.RequerySuggested += (sender, e) => RaiseCanExecutedChanged(sender, e);
    }

    void IMenuItemHandler<TMenuItem>.AttachCommand(IStudioCommand command)
    {
        if (command is not null)
            throw new Exception($"{nameof(command)} has been already attached");
        Command = command;
        Command!.CanExecuteChanged += (sender, e) => RaiseCanExecutedChanged(sender, e);
    }
}
