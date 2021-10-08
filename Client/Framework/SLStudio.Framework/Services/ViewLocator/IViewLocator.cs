using System;

namespace SLStudio
{
    public interface IViewLocator
    {
        Type LocateView(Type viewModelType);

        Type LocateView<TViewModel>()
            where TViewModel : class, IViewModel;

        Type LocateWindow<TViewModel>()
            where TViewModel : class, IWindowViewModel;
    }
}