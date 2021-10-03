namespace SLStudio
{
    internal class Container : SimpleInjector.Container, IContainer
    {
        public Container() : base()
        {
            Name = nameof(Container);
            Options.ResolveUnregisteredConcreteTypes = true;
        }

        public string Name { get; }
    }
}