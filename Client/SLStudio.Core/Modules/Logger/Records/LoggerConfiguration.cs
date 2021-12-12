namespace SLStudio.Logger;

public record LoggerConfiguration(bool WriteToOutput, bool WriteToConsole, bool LogTrace, LogLevel DefaultLogLevel, LogLevel MinimumLogLevel);
