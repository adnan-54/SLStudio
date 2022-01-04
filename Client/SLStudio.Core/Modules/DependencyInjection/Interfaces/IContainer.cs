namespace SLStudio;

public interface IContainer : IContainerContext, IObjectFactory, IServiceProvider, IAsyncDisposable, IDisposable
{
    bool IsLocked { get; }

    void Verify();
}
