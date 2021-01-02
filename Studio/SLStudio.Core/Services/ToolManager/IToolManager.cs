namespace SLStudio.Core
{
    public interface IToolManager
    {
        void Register<TService, TTool>(TTool tool) where TService : class where TTool : TService, IToolPanel;

        void Unregister(IToolPanel tool);
    }
}
