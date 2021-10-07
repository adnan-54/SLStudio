using System;

namespace SLStudio
{
	internal class ViewModelRegisteredMessage
	{
		public ViewModelRegisteredMessage(Type service, Type implementation)
		{
			Service = service;
			Implementation = implementation;
		}

		public Type Service { get; }

		public Type Implementation { get; }
	}
}
