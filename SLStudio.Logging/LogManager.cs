namespace SLStudio.Logging
{
    public static class LogManager
    {
        public static ILogger GetLog(string name)
        {
            return new Logger(name);
        }
    }
}
