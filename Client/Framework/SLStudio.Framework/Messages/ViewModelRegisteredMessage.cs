using System;

namespace SLStudio
{
    internal class ViewModelRegisteredMessage
    {
        public ViewModelRegisteredMessage(Type serviceType, Type implementationType)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
        }

        public Type ServiceType { get; }

        public Type ImplementationType { get; }
    }
}
