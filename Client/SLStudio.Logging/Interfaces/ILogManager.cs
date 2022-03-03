using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Logging;

public interface ILogManager
{
    ILogger CreateLogger(string name);
}

public interface ILogger
{
    string Name { get; }

    void Log(string message, LogLevel level);
}

public enum LogLevel
{
    Debug,
    Information,
    Warning,
    Error,
    Fatal
}

public record Log(Guid Id, Guid InstanceId, string Sender, LogLevel Level, string Message, string StackTrace, DateTime Date);