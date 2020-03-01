using Caliburn.Micro;

namespace SLStudio.Core
{
    public interface IViewModel : IScreen, IViewAware, IChild, INotifyPropertyChangedEx
    {
    }
}