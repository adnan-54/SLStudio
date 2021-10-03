using System;

namespace SLStudio.Core
{
    public interface IObjectFactory
    {
        TService Create<TService>() where TService : class;

        object Create(Type serviceType);

        TService Create<TService>(Type serviceType) where TService : class;
    }
}