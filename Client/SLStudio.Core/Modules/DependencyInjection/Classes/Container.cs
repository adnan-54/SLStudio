using System.Reflection;
using SimpleInjector;

namespace SLStudio;

internal class Container : SimpleInjector.Container, IContainer
{
    public Container() : base()
    {
        Options.ResolveUnregisteredConcreteTypes = true;
    }

    void IContainer.Register(Type openGenericServiceType, Assembly assembly, Lifestyle lifestyle)
    {
        Register(openGenericServiceType, assembly, ConvertLifestyle(lifestyle));
    }

    void IContainer.Register(Type serviceType, Func<object> instanceCreator, Lifestyle lifestyle)
    {
        Register(serviceType, instanceCreator, ConvertLifestyle(lifestyle));
    }

    void IContainer.Register(Type openGenericServiceType, IEnumerable<Assembly> assemblies, Lifestyle lifestyle)
    {
        Register(openGenericServiceType, assemblies, ConvertLifestyle(lifestyle));
    }

    void IContainer.Register(Type openGenericServiceType, IEnumerable<Type> implementationTypes, Lifestyle lifestyle)
    {
        Register(openGenericServiceType, implementationTypes, ConvertLifestyle(lifestyle));
    }

    void IContainer.Register(Type serviceType, Type implementationType, Lifestyle lifestyle)
    {
        Register(serviceType, implementationType, ConvertLifestyle(lifestyle));
    }

    void IContainer.Register<TConcrete>(Lifestyle lifestyle)
    {
        Register<TConcrete>(ConvertLifestyle(lifestyle));
    }

    void IContainer.Register<TService, TImplementation>(Lifestyle lifestyle)
    {
        Register<TService, TImplementation>(ConvertLifestyle(lifestyle));
    }

    void IContainer.Register<TService>(Func<TService> instanceCreator, Lifestyle lifestyle)
    {
        Register(instanceCreator, ConvertLifestyle(lifestyle));
    }

    void IContainer.RegisterConditional(Type serviceType, Func<TypeFactoryContext, Type> implementationTypeFactory, Lifestyle lifestyle, Predicate<PredicateContext> predicate)
    {
        RegisterConditional(serviceType, implementationTypeFactory, ConvertLifestyle(lifestyle), predicate);
    }

    void IContainer.RegisterConditional(Type serviceType, Type implementationType, Lifestyle lifestyle, Predicate<PredicateContext> predicate)
    {
        RegisterConditional(serviceType, implementationType, ConvertLifestyle(lifestyle), predicate);
    }

    void IContainer.RegisterConditional<TService, TImplementation>(Lifestyle lifestyle, Predicate<PredicateContext> predicate)
    {
        RegisterConditional<TService, TImplementation>(ConvertLifestyle(lifestyle), predicate);
    }

    void IContainer.RegisterDecorator(Type serviceType, Func<DecoratorPredicateContext, Type> decoratorTypeFactory, Lifestyle lifestyle, Predicate<DecoratorPredicateContext> predicate)
    {
        RegisterDecorator(serviceType, decoratorTypeFactory, ConvertLifestyle(lifestyle), predicate);
    }

    void IContainer.RegisterDecorator(Type serviceType, Type decoratorType, Lifestyle lifestyle)
    {
        RegisterDecorator(serviceType, decoratorType, ConvertLifestyle(lifestyle));
    }

    void IContainer.RegisterDecorator(Type serviceType, Type decoratorType, Lifestyle lifestyle, Predicate<DecoratorPredicateContext> predicate)
    {
        RegisterDecorator(serviceType, decoratorType, ConvertLifestyle(lifestyle), predicate);
    }

    void IContainer.RegisterDecorator<TService, TDecorator>(Lifestyle lifestyle)
    {
        RegisterDecorator<TService, TDecorator>(ConvertLifestyle(lifestyle));
    }

    private static SimpleInjector.Lifestyle ConvertLifestyle(Lifestyle lifestyle)
    {
        if (lifestyle == Lifestyle.Singleton)
            return SimpleInjector.Lifestyle.Singleton;
        return SimpleInjector.Lifestyle.Transient;
    }
}
