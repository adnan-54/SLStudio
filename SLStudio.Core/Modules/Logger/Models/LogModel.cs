namespace SLStudio.Core.Modules.Logger
{
    class LogModel
    {
        public LogModel(string id, string sender, string level, string title, string message, string date)
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
        public string Date { get; }
    }
}
