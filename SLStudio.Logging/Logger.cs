namespace SLStudio.Logging
{
    class Logger : ILogger
    {
        private readonly string name;

        public Logger(string name)
        {
            this.name = name;
        }
    }
}
