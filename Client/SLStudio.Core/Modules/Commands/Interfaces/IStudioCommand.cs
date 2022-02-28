using System.Windows.Input;

namespace SLStudio;

public interface IStudioCommand : ICommand
{
    string Key { get; }

    bool IsExecuting { get; }
}

public interface IStudioToggleCommand : IStudioCommand
{

}