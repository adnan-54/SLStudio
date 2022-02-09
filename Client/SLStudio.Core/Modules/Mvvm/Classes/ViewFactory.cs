namespace SLStudio;

internal class ViewFactory : IViewFactory
{
    private readonly IObjectFactory objectFactory;
    private readonly IViewModelFactory viewModelFactory;
    private readonly Dictionary<Type, Type> views;

    public ViewFactory(IObjectFactory objectFactory, IViewModelFactory viewModelFactory)
    {
        this.objectFactory = objectFactory;
        this.viewModelFactory = viewModelFactory;
        views = new();
    }

    IView IViewFactory.CreateFromViewModel<TViewModel>()
    {
        var viewModelType = typeof(TViewModel);
        return CreateFromViewModel(viewModelType);
    }

    public IView CreateFromViewModel(Type viewModelType)
    {
        viewModelType = viewModelFactory.GetMappedType(viewModelType);
        if (views.TryGetValue(viewModelType, out var viewType))
            return (IView)objectFactory.GetObject(viewType);
        throw new InvalidOperationException($"Could not find any view for view model '{viewModelType}'");
    }

    void IViewFactory.Register<TView, TViewModel>()
    {
        var viewModelType = typeof(TViewModel);
        if (views.ContainsKey(viewModelType))
            throw new InvalidOperationException($"A view model of type '{viewModelType}' has been already registered");
        views.Add(viewModelType, typeof(TView));
    }
}
