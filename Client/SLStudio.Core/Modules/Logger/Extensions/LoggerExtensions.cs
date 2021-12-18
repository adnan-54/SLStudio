namespace SLStudio.Logger;

public static class LoggerExtensions
{
    public static void LogFormat(this ILogger logger, object message, params string[] values)
    {
        logger.Log(string.Format(message?.ToString() ?? string.Empty, values));
    }

    public static void LogFormat(this ILogger logger, object message, LogLevel level, params string[] values)
    {
        logger.Log(string.Format(message?.ToString() ?? string.Empty, values), level);
    }

    public static void Debug(this ILogger logger, object message)
    {
        logger.Log(message, LogLevel.Debug);
    }

    public static void DebugFormat(this ILogger logger, object message, params string[] values)
    {
        logger.LogFormat(message, LogLevel.Debug, values);
    }

    public static void Information(this ILogger logger, object message)
    {
        logger.Log(message, LogLevel.Information);
    }

    public static void InformationFormat(this ILogger logger, object message, params string[] values)
    {
        logger.LogFormat(message, LogLevel.Information, values);
    }

    public static void Warning(this ILogger logger, object message)
    {
        logger.Log(message, LogLevel.Warning);
    }

    public static void WarningFormat(this ILogger logger, object message, params string[] values)
    {
        logger.LogFormat(message, LogLevel.Warning, values);
    }

    public static void Error(this ILogger logger, object message)
    {
        logger.Log(message, LogLevel.Error);
    }

    public static void ErrorFormat(this ILogger logger, object message, params string[] values)
    {
        logger.LogFormat(message, LogLevel.Error, values);
    }

    public static void Fatal(this ILogger logger, object message)
    {
        logger.Log(message, LogLevel.Fatal);
    }

    public static void FatalFormat(this ILogger logger, object message, params string[] values)
    {
        logger.LogFormat(message, LogLevel.Fatal, values);
    }

    public static void Default(this ILogger logger, object message)
    {
        logger.Log(message);
    }

    public static void DefaultFormat(this ILogger logger, object message, params string[] values)
    {
        logger.LogFormat(message, values);
    }

    public static void Exception(this ILogger logger, Exception exception)
    {
        logger.Log(exception, LogLevel.Error);
    }
}
