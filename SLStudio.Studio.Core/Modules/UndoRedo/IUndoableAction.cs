namespace SLStudio.Studio.Core.Modules.UndoRedo
{
    public interface IUndoableAction
    {
        string Name { get; }

        void Execute();
        void Undo();
    }
}