using System;

namespace SLStudio
{
    internal class ViewRegisteredMessage
    {
        public ViewRegisteredMessage(Type viewType, Type viewModelType)
        {
            ViewType = viewType;
            ViewModelType = viewModelType;
        }

        public Type ViewType { get; }

        public Type ViewModelType { get; }
    }
}
