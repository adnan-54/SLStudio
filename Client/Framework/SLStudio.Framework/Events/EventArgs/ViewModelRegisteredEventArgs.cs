using System;

namespace SLStudio
{
    public class ViewModelRegisteredEventArgs : EventArgs
    {
        public ViewModelRegisteredEventArgs(Type serviceType, Type implementationType)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
        }

        public Type ServiceType { get; }

        public Type ImplementationType { get; }
    }
}