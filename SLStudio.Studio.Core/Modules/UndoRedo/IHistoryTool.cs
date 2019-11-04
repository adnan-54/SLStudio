using SLStudio.Studio.Core.Framework;

namespace SLStudio.Studio.Core.Modules.UndoRedo
{
    public interface IHistoryTool : ITool
    {
        IUndoRedoManager UndoRedoManager { get; set; }
    }
}