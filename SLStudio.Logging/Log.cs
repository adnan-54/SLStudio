using System;

namespace SLStudio.Logging
{
    public class Log
    {
        internal Log()
        {
        }

        internal Log(string title, string message, string sender, string level, DateTime date)
        {
            Title = title;
            Message = message;
            Sender = sender;
            Level = level;
            Date = date;
        }

        public string Title { get; }
        public string Message { get; }
        public string Sender { get; }
        public string Level { get; }
        public DateTime Date { get; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Title))
                return $"({Date}) <{Sender}>: [{Level}] \"{Message}\"";
            return $"({Date}) <{Sender}>: [{Level}] \"{Title}\", \"{Message}\"";
        }
    }
}