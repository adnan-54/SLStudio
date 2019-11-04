using Caliburn.Micro;

namespace SLStudio.Studio.Core.Modules.ToolBars
{
    public interface IToolBars
    {
        IObservableCollection<IToolBar> Items { get; }
        bool Visible { get; set; }
    }
}