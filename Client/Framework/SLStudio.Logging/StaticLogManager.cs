using System;

namespace SLStudio.Logging
{
    public partial class LogManager
    {
        static LogManager()
        {
            Default = new LogManager();
        }

        public static ILogManager Default { get; }

        public static ILogger GetLogger<Type>() where Type : class
        {
            return GetLogger(typeof(Type));
        }

        public static ILogger GetLogger(Type type)
        {
            return GetLogger(type.FullName);
        }

        public static ILogger GetLogger(string name)
        {
            return Default.GetLogger(name);
        }

        public static void RequestDump()
        {
            (Default as LogManager).OnDumpRequested();
        }
    }
}