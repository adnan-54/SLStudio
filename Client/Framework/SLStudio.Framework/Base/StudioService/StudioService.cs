namespace SLStudio
{
    public abstract class StudioService : IStudioService
    {
        protected StudioService()
        {
            Name = GetType().Name;
        }

        public virtual string Name { get; }
    }
}