using System;

namespace SLStudio.Core.Events
{
    public class NewLogRequestedEvent
    {
        public NewLogRequestedEvent(string sender, string level, string title, string description, DateTime date)
        {
            Sender = sender;
            Level = level;
            Title = title;
            Message = description;
            Date = date;
        }

        public string Sender { get; }
        public string Level { get; }
        public string Title { get; }
        public string Message { get; }
        public DateTime Date { get; }

        public override string ToString()
        {
            return $"{Sender} ({Date}): [{Level.ToUpper()}] \"{Title}\", {Message}";
        }
    }
}
