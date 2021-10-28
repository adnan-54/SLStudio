using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio
{
    internal class ViewLocator : IViewLocator
    {
        private record ViewDescriptor(Type ViewType, Type ViewModelType);
        private readonly IViewModelResolver viewModelResolver;
        private readonly List<ViewDescriptor> descriptions;

        public ViewLocator(IViewModelResolver viewModelResolver, IMessenger messenger)
        {
            this.viewModelResolver = viewModelResolver;
            descriptions = new();
            messenger.Register<ViewRegisteredMessage>(this, OnViewRegistered);
        }

        public Type Locate(object viewModel)
        {
            var viewModelType = viewModelResolver.Resolve(viewModel.GetType());
            return Locate(viewModelType);
        }

        Type IViewLocator.Locate<TViewModel>()
        {
            return Locate(typeof(TViewModel));
        }

        public Type Locate(Type viewModelType)
        {
            var descriptor = descriptions.FirstOrDefault(d => d.ViewModelType == viewModelType);
            if (descriptor is null)
                throw new InvalidOperationException($"Could not find a view for '{viewModelType.Name}'");
            return descriptor.ViewType;
        }

        private void OnViewRegistered(ViewRegisteredMessage message)
        {
            var viewType = message.ViewType;
            var viewModelType = message.ViewModelType;
            descriptions.Add(new(viewType, viewModelType));
        }
    }
}
