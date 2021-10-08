﻿using DevExpress.Mvvm;
using System;

namespace SLStudio
{
    internal class ModuleRegister : Service, IModuleRegister
    {
        private readonly IContainer container;
        private readonly IApplicationInfo applicationInfo;
        private readonly IMessenger messenger;

        public ModuleRegister(IContainer container, IApplicationInfo applicationInfo, IMessenger messenger)
        {
            this.container = container;
            this.applicationInfo = applicationInfo;
            this.messenger = messenger;
        }

        void IModuleRegister.RegisterResource(Uri uri)
        {
            applicationInfo.MergeResource(uri);
        }

        void IModuleRegister.MenuConfiguration<TConfiguratiton>()
        {
            container.RegisterSingleton<TConfiguratiton>();
            messenger.Send(new MenuConfigurationRegisteredMessage(typeof(TConfiguratiton)));
        }

        void IModuleRegister.ViewModel<TConcrete>(LifeStyle lifeStyle)
        {
            container.Register<TConcrete>(lifeStyle.ToLifestyle());

            messenger.Send(new ViewModelRegisteredMessage(typeof(TConcrete), typeof(TConcrete)));
        }

        void IModuleRegister.ViewModel<TService, TImplementation>(LifeStyle lifeStyle)
        {
            container.Register<TService, TImplementation>(lifeStyle.ToLifestyle());

            messenger.Send(new ViewModelRegisteredMessage(typeof(TService), typeof(TImplementation)));
        }

        void IModuleRegister.View<TView, TViewModel>()
        {
            var viewType = typeof(TView);
            var viewModelType = typeof(TViewModel);

            messenger.Send(new ViewRegisteredMessage(viewType, viewModelType));
        }

        void IModuleRegister.Window<TWindow, TViewModel>()
        {
            var windowType = typeof(TWindow);
            var viewModelType = typeof(TViewModel);

            messenger.Send(new ViewRegisteredMessage(windowType, viewModelType));
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