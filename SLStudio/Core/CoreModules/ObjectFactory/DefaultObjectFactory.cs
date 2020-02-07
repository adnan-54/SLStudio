using Caliburn.Micro;
using System;

namespace SLStudio.Core.CoreModules.ObjectFactory
{
    internal class DefaultObjectFactory : IObjectFactory
    {
        private readonly SimpleContainer container;

        public DefaultObjectFactory(SimpleContainer container)
        {
            this.container = container;
        }

        TService IObjectFactory.Create<TService>()
        {
            return container.GetInstance<TService>();
        }

        object IObjectFactory.Create(Type type)
        {
            return container.Instance(type);
        }
    }
}