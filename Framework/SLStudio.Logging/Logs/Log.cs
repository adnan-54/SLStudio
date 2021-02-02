using System;
using System.Text;

namespace SLStudio.Logging
{
    public record Log(string Title, string Message, string Sender, string Level, DateTime Date, string CallerFile, string CallerMember, int CallerLine, string StackTrace)
    {
        private string stringRepresentation;

        public string StringRepresentation => stringRepresentation ??= CreateStringRepresentation();

        private string CreateStringRepresentation()
        {
            if (string.IsNullOrEmpty(stringRepresentation))
            {
                var sb = new StringBuilder();
                sb.Append($"({Date}) | [{Level}] <{Sender}>: ");

                if (!string.IsNullOrEmpty(Title))
                {
                    sb.Append($"\"{Title}\"");
                    sb.AppendLine();
                }

                sb.Append($"\"{Message}\"");
                sb.AppendLine();
                sb.Append($"{CallerFile}, {CallerMember}, {CallerLine}");
                sb.Append($"@{StackTrace}");

                stringRepresentation = $"{sb}";
            }

            return stringRepresentation;
        }
    }
}