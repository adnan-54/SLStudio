namespace SLStudio
{
    public interface IScheduledAction
    {
        bool Completed { get; }

        void Run(IObjectFactory objectFactory);
    }
}