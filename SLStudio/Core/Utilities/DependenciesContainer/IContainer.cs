using SLStudio.Core.Utilities.DependenciesContainer;
using System;
using System.Reflection;

namespace SLStudio.Core
{
    public interface IContainer
    {
        Container Singleton<TImplementation>(string key = null);

        Container Singleton<TService, TImplementation>(string key = null) where TImplementation : TService;

        Container PerRequest<TImplementation>(string key = null);

        Container PerRequest<TService, TImplementation>(string key = null) where TImplementation : TService;

        Container Instance<TService>(TService instance);

        Container Handler<TService>(Func<Container, object> handler);

        Container AllTypesOf<TService>(Assembly assembly, Func<Type, bool> filter = null);
    }
}