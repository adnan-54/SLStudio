﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Logging
{
    internal class DefaultInternalLogger : IInternalLogger
    {
        private readonly FileStream logStream;
        private readonly StreamReader logReader;
        private readonly StreamWriter logWriter;
        private readonly FileInfo logFile;

        public DefaultInternalLogger()
        {
            var filePath = StudioConstants.InternalLogsFile;
            logStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            logReader = new StreamReader(logStream);
            logWriter = new StreamWriter(logStream);
            logFile = new FileInfo(filePath);
        }

        public async void Log(object message, Log log, string callerFile, string callerMember, int callerLine)
        {
            var sb = new StringBuilder();
            try
            {
                sb.AppendLine("---------------------------");
                sb.AppendLine($"Date: {DateTime.Now}");
                sb.AppendLine();

                if (!string.IsNullOrEmpty(callerFile))
                {
                    sb.AppendLine($"File: {callerFile}");
                    sb.AppendLine();
                }

                if (!string.IsNullOrEmpty(callerMember))
                {
                    sb.AppendLine($"Caller: {callerMember}");
                    sb.AppendLine();
                }

                if (callerLine > 0)
                {
                    sb.AppendLine($"Line: {callerLine}");
                    sb.AppendLine();
                }

                if (message is not null)
                {
                    if (message is Exception exception)
                    {
                        sb.AppendLine($"Message: {exception.Message}");
                        sb.AppendLine();
                        sb.AppendLine($"Exception: {exception}");
                    }
                    else
                    if (message is string messageString && !string.IsNullOrEmpty(messageString))
                    {
                        sb.AppendLine($"Message: {messageString}");
                        sb.AppendLine();
                    }
                    else
                    {
                        sb.AppendLine($"Message: {message}");
                        sb.AppendLine();
                    }
                }

                if (log is not null)
                {
                    sb.AppendLine($"Log: {log}");
                    sb.AppendLine();
                }

                logStream.Seek(0, SeekOrigin.End);
                await logWriter.WriteAsync($"{sb}").ConfigureAwait(false);
                await logWriter.FlushAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"An error ocurred while trying to write in the internal log file: {sb}{Environment.NewLine}{ex}");
            }
        }

        public Task<string> GetLogs()
        {
            logStream.Seek(0, SeekOrigin.Begin);
            return logReader.ReadToEndAsync();
        }

        public long GetSize()
        {
            logFile.Refresh();
            return logFile.Length;
        }
    }

    internal interface IInternalLogger
    {
        void Log(object message, Log log = null, [CallerFilePath] string callerFile = null, [CallerMemberName] string callerMember = null, [CallerLineNumber] int callerLine = 0);

        Task<string> GetLogs();

        long GetSize();
    }
}