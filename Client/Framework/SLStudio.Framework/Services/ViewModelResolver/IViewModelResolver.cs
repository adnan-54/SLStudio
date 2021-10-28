using System;

namespace SLStudio
{
    internal interface IViewModelResolver
    {
        Type Resolve(Type viewModelType);

        Type Resolve<TViewModel>()
            where TViewModel : class, IViewModel;
    }
}
