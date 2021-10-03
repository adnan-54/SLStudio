using SimpleInjector;
using SimpleInjector.Advanced;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace SLStudio
{
    public interface IContainer : IService, IAsyncDisposable, IDisposable, IServiceProvider
    {
        event EventHandler<ExpressionBuildingEventArgs> ExpressionBuilding;

        event EventHandler<ExpressionBuiltEventArgs> ExpressionBuilt;

        event EventHandler<UnregisteredTypeEventArgs> ResolveUnregisteredType;

        ContainerCollectionRegistrator Collection { get; }

        ContainerScope ContainerScope { get; }

        bool IsLocked { get; }

        bool IsVerifying { get; }

        ContainerOptions Options { get; }

        void AddRegistration(Type serviceType, Registration registration);

        void AddRegistration<TService>(Registration registration) where TService : class;

        Task DisposeContainerAsync();

        IEnumerable<object> GetAllInstances(Type serviceType);

        IEnumerable<TService> GetAllInstances<TService>() where TService : class;

        InstanceProducer[] GetCurrentRegistrations();

        object GetInstance(Type serviceType);

        TService GetInstance<TService>() where TService : class;

        InstanceProducer GetRegistration(Type serviceType);

        InstanceProducer GetRegistration(Type serviceType, bool throwOnFailure);

        InstanceProducer<TService> GetRegistration<TService>() where TService : class;

        InstanceProducer<TService> GetRegistration<TService>(bool throwOnFailure);

        InstanceProducer[] GetRootRegistrations();

        IEnumerable<Type> GetTypesToRegister(Type serviceType, IEnumerable<Assembly> assemblies);

        IEnumerable<Type> GetTypesToRegister(Type serviceType, IEnumerable<Assembly> assemblies, TypesToRegisterOptions options);

        IEnumerable<Type> GetTypesToRegister(Type serviceType, params Assembly[] assemblies);

        IEnumerable<Type> GetTypesToRegister<TService>(IEnumerable<Assembly> assemblies);

        IEnumerable<Type> GetTypesToRegister<TService>(params Assembly[] assemblies);

        void Register(Type concreteType);

        void Register(Type openGenericServiceType, Assembly assembly, Lifestyle lifestyle);

        void Register(Type serviceType, Func<object> instanceCreator);

        void Register(Type serviceType, Func<object> instanceCreator, Lifestyle lifestyle);

        void Register(Type openGenericServiceType, IEnumerable<Assembly> assemblies);

        void Register(Type openGenericServiceType, IEnumerable<Assembly> assemblies, Lifestyle lifestyle);

        void Register(Type openGenericServiceType, IEnumerable<Type> implementationTypes);

        void Register(Type openGenericServiceType, IEnumerable<Type> implementationTypes, Lifestyle lifestyle);

        void Register(Type openGenericServiceType, params Assembly[] assemblies);

        void Register(Type serviceType, Type implementationType);

        void Register(Type serviceType, Type implementationType, Lifestyle lifestyle);

        void Register<TConcrete>() where TConcrete : class;

        void Register<TConcrete>(Lifestyle lifestyle) where TConcrete : class;

        void Register<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        void Register<TService, TImplementation>(Lifestyle lifestyle)
            where TService : class
            where TImplementation : class, TService;

        void Register<TService>(Func<TService> instanceCreator) where TService : class;

        void Register<TService>(Func<TService> instanceCreator, Lifestyle lifestyle) where TService : class;

        void RegisterCollection(Type serviceType, IEnumerable containerUncontrolledCollection);

        void RegisterCollection(Type serviceType, IEnumerable<Assembly> assemblies);

        void RegisterCollection(Type serviceType, IEnumerable<Registration> registrations);

        void RegisterCollection(Type serviceType, IEnumerable<Type> serviceTypes);

        void RegisterCollection(Type serviceType, params Assembly[] assemblies);

        void RegisterCollection<TService>(IEnumerable<Assembly> assemblies) where TService : class;

        void RegisterCollection<TService>(IEnumerable<Registration> registrations) where TService : class;

        void RegisterCollection<TService>(IEnumerable<TService> containerUncontrolledCollection) where TService : class;

        void RegisterCollection<TService>(IEnumerable<Type> serviceTypes) where TService : class;

        void RegisterCollection<TService>(params TService[] singletons) where TService : class;

        void RegisterConditional(Type serviceType, Func<TypeFactoryContext, Type> implementationTypeFactory, Lifestyle lifestyle, Predicate<PredicateContext> predicate);

        void RegisterConditional(Type serviceType, Registration registration, Predicate<PredicateContext> predicate);

        void RegisterConditional(Type serviceType, Type implementationType, Lifestyle lifestyle, Predicate<PredicateContext> predicate);

        void RegisterConditional(Type serviceType, Type implementationType, Predicate<PredicateContext> predicate);

        void RegisterConditional<TService, TImplementation>(Lifestyle lifestyle, Predicate<PredicateContext> predicate)
            where TService : class
            where TImplementation : class, TService;

        void RegisterConditional<TService, TImplementation>(Predicate<PredicateContext> predicate)
            where TService : class
            where TImplementation : class, TService;

        void RegisterConditional<TService>(Registration registration, Predicate<PredicateContext> predicate);

        void RegisterDecorator(Type serviceType, Func<DecoratorPredicateContext, Type> decoratorTypeFactory, Lifestyle lifestyle, Predicate<DecoratorPredicateContext> predicate);

        void RegisterDecorator(Type serviceType, Type decoratorType);

        void RegisterDecorator(Type serviceType, Type decoratorType, Lifestyle lifestyle);

        void RegisterDecorator(Type serviceType, Type decoratorType, Lifestyle lifestyle, Predicate<DecoratorPredicateContext> predicate);

        void RegisterDecorator(Type serviceType, Type decoratorType, Predicate<DecoratorPredicateContext> predicate);

        void RegisterDecorator<TService, TDecorator>()
            where TService : class
            where TDecorator : class, TService;

        void RegisterDecorator<TService, TDecorator>(Lifestyle lifestyle)
            where TService : class
            where TDecorator : class, TService;

        void RegisterInitializer(Action<InstanceInitializationData> instanceInitializer, Predicate<InitializerContext> predicate);

        void RegisterInitializer<TService>(Action<TService> instanceInitializer) where TService : class;

        void RegisterInstance(Type serviceType, object instance);

        void RegisterInstance<TService>(TService instance) where TService : class;

        void RegisterSingleton(Type serviceType, Func<object> instanceCreator);

        void RegisterSingleton(Type openGenericServiceType, IEnumerable<Assembly> assemblies);

        void RegisterSingleton(Type serviceType, object instance);

        void RegisterSingleton(Type openGenericServiceType, params Assembly[] assemblies);

        void RegisterSingleton(Type serviceType, Type implementationType);

        void RegisterSingleton<TConcrete>() where TConcrete : class;

        void RegisterSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService;

        void RegisterSingleton<TService>(Func<TService> instanceCreator) where TService : class;

        void RegisterSingleton<TService>(TService instance) where TService : class;

        void Verify();

        void Verify(VerificationOption option);
    }
}
