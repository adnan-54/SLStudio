using System;
using System.Collections.Generic;

namespace SLStudio
{
    public interface IServiceCollection
    {
        IReadOnlyDictionary<Type, object> GetAll();

        TService Get<TService>()
            where TService : class, IService;

        void Lock();
    }
}