using SLStudio.Studio.Core.Modules.UndoRedo;

namespace SLStudio.Studio.Core.Framework
{
    public interface IDocument : ILayoutItem
	{
        IUndoRedoManager UndoRedoManager { get; }
	}
}