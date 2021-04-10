namespace SLStudio
{
    internal class Module
    {
        protected void Register(IContainer container)
        {
            container.RegisterInstance(container);
            container.RegisterService<IMessenger, Messenger>();
        }
    }
}