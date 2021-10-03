using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SLStudio.Logging
{
    internal class DefaultLoggingService : ILoggingService
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<DefaultLoggingService>();

        private readonly ILogRespository logRespository;
        private readonly FileInfo dbFile;
        private readonly LogsProcessor logsProcessor;

        public DefaultLoggingService(LogManager logManager, IInternalLogger internalLogger, IConfigurationService configurationService)
        {
            logRespository = new DefaultLogRespository(configurationService);
            dbFile = new FileInfo(StudioConstants.StudioLogFile);
            logsProcessor = new LogsProcessor(logManager, internalLogger, configurationService, logRespository);
        }

        public void EnqueueLog(string sender, string title, string message, string level, string callerFile, string callerMember, int callerLine)
        {
            var log = CreateLog(title, message, sender, level, callerFile, callerMember, callerLine, Environment.StackTrace);
            logsProcessor.EnqueueLog(log);
        }

        public IEnumerable<Log> GetLogs()
        {
            return logRespository.GetAll();
        }

        public long GetSize()
        {
            dbFile.Refresh();
            return dbFile.Length;
        }

        public void DeleteAll()
        {
            logRespository.DeleteAll();
            logger.Info("All logs have been successfully deleted", "Clear Logs");
        }

        private static Log CreateLog(string title, string message, string sender, string level, string callerFile, string callerMember, int callerLine, string stackTrace)
        {
            return new Log(title, message, sender, level, DateTime.Now.ToString(), callerFile, callerMember, callerLine, stackTrace, default);
        }

        private class LogsProcessor
        {
            private readonly LogManager logManager;
            private readonly IInternalLogger internalLogger;
            private readonly IConfigurationService configurationService;
            private readonly ILogRespository logRespository;
            private readonly BlockingCollection<Log> logs;
            private Action<Log> enqueueLog;
            private CancellationTokenSource cancellationToken;
            private Task<bool> processTask;
            private bool isProcessing;

            public LogsProcessor(LogManager logManager, IInternalLogger internalLogger, IConfigurationService configurationService, ILogRespository logRespository)
            {
                this.logManager = logManager;
                this.internalLogger = internalLogger;
                this.configurationService = configurationService;
                this.logRespository = logRespository;

                logs = new BlockingCollection<Log>(new ConcurrentQueue<Log>());
                enqueueLog = log => logs.Add(log);

                logManager.Initialized += LogManagerInitialized;
                logManager.DumpRequested += LogManagerDumpRequested;
            }

            public void EnqueueLog(Log log)
            {
                enqueueLog(log);
            }

            private void Start()
            {
                if (!logManager.IsInitialized || isProcessing)
                    return;
                isProcessing = true;

                enqueueLog = log => logs.Add(log);

                cancellationToken = new CancellationTokenSource();
                processTask = Task.Run(Process, cancellationToken.Token);
                processTask.ContinueWith(t => StopAndDump());
            }

            private void Stop()
            {
                if (!isProcessing)
                    return;
                isProcessing = false;

                enqueueLog = log => internalLogger.Log("A log has been queued, but the logs processor has been stopped", log);

                var taskStatus = CreateTaskStatus(processTask);
                internalLogger.Log($"The logging processor has stopped with the result: {taskStatus}");

                cancellationToken.Cancel();
                cancellationToken.Dispose();
                cancellationToken = null;

                processTask.Dispose();
                processTask = null;
            }

            private Task<bool> Process()
            {
                Log lastDequeuedLog = null;

                try
                {
                    while (CanProcess())
                    {
                        cancellationToken.Token.ThrowIfCancellationRequested();

                        lastDequeuedLog = logs.Take();
                        lastDequeuedLog = UpdateLog(lastDequeuedLog);

                        if (!CanLog(lastDequeuedLog))
                            continue;

                        logRespository.Add(lastDequeuedLog);
                        logManager.OnLogCompleted(lastDequeuedLog);
                    }
                }
                catch (Exception ex)
                {
                    internalLogger.Log(ex, lastDequeuedLog);
                    throw;
                }

                return Task.FromResult(true);
            }

            private bool CanProcess()
            {
                return isProcessing && logManager.IsInitialized;
            }

            private void StopAndDump()
            {
                Stop();

                if (logs.Any())
                    Dump();
            }

            private void Dump()
            {
                var wasProcessing = isProcessing;
                if (isProcessing)
                    Stop();

                var sb = new StringBuilder();
                sb.AppendLine("Dumping all logs...");
                sb.AppendLine($"{logs.Count} logs remaining in the queue");
                while (logs.Any())
                    sb.AppendLine($"{logs.Take()}");

                internalLogger.Log(sb.ToString().Trim());

                if (wasProcessing && !isProcessing)
                    Start();
            }

            private Log UpdateLog(Log log)
            {
                if (string.IsNullOrEmpty(log.Level))
                    log = log with { Level = $"{configurationService.DefaultLogLevel}" };

                var level = Enum.Parse<LogLevel>(log.Level, true);
                if (level < LogLevel.Error)
                    log = log with { StackTrace = string.Empty };

                return log;
            }

            private bool CanLog(Log log)
            {
                var miminumLevel = configurationService.MinimumLogLevel;
                var level = Enum.Parse<LogLevel>(log.Level, true);

                if (level == LogLevel.Debug && configurationService.IgnoreDebugLevel)
                    return false;

                return level >= miminumLevel;
            }

            private static string CreateTaskStatus(Task<bool> task)
            {
                var properties = task.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                var sb = new StringBuilder();
                foreach (var property in properties)
                    sb.AppendLine($"{property.Name}: {TryGetValue(property)}, ");

                return $"{sb}";

                object TryGetValue(PropertyInfo propertyInfo)
                {
                    try
                    {
                        var value = propertyInfo.GetValue(task);
                        return value;
                    }
                    catch
                    {
                        return false;
                    }
                }
            }

            private void LogManagerInitialized(object sender, EventArgs e)
            {
                Start();
            }

            private void LogManagerDumpRequested(object sender, EventArgs e)
            {
                Dump();
            }
        }
    }

    internal interface ILoggingService
    {
        void EnqueueLog(string sender, string title, string message, string level, string callerFile, string callerMember, int callerLine);

        IEnumerable<Log> GetLogs();

        long GetSize();

        void DeleteAll();
    }
}