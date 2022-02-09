namespace SLStudio;

internal interface IViewModelFactory
{
    TViewModel Create<TViewModel>()
        where TViewModel : class, IViewModel;

    internal Type GetMappedType(Type viewModelType);

    internal void Register<TViewModel>()
        where TViewModel : class, IViewModel;

    internal void Register<TService, TViewModel>()
        where TService : class, IViewModel
        where TViewModel : class, TService;
}