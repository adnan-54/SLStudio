namespace SLStudio.Core
{
    public interface ILoggingFactory
    {
        ILogger GetLogger<TType>() where TType : class;
    }
}