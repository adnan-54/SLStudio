using System;

namespace SLStudio
{
    public interface IViewLocator
    {
        Type Locate(object viewModel);

        Type Locate(Type viewModelType);

        Type Locate<TViewModel>()
            where TViewModel : class, IViewModel;
    }
}