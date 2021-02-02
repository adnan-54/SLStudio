using System;
using System.Runtime.CompilerServices;

namespace SLStudio.Logging
{
    public interface ILogger
    {
        string Name { get; }

        void Log(object message, string title = null, [CallerFilePath] string callerFile = null, [CallerMemberName] string callerMember = null, [CallerLineNumber] int callerLine = 0);

        void Log(Exception exception, [CallerFilePath] string callerFile = null, [CallerMemberName] string callerMember = null, [CallerLineNumber] int callerLine = 0);

        void Debug(object message, string title = null, [CallerFilePath] string callerFile = null, [CallerMemberName] string callerMember = null, [CallerLineNumber] int callerLine = 0);

        void Debug(Exception exception, [CallerFilePath] string callerFile = null, [CallerMemberName] string callerMember = null, [CallerLineNumber] int callerLine = 0);

        void Info(object message, string title = null, [CallerFilePath] string callerFile = null, [CallerMemberName] string callerMember = null, [CallerLineNumber] int callerLine = 0);

        void Info(Exception exception, [CallerFilePath] string callerFile = null, [CallerMemberName] string callerMember = null, [CallerLineNumber] int callerLine = 0);

        void Warn(object message, string title = null, [CallerFilePath] string callerFile = null, [CallerMemberName] string callerMember = null, [CallerLineNumber] int callerLine = 0);

        void Warn(Exception exception, [CallerFilePath] string callerFile = null, [CallerMemberName] string callerMember = null, [CallerLineNumber] int callerLine = 0);

        void Error(object message, string title = null, [CallerFilePath] string callerFile = null, [CallerMemberName] string callerMember = null, [CallerLineNumber] int callerLine = 0);

        void Error(Exception exception, [CallerFilePath] string callerFile = null, [CallerMemberName] string callerMember = null, [CallerLineNumber] int callerLine = 0);

        void Fatal(object message, string title = null, [CallerFilePath] string callerFile = null, [CallerMemberName] string callerMember = null, [CallerLineNumber] int callerLine = 0);

        void Fatal(Exception exception, [CallerFilePath] string callerFile = null, [CallerMemberName] string callerMember = null, [CallerLineNumber] int callerLine = 0);
    }
}