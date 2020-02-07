using System;

namespace SLStudio.Core
{
    public interface IObjectFactory
    {
        Type Create<Type>() where Type : class;

        object Create(Type type);
    }
}