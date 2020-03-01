using System;

namespace SLStudio.Core.Events
{
    public class NewLogRequestedEvent
    {
        public NewLogRequestedEvent(string sender, string level, string title, string message, DateTime date)
        {
            Sender = sender;
            Level = level;
            Title = title;
            Message = message;
            Date = date;
        }

        public string Sender { get; }
        public string Level { get; }
        public string Title { get; }
        public string Message { get; }
        public DateTime Date { get; }

        public override string ToString()
        {
            return $"({Date}) {Sender}: [{Level.ToUpper()}] \"{Title}\", {Message}";
        }
    }
}