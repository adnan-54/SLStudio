﻿using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SLStudio
{
    internal class ViewModelLocator : Service, IViewModelLocator
    {
        private readonly Dictionary<Type, Type> fromViewModels;
        private readonly Dictionary<Type, Type> fromViews;

        public ViewModelLocator(IModuleRegister moduleRegister)
        {
            fromViewModels = new Dictionary<Type, Type>();
            fromViews = new Dictionary<Type, Type>();

            moduleRegister.ViewRegistered += OnViewRegistered;
            moduleRegister.ViewModelRegistered += OnViewModelRegistered;
        }

        public Type LocateFromView(Type viewType)
        {
            if (viewType.IsAssignableTo(typeof(UserControl)) && fromViews.TryGetValue(viewType, out var viewModel))
                return viewModel;

            throw new ArgumentException($"Could not find a view model for '{viewType.Name}'");
        }

        public Type LocateFromViewModel(Type viewModelType)
        {
            if (viewModelType.IsAssignableTo(typeof(IViewModel)) && fromViewModels.TryGetValue(viewModelType, out var viewModel))
                return viewModel;

            throw new ArgumentException($"Could not find a view model for '{viewModelType.Name}'");
        }

        private void OnViewRegistered(object sender, ViewRegisteredEventArgs e)
        {
            var viewType = e.ViewType;
            var viewModelType = e.ViewModelType;

            fromViews.TryAdd(viewType, viewModelType);
        }

        private void OnViewModelRegistered(object sender, ViewModelRegisteredEventArgs e)
        {
            var serviceType = e.ServiceType;
            var implementationType = e.ImplementationType;

            fromViewModels.TryAdd(serviceType, serviceType);
            fromViewModels.TryAdd(implementationType, serviceType);
        }
    }
}