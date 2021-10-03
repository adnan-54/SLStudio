using System;

namespace SLStudio
{
    internal class ModuleRegister : Service, IModuleRegister
    {
        private event EventHandler<ViewModelRegisteredEventArgs> ViewModelRegistered;

        private event EventHandler<ViewRegisteredEventArgs> ViewRegistered;

        private readonly IContainer container;

        public ModuleRegister(IContainer container)
        {
            this.container = container;
        }

        event EventHandler<ViewModelRegisteredEventArgs> IModuleRegister.ViewModelRegistered
        {
            add => ViewModelRegistered += value;
            remove => ViewModelRegistered -= value;
        }

        event EventHandler<ViewRegisteredEventArgs> IModuleRegister.ViewRegistered
        {
            add => ViewRegistered += value;
            remove => ViewRegistered -= value;
        }

        void IModuleRegister.ViewModel<TConcrete>(LifeStyle lifeStyle)
        {
            container.Register<TConcrete>(lifeStyle.ToLifestyle());

            ViewModelRegistered?.Invoke(this, new ViewModelRegisteredEventArgs(typeof(TConcrete), typeof(TConcrete)));
        }

        void IModuleRegister.ViewModel<TService, TImplementation>(LifeStyle lifeStyle)
        {
            container.Register<TService, TImplementation>(lifeStyle.ToLifestyle());

            ViewModelRegistered?.Invoke(this, new ViewModelRegisteredEventArgs(typeof(TService), typeof(TImplementation)));
        }

        void IModuleRegister.View<TView, TViewModel>()
        {
            var viewType = typeof(TView);
            var viewModelType = typeof(TViewModel);

            ViewRegistered?.Invoke(this, new ViewRegisteredEventArgs(viewType, viewModelType));
        }

        void IModuleRegister.Window<TWindow, TViewModel>()
        {
            var windowType = typeof(TWindow);
            var viewModelType = typeof(TViewModel);

            ViewRegistered?.Invoke(this, new ViewRegisteredEventArgs(windowType, viewModelType));
        }

        void IModuleRegister.Singleton<TConcrete>()
        {
            container.RegisterSingleton<TConcrete>();
        }

        void IModuleRegister.Singleton<TService, TImplementation>()
        {
            container.RegisterSingleton<TService, TImplementation>();
        }

        void IModuleRegister.Service<TConcrete>()
        {
            container.RegisterSingleton<TConcrete>();
        }

        void IModuleRegister.Service<TService, TImplementation>()
        {
            container.RegisterSingleton<TService, TImplementation>();
        }

        public void ServiceCollection(IServiceCollection serviceContainer)
        {
            var servicesDictionary = serviceContainer.GetAll();
            serviceContainer.Lock();

            foreach (var kvp in servicesDictionary)
            {
                var type = kvp.Key;
                var service = kvp.Value;
                container.RegisterSingleton(type, service);
            }
        }

        public void Custom(Action<IContainer> callback)
        {
            callback?.Invoke(container);
        }
    }
}