namespace SLStudio;

internal class ViewFactory : IViewFactory
{
    private readonly IObjectFactory objectFactory;
    private readonly Dictionary<Type, Type> views;

    public ViewFactory(IObjectFactory objectFactory)
    {
        this.objectFactory = objectFactory;
        views = new();
    }

    IView IViewFactory.CreateFromViewModel<TViewModel>()
    {
        var viewModelType = typeof(TViewModel);

        if (!views.TryGetValue(viewModelType, out var viewType))
            throw new InvalidOperationException($"Could not find any view for view model '{viewModelType}'");
        return (IView)objectFactory.GetObject(viewType);

    }

    void IViewFactory.Register<TView, TViewModel>()
    {
        var viewModelType = typeof(TViewModel);

        if (views.ContainsKey(viewModelType))
            throw new InvalidOperationException($"A view model of type '{viewModelType}' has been already registered");

        views.Add(viewModelType, typeof(TView));
    }
}
