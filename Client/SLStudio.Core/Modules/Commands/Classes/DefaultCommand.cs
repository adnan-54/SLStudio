namespace SLStudio.Commands;

internal sealed class DefaultCommand : IStudioCommand
{
    internal static readonly IStudioCommand Instance = new DefaultCommand();

    private DefaultCommand()
    {
        Key = "Command.Null";
    }

    public event EventHandler? CanExecuteChanged;

    public string Key { get; }

    public bool IsExecuting { get; }

    public bool CanExecute(object? parameter)
    {
        return false;
    }

    public void Execute(object? parameter)
    {

    }
}
