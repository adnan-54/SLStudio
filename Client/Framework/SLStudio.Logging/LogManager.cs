using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace SLStudio.Logging
{
    public partial class LogManager : ILogManager
    {
        private readonly Container container;
        private readonly IObjectFactory objectFactory;
        private readonly ILoggerFactory loggerFactory;
        private readonly IInternalLogger internalLogger;
        private readonly ILoggingService loggingService;
        private readonly IConfigurationService configurationService;
        private readonly ConsoleLogger consoleLogger;
        private readonly DebugLogger debugLogger;
        private bool initialized;

        private LogManager()
        {
            var logsDir = StudioConstants.LogsDirectory;
            if (!Directory.Exists(logsDir))
                Directory.CreateDirectory(logsDir);

            container = new Container();
            objectFactory = new DefaultObjectFactory(container);
            loggerFactory = new DefaultLoggerFactory(objectFactory);
            internalLogger = new DefaultInternalLogger();
            configurationService = new DefaultConfigurationService();
            loggingService = new DefaultLoggingService(this, internalLogger, configurationService);
            consoleLogger = new ConsoleLogger(this, configurationService);
            debugLogger = new DebugLogger(this, configurationService);

            RegisterInstances();
        }

        public event EventHandler<LogCompletedEventArgs> LogCompleted;

        public event EventHandler Initialized;

        public event EventHandler DumpRequested;

        public bool IsInitialized => initialized;

        ILogger ILogManager.GetLogger(string name)
        {
            return loggerFactory.Create(name);
        }

        public void Initialize(LoggerConfiguration configuration)
        {
            if (initialized)
                return;
            initialized = true;

            configurationService.Initialize(configuration);

            OnInitialized();
        }

        public IEnumerable<Log> GetLogs()
        {
            return loggingService.GetLogs();
        }

        public long GetLogsSize()
        {
            return loggingService.GetSize();
        }

        public void DeleteAllLogs()
        {
            loggingService.DeleteAll();
        }

        public string GetInternalLogs()
        {
            return internalLogger.GetLogs();
        }

        public long GetInternalLogsSize()
        {
            return internalLogger.GetSize();
        }

        public void ExportToZip(string fileName)
        {
            using var zipExporter = new ZipExporter(fileName);
            zipExporter.Export();
        }

        internal void OnLogCompleted(Log log)
        {
            LogCompleted?.Invoke(this, new LogCompletedEventArgs(log));
        }

        private void OnInitialized()
        {
            Initialized?.Invoke(this, EventArgs.Empty);
        }

        private void OnDumpRequested()
        {
            DumpRequested?.Invoke(this, EventArgs.Empty);
        }

        private void RegisterInstances()
        {
            container.RegisterInstance(this);
            container.RegisterInstance(objectFactory);
            container.RegisterInstance(loggerFactory);
            container.RegisterInstance(loggingService);
            container.RegisterInstance(internalLogger);
            container.RegisterInstance(configurationService);
            container.RegisterInstance(consoleLogger);
            container.RegisterInstance(debugLogger);
            container.Register<DefaultLogger>();
        }
    }

    public interface ILogManager
    {
        event EventHandler<LogCompletedEventArgs> LogCompleted;

        void Initialize(LoggerConfiguration configuration);

        ILogger GetLogger(string name);

        IEnumerable<Log> GetLogs();

        long GetLogsSize();

        void DeleteAllLogs();

        string GetInternalLogs();

        long GetInternalLogsSize();

        void ExportToZip(string fileName);
    }
}