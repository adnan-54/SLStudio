using SLStudio.Logging;
using System;
using System.Globalization;

namespace SLStudio.Core
{
    internal class RecentFile : IRecentFile
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<RecentFile>();

        public RecentFile(string fileName, DateTime creationDate)
        {
            FileName = fileName;
            CreationDate = creationDate;
        }

        public string FileName { get; }

        public DateTime CreationDate { get; }

        public override string ToString()
        {
            return $"{FileName};{CreationDate.ToString(CultureInfo.InvariantCulture)}";
        }

        public static bool TryParse(string entry, out RecentFile parsed)
        {
            try
            {
                var splitted = entry.Split(';');
                var fileName = splitted[0];
                var creationDate = DateTime.Parse(splitted[1], CultureInfo.InvariantCulture);
                parsed = new RecentFile(fileName, creationDate);
                return true;
            }
            catch (Exception ex)
            {
                logger.Warn(ex);
                parsed = null;
                return false;
            }
        }
    }

    public interface IRecentFile
    {
        string FileName { get; }

        DateTime CreationDate { get; }
    }
}