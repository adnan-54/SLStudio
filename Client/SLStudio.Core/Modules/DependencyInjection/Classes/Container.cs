namespace SLStudio;

internal class Container : SimpleInjector.Container, IContainer
{
    public Container() : base()
    {
        Options.ResolveUnregisteredConcreteTypes = true;
    }
}
