﻿namespace SLStudio.Logger;

public interface ILogManager
{
    event EventHandler Initialized;

    event EventHandler<Log> LogAdded;

    LoggerConfiguration Configuration { get; }

    bool IsInitialized { get; }

    void Initialize(LoggerConfiguration loggerConfiguration);

    ILogger GetLogger(object? name);

    ILogger GetLogger<TType>()
        where TType : class;

    Task RequestDump();

    internal void OnLogAdded(Log log);
}
