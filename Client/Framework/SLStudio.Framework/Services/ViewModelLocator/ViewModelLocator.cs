using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio
{
    internal class ViewModelLocator : IViewModelLocator
    {
        private record ViewModelDescriptor(Type ViewModelType, Type ViewType);
        private readonly List<ViewModelDescriptor> descriptions;

        public ViewModelLocator(IMessenger messenger)
        {
            descriptions = new();
            messenger.Register<ViewRegisteredMessage>(this, OnViewRegistered);
        }

        public Type Locate(object view)
        {
            return Locate(view.GetType());
        }

        Type IViewModelLocator.Locate<TView>()
        {
            return Locate(typeof(TView));
        }

        public Type Locate(Type viewType)
        {
            var descriptor = descriptions.FirstOrDefault(d => d.ViewType == viewType);
            if (descriptor is null)
                throw new InvalidOperationException($"Could not find a view model for '{viewType.Name}'");
            return descriptor.ViewModelType;
        }

        private void OnViewRegistered(ViewRegisteredMessage message)
        {
            var viewModelType = message.ViewModelType;
            var viewType = message.ViewType;
            descriptions.Add(new(viewModelType, viewType));
        }
    }
}
