namespace SLStudio
{
    public interface IServiceContainer
    {
        TService GetService<TService>()
            where TService : class;

        bool TryGetService<TService>(out TService service)
            where TService : class;

        void RegisterToContainer(IContainer container);
    }

}
