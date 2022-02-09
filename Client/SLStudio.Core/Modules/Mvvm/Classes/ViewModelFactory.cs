namespace SLStudio;

internal class ViewModelFactory : IViewModelFactory
{
    private readonly IObjectFactory objectFactory;
    private readonly Dictionary<Type, Type> viewModels;

    public ViewModelFactory(IObjectFactory objectFactory)
    {
        this.objectFactory = objectFactory;
        viewModels = new();
    }

    TViewModel IViewModelFactory.Create<TViewModel>()
    {
        var viewModelType = typeof(TViewModel);
        if (!viewModels.TryGetValue(viewModelType, out viewModelType))
            throw new InvalidOperationException($"Could not find any view model for the type '{viewModelType}'");

        return (TViewModel)objectFactory.GetObject(viewModelType);
    }

    Type IViewModelFactory.GetMappedType(Type viewModelType)
    {
        if (viewModels.TryGetValue(viewModelType, out var mappedType))
            return mappedType;
        throw new InvalidOperationException($"Could not find any view model with type '{viewModelType}'");
    }

    void IViewModelFactory.Register<TViewModel>()
    {
        var viewModelType = typeof(TViewModel);
        if (viewModels.ContainsKey(viewModelType))
            throw new InvalidOperationException($"A view model of type '{viewModelType}' has been already registered");

        viewModels.Add(viewModelType, viewModelType);
    }

    void IViewModelFactory.Register<TService, TViewModel>()
    {
        var serviceType = typeof(TService);
        var viewModelType = typeof(TViewModel);

        if (viewModels.ContainsKey(serviceType))
            throw new InvalidOperationException($"A view model of type '{serviceType}' has been already registered");
        if (viewModels.ContainsKey(viewModelType))
            throw new InvalidOperationException($"A view model of type '{viewModelType}' has been already registered");

        viewModels.Add(serviceType, serviceType);
        viewModels.Add(viewModelType, serviceType);
    }
}