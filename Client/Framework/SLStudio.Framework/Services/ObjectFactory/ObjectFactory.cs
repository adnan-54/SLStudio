using System;

namespace SLStudio
{
    internal class ObjectFactory : IObjectFactory
    {
        private readonly IContainer container;

        public ObjectFactory(IContainer container)
        {
            this.container = container;
        }

        TObject IObjectFactory.Create<TObject>()
        {
            return container.GetInstance<TObject>();
        }

        object IObjectFactory.Create(Type type)
        {
            return container.GetInstance(type);
        }

        TObject IObjectFactory.Create<TObject>(Type type)
        {
            return (TObject)container.GetInstance(type);
        }
    }
}