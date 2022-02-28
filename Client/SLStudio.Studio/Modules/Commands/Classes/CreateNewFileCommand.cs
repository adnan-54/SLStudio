namespace SLStudio.Studio.Commands;

internal class CreateNewFileCommand : IStudioCommand
{
    public event EventHandler? CanExecuteChanged;

    public string Key => "File.New";

    public bool IsExecuting { get; }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        MessageBox.Show("Fodase?");
    }
}