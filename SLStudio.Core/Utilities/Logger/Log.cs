using System;

namespace SLStudio.Core.Utilities.Logger
{
    public class Log
    {
        public Log(string id, string sender, string level, string title, string message, DateTime date)
        {
            Id = id;
            Sender = sender;
            Level = level;
            Title = title;
            Message = message;
            Date = date;
        }

        public string Id { get; }
        public string Sender { get; }
        public string Level { get; }
        public string Title { get; }
        public string Message { get; }
        public DateTime Date { get; }

        public override string ToString()
        {
            var title = string.Empty;
            if (!string.IsNullOrEmpty(Title))
                title = $"\"{Title}\", ";
            return $"({Date}) <{Sender}>: [{Level.ToUpper()}] - {title}{Message}";
        }
    }
}