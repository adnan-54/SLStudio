using System.Text;

namespace SLStudio.Logging
{
    public record Log(string Title, string Message, string Sender, string Level, string Date, string CallerFile, string CallerMember, int CallerLine, string StackTrace, int Id)
    {
        private string stringRepresentation;

        public string StringRepresentation => stringRepresentation ??= CreateStringRepresentation();

        private string CreateStringRepresentation()
        {
            if (string.IsNullOrEmpty(stringRepresentation))
            {
                var sb = new StringBuilder();
                sb.Append($"({Date}) | <{Sender}>: [{Level}] ");

                if (!string.IsNullOrEmpty(Title))
                {
                    sb.Append($"\"{Title}\"");
                    sb.AppendLine();
                }

                sb.Append($"\"{Message}\"");

                if (!string.IsNullOrEmpty(StackTrace))
                    sb.AppendLine($"@{StackTrace}");

                stringRepresentation = $"{sb}";
            }

            return stringRepresentation;
        }
    }
}