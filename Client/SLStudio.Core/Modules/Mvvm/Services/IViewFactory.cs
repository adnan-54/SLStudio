namespace SLStudio;

internal interface IViewFactory
{
    IView CreateFromViewModel<TViewModel>()
        where TViewModel : IViewModel;

    internal void Register<TView, TViewModel>()
        where TView : class, IView
        where TViewModel : class, IViewModel;
}
