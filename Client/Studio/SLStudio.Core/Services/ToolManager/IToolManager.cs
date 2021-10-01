namespace SLStudio.Core
{
    public interface IToolManager
    {
        void Register<TService, TTool>(TTool tool) where TService : class where TTool : TService, IToolItem;

        void Unregister(IToolItem tool);
    }
}
