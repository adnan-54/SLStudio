namespace SLStudio.Core
{
    public interface ILoggingFactory
    {
        ILogger GetLoggerFor<TType>() where TType : class;
    }
}
