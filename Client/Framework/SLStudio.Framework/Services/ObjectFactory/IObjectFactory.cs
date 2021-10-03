using System;

namespace SLStudio
{
    public interface IObjectFactory : IService
    {
        TObject Create<TObject>()
               where TObject : class;

        object Create(Type type);

        TObject Create<TObject>(Type type)
            where TObject : class;
    }
}