namespace SLStudio.Logger;

public static class LoggerExtensions
{
    private static LogLevel DefaultLevel => LogManager.Default.Configuration.DefaultLogLevel;

    public static void Debug(this ILogger logger, string message)
    {
        logger.Log(message, LogLevel.Debug);
    }

    public static void Debug<T0>(this ILogger logger, string format, T0 arg0)
    {
        Log(logger, LogLevel.Debug, format, arg0);
    }

    public static void Debug<T0, T1>(this ILogger logger, string format, T0 arg0, T1 arg1)
    {
        Log(logger, LogLevel.Debug, format, arg0, arg1);
    }

    public static void Debug<T0, T1, T2>(this ILogger logger, string format, T0 arg0, T1 arg1, T2 arg2)
    {
        Log(logger, LogLevel.Debug, format, arg0, arg1, arg2);
    }

    public static void Debug(this ILogger logger, string format, params object?[] args)
    {
        Log(logger, LogLevel.Debug, format, args);
    }

    public static void Information(this ILogger logger, string message)
    {
        logger.Log(message, LogLevel.Information);
    }

    public static void Information<T0>(this ILogger logger, string format, T0 arg0)
    {
        Log(logger, LogLevel.Information, format, arg0);
    }

    public static void Information<T0, T1>(this ILogger logger, string format, T0 arg0, T1 arg1)
    {
        Log(logger, LogLevel.Information, format, arg0, arg1);
    }

    public static void Information<T0, T1, T2>(this ILogger logger, string format, T0 arg0, T1 arg1, T2 arg2)
    {
        Log(logger, LogLevel.Information, format, arg0, arg1, arg2);
    }

    public static void Information(this ILogger logger, string format, params object?[] args)
    {
        Log(logger, LogLevel.Information, format, args);
    }

    public static void Warning(this ILogger logger, string message)
    {
        logger.Log(message, LogLevel.Warning);
    }

    public static void Warning<T0>(this ILogger logger, string format, T0 arg0)
    {
        Log(logger, LogLevel.Warning, format, arg0);
    }

    public static void Warning<T0, T1>(this ILogger logger, string format, T0 arg0, T1 arg1)
    {
        Log(logger, LogLevel.Warning, format, arg0, arg1);
    }

    public static void Warning<T0, T1, T2>(this ILogger logger, string format, T0 arg0, T1 arg1, T2 arg2)
    {
        Log(logger, LogLevel.Warning, format, arg0, arg1, arg2);
    }

    public static void Warning(this ILogger logger, string format, params object?[] args)
    {
        Log(logger, LogLevel.Warning, format, args);
    }

    public static void Error(this ILogger logger, string message)
    {
        logger.Log(message, LogLevel.Error);
    }

    public static void Error<T0>(this ILogger logger, string format, T0 arg0)
    {
        Log(logger, LogLevel.Error, format, arg0);
    }

    public static void Error<T0, T1>(this ILogger logger, string format, T0 arg0, T1 arg1)
    {
        Log(logger, LogLevel.Error, format, arg0, arg1);
    }

    public static void Error<T0, T1, T2>(this ILogger logger, string format, T0 arg0, T1 arg1, T2 arg2)
    {
        Log(logger, LogLevel.Error, format, arg0, arg1, arg2);
    }

    public static void Error(this ILogger logger, string format, params object?[] args)
    {
        Log(logger, LogLevel.Error, format, args);
    }

    public static void Fatal(this ILogger logger, string message)
    {
        logger.Log(message, LogLevel.Fatal);
    }

    public static void Fatal<T0>(this ILogger logger, string format, T0 arg0)
    {
        Log(logger, LogLevel.Fatal, format, arg0);
    }

    public static void Fatal<T0, T1>(this ILogger logger, string format, T0 arg0, T1 arg1)
    {
        Log(logger, LogLevel.Fatal, format, arg0, arg1);
    }

    public static void Fatal<T0, T1, T2>(this ILogger logger, string format, T0 arg0, T1 arg1, T2 arg2)
    {
        Log(logger, LogLevel.Fatal, format, arg0, arg1, arg2);
    }

    public static void Fatal(this ILogger logger, string format, params object?[] args)
    {
        Log(logger, LogLevel.Fatal, format, args);
    }

    public static void Default(this ILogger logger, string message)
    {
        logger.Log(message, DefaultLevel);
    }

    public static void Default<T0>(this ILogger logger, string format, T0 arg0)
    {
        Log(logger, DefaultLevel, format, arg0);
    }

    public static void Default<T0, T1>(this ILogger logger, string format, T0 arg0, T1 arg1)
    {
        Log(logger, DefaultLevel, format, arg0, arg1);
    }

    public static void Default<T0, T1, T2>(this ILogger logger, string format, T0 arg0, T1 arg1, T2 arg2)
    {
        Log(logger, DefaultLevel, format, arg0, arg1, arg2);
    }

    public static void Default(this ILogger logger, string format, params object?[] args)
    {
        Log(logger, DefaultLevel, format, args);
    }

    public static void Exception(this ILogger logger, Exception exception)
    {
        logger.Log(exception.ToString(), LogLevel.Error);
    }

    internal static void Log<T0>(ILogger logger, LogLevel level, string format, T0 arg0)
    {
        if (logger.IsEnabled(level))
            Log(logger, level, format, new object?[] { arg0 });
    }

    internal static void Log<T0, T1>(ILogger logger, LogLevel level, string format, T0 arg0, T1 arg1)
    {
        if (logger.IsEnabled(level))
            Log(logger, level, format, new object?[] { arg0, arg1 });
    }

    internal static void Log<T0, T1, T2>(ILogger logger, LogLevel level, string format, T0 arg0, T1 arg1, T2 arg2)
    {
        if (logger.IsEnabled(level))
            Log(logger, level, format, new object?[] { arg0, arg1, arg2 });
    }

    internal static void Log(ILogger logger, LogLevel level, string format, params object?[] args)
    {
        var message = string.Format(format, args);
        logger.Log(message, level);
    }
}
