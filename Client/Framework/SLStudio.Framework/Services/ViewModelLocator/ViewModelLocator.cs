using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace SLStudio
{
	internal class ViewModelLocator : Service, IViewModelLocator
	{
		private readonly Dictionary<Type, Type> fromViewModels;
		private readonly Dictionary<Type, Type> fromViews;

		public ViewModelLocator(IMessenger messenger)
		{
			fromViewModels = new Dictionary<Type, Type>();
			fromViews = new Dictionary<Type, Type>();

			messenger.Register<ViewRegisteredMessage>(this, OnViewRegistered);
			messenger.Register<ViewModelRegisteredMessage>(this, OnViewModelRegistered);
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

		private void OnViewRegistered(ViewRegisteredMessage message)
		{
			var viewType = message.View;
			var viewModelType = message.ViewModel;

			fromViews.TryAdd(viewType, viewModelType);
		}

		private void OnViewModelRegistered(ViewModelRegisteredMessage message)
		{
			var serviceType = message.Service;
			var implementationType = message.Implementation;

			fromViewModels.TryAdd(serviceType, serviceType);
			fromViewModels.TryAdd(implementationType, serviceType);
		}
	}
}
