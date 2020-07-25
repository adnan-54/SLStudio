using System;

namespace SLStudio.Core
{
    public class RecentFile
    {
        public RecentFile(string location, DateTime date)
        {
            Location = location;
            Date = date;
        }

        public string Location { get; }

        public DateTime Date { get; }

        public bool Pinned { get; set; }
    }
}