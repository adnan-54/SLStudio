using DevExpress.Mvvm;
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

		public ViewLocator(IViewModelLocator viewModelLocator, IMessenger messenger)
		{
			this.viewModelLocator = viewModelLocator;
			views = new Dictionary<Type, Type>();
			windows = new Dictionary<Type, Type>();

			messenger.Register<ViewRegisteredMessage>(this, OnViewRegistered);
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

		private void OnViewRegistered(ViewRegisteredMessage message)
		{
			if (message.View.IsAssignableTo(typeof(Window)))
				windows.Add(message.ViewModel, message.View);
			else
			if (message.View.IsAssignableTo(typeof(UserControl)))
				views.Add(message.ViewModel, message.View);
		}
	}
}
