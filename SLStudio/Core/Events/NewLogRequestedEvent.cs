using System;

namespace SLStudio.Core.Events
{
    internal class NewLogRequestedEvent
    {
        public NewLogRequestedEvent(string sender, string level, string title, string description, DateTime date)
        {
            Sender = sender;
            Level = level;
            Title = title;
            Description = description;
            Date = date;
        }

        public string Sender { get; }
        public string Level { get; }
        public string Title { get; }
        public string Description { get; }
        public DateTime Date { get; }
    }
}
