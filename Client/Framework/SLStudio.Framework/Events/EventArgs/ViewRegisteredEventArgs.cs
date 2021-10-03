using System;

namespace SLStudio
{
    public class ViewRegisteredEventArgs : EventArgs
    {
        public ViewRegisteredEventArgs(Type viewType, Type viewModelType)
        {
            ViewType = viewType;
            ViewModelType = viewModelType;
        }

        public Type ViewType { get; }

        public Type ViewModelType { get; }
    }
}