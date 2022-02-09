namespace SLStudio;

internal interface IViewFactory
{
    IView CreateFromViewModel<TViewModel>()
        where TViewModel : IViewModel;

    IView CreateFromViewModel(Type viewModelType);

    internal void Register<TView, TViewModel>()
        where TView : class, IView
        where TViewModel : class, IViewModel;
}
