﻿namespace SLStudio.Logger;

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
        defaultConfiguration = new(LogLevel.Information, LogLevel.Information);
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

    ILogger ILogManager.GetLogger(string? name)
    {
        if (string.IsNullOrEmpty(name))
            return defaultLogger;

        if (!cache.TryGetValue(name, out var logger))
        {
            logger = new ExternalLogger(this, internalLogger, logsRepository, name);
            cache.Add(name, logger);
        }

        return logger;
    }

    public Task Dump()
    {
        return logsRepository.DumpQueue();
    }

    void ILogManager.OnLogAdded(Log log)
    {
        LogAdded?.Invoke(this, log);
    }
}
