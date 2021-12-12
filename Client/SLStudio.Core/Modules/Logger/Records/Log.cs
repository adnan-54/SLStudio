namespace SLStudio.Logger;

public record Log(Guid Id, string Sender, string Message, LogLevel Level, DateTime Date, string StackTrace);
