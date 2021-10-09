using System;
using System.Windows;
using System.Windows.Controls;

namespace SLStudio
{
    public interface IModuleRegister
    {
        void RegisterResource(Uri uri);

        void MenuConfiguration<TConfiguratiton>()
            where TConfiguratiton : class, IMenuConfiguration;

        void ViewModel<TConcrete>(LifeStyle lifeStyle)
            where TConcrete : class, IViewModel;

        void ViewModel<TService, TImplementation>(LifeStyle lifeStyle)
            where TService : class, IViewModel
            where TImplementation : class, TService;

        void View<TView, TViewModel>()
            where TView : UserControl
            where TViewModel : class, IViewModel;

        void Window<TWindow, TViewModel>()
            where TWindow : Window
            where TViewModel : class, IWindowViewModel;

        void Singleton<TConcrete>()
            where TConcrete : class;

        void Singleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        void Service<TConcrete>()
            where TConcrete : class;

        void Service<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        void ServiceContainer(IServiceContainer serviceContainer);

        void Custom(Action<IContainer> callback);
    }
}
