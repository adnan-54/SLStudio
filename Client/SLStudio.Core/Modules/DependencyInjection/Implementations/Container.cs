namespace SLStudio;

internal class Container : IContainer
{
    private readonly SimpleInjector.Container container;

    public Container()
    {
        container = new();
    }

}
