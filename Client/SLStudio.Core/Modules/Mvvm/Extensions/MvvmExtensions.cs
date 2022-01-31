using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace SLStudio;

public static class MvvmExtensions
{
    public static IConfigurationContext AddViewModel<TViewModel>(this IConfigurationContext context, Lifestyle lifestyle)
        where TViewModel : class, IViewModel
    {
        var viewModelFactory = context.GetService<IViewModelFactory>()!;
        viewModelFactory.Register<TViewModel>();

        if (lifestyle == Lifestyle.Singleton)
            context.AddSingleton<TViewModel>();
        else
            context.AddTransient<TViewModel>();

        return context;
    }

    public static IConfigurationContext AddViewModel<TService, TViewModel>(this IConfigurationContext context, Lifestyle lifestyle)
        where TService : class, IViewModel
        where TViewModel : class, TService
    {
        var viewModelFactory = context.GetService<IViewModelFactory>()!;
        viewModelFactory.Register<TService, TViewModel>();

        if (lifestyle == Lifestyle.Singleton)
            context.AddSingleton<TService, TViewModel>();
        else
            context.AddTransient<TService, TViewModel>();

        return context;
    }

    public static IConfigurationContext AddView<TView, TViewModel>(this IConfigurationContext context)
        where TView : ContentControl, IView
        where TViewModel : class, IViewModel
    {
        var viewFactory = context.GetService<IViewFactory>()!;
        viewFactory.Register<TView, TViewModel>();

        return context;
    }
}
