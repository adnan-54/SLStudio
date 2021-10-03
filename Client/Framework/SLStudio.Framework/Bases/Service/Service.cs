namespace SLStudio
{
    public abstract class Service : IService
    {
        protected Service()
        {
            Name = GetType().Name;
        }

        public string Name { get; }
    }
}