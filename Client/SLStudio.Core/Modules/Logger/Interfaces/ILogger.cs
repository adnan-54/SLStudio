namespace SLStudio.Logger;

public interface ILogger
{
    string Name { get; }

    void Log(object message);

    void Log(object message, LogLevel? level);
}
