using System;

namespace SLStudio
{
    public interface IObjectFactory : IStudioService
    {
        TService Create<TService>()
            where TService : class;

        object Create(Type serviceType);

        TService Create<TService>(Type serviceType)
            where TService : class;
    }
}