namespace SLStudio.Logger;

public interface ILogger
{
    string Name { get; }

    bool IsEnabled(LogLevel logLevel);

    void Log(string message, LogLevel level);
}
