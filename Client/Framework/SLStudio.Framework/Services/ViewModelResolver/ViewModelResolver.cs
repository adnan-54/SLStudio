using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio
{
    internal class ViewModelResolver : IViewModelResolver
    {
        private record ViewModelDescriptor(Type Service, Type Implementation, Type Concrete);
        private readonly List<ViewModelDescriptor> viewModelDescriptors;

        public ViewModelResolver(IMessenger messenger)
        {
            viewModelDescriptors = new();
            messenger.Register<ViewModelRegisteredMessage>(this, OnViewModelRegistered);
        }

        public Type Resolve(Type viewModelType)
        {
            var targetType = viewModelDescriptors.FirstOrDefault(t => t.Concrete == viewModelType);
            if (targetType is not null)
                return targetType.Concrete;

            targetType = viewModelDescriptors.FirstOrDefault(t => t.Implementation == viewModelType);
            if (targetType is not null)
                return targetType.Service;

            targetType = viewModelDescriptors.FirstOrDefault(t => t.Service == viewModelType);
            if (targetType is not null)
                return targetType.Service;

            throw new InvalidOperationException($"No registration found for type '{viewModelType.Name}'");
        }

        Type IViewModelResolver.Resolve<TViewModel>()
        {
            return Resolve(typeof(TViewModel));
        }

        private void OnViewModelRegistered(ViewModelRegisteredMessage message)
        {
            viewModelDescriptors.Add(new(message.ServiceType, message.ImplementationType, message.ConcreteType));
        }
    }
}
