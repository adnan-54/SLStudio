namespace SLStudio.Studio.Core.Framework
{
    public interface IDocument : ILayoutItem
	{
        IUndoRedoManager UndoRedoManager { get; }
	}
}