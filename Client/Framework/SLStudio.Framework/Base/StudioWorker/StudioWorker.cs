namespace SLStudio
{
    public abstract class StudioWorker : IStudioWorker
    {
        protected StudioWorker()
        {
            Name = GetType().Name;
        }

        public virtual string Name { get; }
    }
}