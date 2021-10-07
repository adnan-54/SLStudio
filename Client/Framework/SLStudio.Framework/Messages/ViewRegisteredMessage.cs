using System;

namespace SLStudio
{
	internal class ViewRegisteredMessage
	{
		public ViewRegisteredMessage(Type view, Type viewModel)
		{
			View = view;
			ViewModel = viewModel;
		}

		public Type View { get; }

		public Type ViewModel { get; }
	}
}
