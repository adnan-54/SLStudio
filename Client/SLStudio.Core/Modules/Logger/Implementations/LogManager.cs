namespace SLStudio.Logger;

public partial class LogManager : ILogManager
{
    private readonly Dictionary<string, ILogger> cache;
    private readonly ILogger internalLogger;
    private readonly ILogger defaultLogger;
    private readonly ILogsRepository logsRepository;
    private readonly LoggerConfiguration defaultConfiguration;
    private LoggerConfiguration currentConfiguration;

    private LogManager()
    {
        cache = new();
        internalLogger = new InternalLogger(this);
        logsRepository = new LogsRepository(this, internalLogger);
        defaultLogger = new ExternalLogger(this, internalLogger, logsRepository, "Default Logger");
        defaultConfiguration = new(false, false, false, LogLevel.Information, LogLevel.Information);
        currentConfiguration = defaultConfiguration;
    }

    public event EventHandler? Initialized;

    public event EventHandler<Log>? LogAdded;

    public LoggerConfiguration Configuration => currentConfiguration;

    public bool IsInitialized => currentConfiguration != defaultConfiguration;

    public void Initialize(LoggerConfiguration configuration)
    {
        if (IsInitialized)
            throw new InvalidOperationException($"{nameof(LogManager)} is already initialized");

        currentConfiguration = configuration;
        Initialized?.Invoke(this, EventArgs.Empty);
    }

    ILogger ILogManager.GetLogger(object? name)
    {
        if (name is null)
            return defaultLogger;

        var loggerName = name.ToString()!;
        if (!cache.TryGetValue(loggerName, out var logger))
        {
            logger = new ExternalLogger(this, internalLogger, logsRepository, loggerName);
            cache.Add(loggerName, logger);
        }

        return logger;
    }

    ILogger ILogManager.GetLogger<TType>()
    {
        return Default.GetLogger(typeof(TType).Name);
    }

    public Task RequestDump()
    {
        return logsRepository.DumpQueue();
    }

    void ILogManager.OnLogAdded(Log log)
    {
        LogAdded?.Invoke(this, log);
    }
}

