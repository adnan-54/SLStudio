using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SLStudio
{
    internal class ViewLocator : Service, IViewLocator
    {
        private readonly IViewModelLocator viewModelLocator;
        private readonly Dictionary<Type, Type> views;
        private readonly Dictionary<Type, Type> windows;

        public ViewLocator(IViewModelLocator viewModelLocator, IModuleRegister moduleRegister)
        {
            this.viewModelLocator = viewModelLocator;
            views = new Dictionary<Type, Type>();
            windows = new Dictionary<Type, Type>();

            moduleRegister.ViewRegistered += OnViewRegistered;
        }

        public Type LocateView(Type viewModelType)
        {
            viewModelType = viewModelLocator.LocateFromViewModel(viewModelType);

            if (views.TryGetValue(viewModelType, out var viewType))
                return viewType;
            throw new ArgumentException($"Could not find a view for '{viewModelType.Name}'");
        }

        Type IViewLocator.LocateView<TViewModel>()
        {
            var viewModelType = typeof(TViewModel);

            return LocateView(viewModelType);
        }

        Type IViewLocator.LocateWindow<TViewModel>()
        {
            var viewModelType = viewModelLocator.LocateFromViewModel(typeof(TViewModel));

            if (windows.TryGetValue(viewModelType, out var windowType))
                return windowType;

            throw new ArgumentException($"Could not find a window for '{viewModelType.Name}'");
        }

        private void OnViewRegistered(object sender, ViewRegisteredEventArgs e)
        {
            if (e.ViewType.IsAssignableTo(typeof(Window)))
                windows.Add(e.ViewModelType, e.ViewType);
            else
            if (e.ViewType.IsAssignableTo(typeof(UserControl)))
                views.Add(e.ViewModelType, e.ViewType);
        }
    }
}