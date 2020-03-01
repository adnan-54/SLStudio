using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SLStudio.Core.Utilities.DependenciesContainer
{
    public class Container : IContainer
    {
        private static readonly Type delegateType = typeof(Delegate);
        private static readonly Type enumerableType = typeof(IEnumerable);
        private readonly List<ContainerEntry> entries;

        public Container()
        {
            entries = new List<ContainerEntry>();
        }

        private Container(IEnumerable<ContainerEntry> entries)
        {
            this.entries = new List<ContainerEntry>(entries);
        }

        public bool EnablePropertyInjection { get; set; }

        public void RegisterInstance(Type service, string key, object implementation)
        {
            RegisterHandler(service, key, container => implementation);
        }

        public void RegisterPerRequest(Type service, string key, Type implementation)
        {
            RegisterHandler(service, key, container => container.BuildInstance(implementation));
        }

        public void RegisterSingleton(Type service, string key, Type implementation)
        {
            object singleton = null;
            RegisterHandler(service, key, container => singleton ?? (singleton = container.BuildInstance(implementation)));
        }

        public void RegisterHandler(Type service, string key, Func<Container, object> handler)
        {
            GetOrCreateEntry(service, key).Add(handler);
        }

        public void UnregisterHandler(Type service, string key)
        {
            var entry = GetEntry(service, key);
            if (entry != null)
            {
                entries.Remove(entry);
            }
        }

        public object GetInstance(Type service, string key)
        {
            var entry = GetEntry(service, key);
            if (entry != null)
            {
                var instance = entry.Single()(this);

                if (EnablePropertyInjection && instance != null)
                    BuildUp(instance);

                return instance;
            }

            if (service == null)
            {
                return null;
            }

            if (delegateType.GetTypeInfo().IsAssignableFrom(service.GetTypeInfo()))
            {
                var typeToCreate = service.GetTypeInfo().GenericTypeArguments[0];
                var factoryFactoryType = typeof(FactoryFactory<>).MakeGenericType(typeToCreate);
                var factoryFactoryHost = Activator.CreateInstance(factoryFactoryType);
                var factoryFactoryMethod = factoryFactoryType.GetRuntimeMethod("Create", new Type[] { typeof(Container) });
                return factoryFactoryMethod.Invoke(factoryFactoryHost, new object[] { this });
            }

            if (enumerableType.GetTypeInfo().IsAssignableFrom(service.GetTypeInfo()) && service.GetTypeInfo().IsGenericType)
            {
                var listType = service.GetTypeInfo().GenericTypeArguments[0];
                var instances = GetAllInstances(listType).ToList();
                var array = Array.CreateInstance(listType, instances.Count);

                for (var i = 0; i < array.Length; i++)
                {
                    if (EnablePropertyInjection)
                    {
                        BuildUp(instances[i]);
                    }

                    array.SetValue(instances[i], i);
                }

                return array;
            }

            return null;
        }

        public bool HasHandler(Type service, string key)
        {
            return GetEntry(service, key) != null;
        }

        public IEnumerable<object> GetAllInstances(Type service, string key = null)
        {
            var entries = GetEntry(service, key);

            if (entries == null)
            {
                return new object[0];
            }

            var instances = entries.Select(e => e(this));

            foreach (var instance in instances)
            {
                if (EnablePropertyInjection && instance != null)
                {
                    BuildUp(instance);
                }
            }

            return instances;
        }

        public void BuildUp(object instance)
        {
            var properties = instance
                .GetType()
                .GetRuntimeProperties()
                .Where(p => p.CanRead && p.CanWrite && p.PropertyType.GetTypeInfo().IsInterface);

            foreach (var property in properties)
            {
                var value = GetInstance(property.PropertyType, null);

                if (value != null)
                {
                    property.SetValue(instance, value, null);
                }
            }
        }

        public Container CreateChildContainer()
        {
            return new Container(entries);
        }

        private ContainerEntry GetOrCreateEntry(Type service, string key)
        {
            var entry = GetEntry(service, key);
            if (entry == null)
            {
                entry = new ContainerEntry { Service = service, Key = key };
                entries.Add(entry);
            }

            return entry;
        }

        private ContainerEntry GetEntry(Type service, string key)
        {
            if (service == null)
            {
                return entries.FirstOrDefault(x => x.Key == key);
            }

            if (key == null)
            {
                return entries.FirstOrDefault(x => x.Service == service && x.Key == null)
                       ?? entries.FirstOrDefault(x => x.Service == service);
            }

            return entries.FirstOrDefault(x => x.Service == service && x.Key == key);
        }

        protected object BuildInstance(Type type)
        {
            var args = DetermineConstructorArgs(type);
            return ActivateInstance(type, args);
        }

        protected virtual object ActivateInstance(Type type, object[] args)
        {
            var instance = args.Length > 0 ? System.Activator.CreateInstance(type, args) : System.Activator.CreateInstance(type);
            Activated(instance);
            return instance;
        }

        public event Action<object> Activated = delegate { };

        private object[] DetermineConstructorArgs(Type implementation)
        {
            var args = new List<object>();
            var constructor = SelectEligibleConstructor(implementation);

            if (constructor != null)
            {
                args.AddRange(constructor.GetParameters().Select(info => GetInstance(info.ParameterType, null)));
            }

            return args.ToArray();
        }

        private ConstructorInfo SelectEligibleConstructor(Type type)
        {
            return type.GetTypeInfo().DeclaredConstructors
                .Where(c => c.IsPublic)
                .Select(c => new
                {
                    Constructor = c,
                    HandledParamters = c.GetParameters().Count(p => HasHandler(p.ParameterType, null))
                })
                .OrderByDescending(c => c.HandledParamters)
                .Select(c => c.Constructor)
                .FirstOrDefault();
        }

        public Container Singleton<TImplementation>(string key = null)
        {
            return Singleton<TImplementation, TImplementation>(key);
        }

        public Container Singleton<TService, TImplementation>(string key = null) where TImplementation : TService
        {
            RegisterSingleton(typeof(TService), key, typeof(TImplementation));
            return this;
        }

        public Container PerRequest<TImplementation>(string key = null)
        {
            return PerRequest<TImplementation, TImplementation>(key);
        }

        public Container PerRequest<TService, TImplementation>(string key = null) where TImplementation : TService
        {
            RegisterPerRequest(typeof(TService), key, typeof(TImplementation));
            return this;
        }

        public Container Instance<TService>(TService instance)
        {
            RegisterInstance(typeof(TService), null, instance);
            return this;
        }

        public Container Handler<TService>(Func<Container, object> handler)
        {
            RegisterHandler(typeof(TService), null, handler);
            return this;
        }

        public Container AllTypesOf<TService>(Assembly assembly, Func<Type, bool> filter = null)
        {
            if (filter == null)
            {
                filter = type => true;
            }

            var serviceType = typeof(TService);
            var types = from type in assembly.DefinedTypes
                        where serviceType.GetTypeInfo().IsAssignableFrom(type)
                              && !type.IsAbstract
                              && !type.IsInterface
                              && filter(type.AsType())
                        select type;

            foreach (var type in types)
            {
                RegisterSingleton(typeof(TService), null, type.AsType());
            }

            return this;
        }

        public TService GetInstance<TService>(string key = null)
        {
            return (TService)GetInstance(typeof(TService), key);
        }

        public IEnumerable<TService> GetAllInstances<TService>(string key = null)
        {
            return GetAllInstances(typeof(TService), key).Cast<TService>();
        }

        public bool HasHandler<TService>(string key = null)
        {
            return HasHandler(typeof(TService), key);
        }

        public void UnregisterHandler<TService>(string key = null)
        {
            UnregisterHandler(typeof(TService), key);
        }

        private class ContainerEntry : List<Func<Container, object>>
        {
            public string Key;
            public Type Service;
        }

        private class FactoryFactory<T>
        {
            public Func<T> Create(Container container)
            {
                return () => (T)container.GetInstance(typeof(T), null);
            }
        }
    }
}