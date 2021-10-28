using System;

namespace SLStudio
{
    internal class ViewModelRegisteredMessage
    {
        public ViewModelRegisteredMessage(Type concreteType)
        {
            ConcreteType = concreteType;
        }

        public ViewModelRegisteredMessage(Type serviceType, Type implementationType)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
        }

        public Type ConcreteType { get; }

        public Type ServiceType { get; }

        public Type ImplementationType { get; }
    }
}
