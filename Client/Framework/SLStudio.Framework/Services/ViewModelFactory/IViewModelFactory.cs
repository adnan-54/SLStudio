using System;

namespace SLStudio
{
    internal interface IViewModelFactory
    {
        IViewModel Create(Type viewModelType);

        TViewModel Create<TViewModel>()
            where TViewModel : class, IViewModel;
    }
}
