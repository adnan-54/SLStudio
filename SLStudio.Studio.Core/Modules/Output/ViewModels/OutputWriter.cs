using System.IO;
using System.Text;

namespace SLStudio.Studio.Core.Modules.Output.ViewModels
{
    internal class OutputWriter : TextWriter
    {
        private readonly IOutput output;

        public OutputWriter(IOutput output)
        {
            this.output = output;
        }

        public override Encoding Encoding => Encoding.Default;

        public override void WriteLine()
        {
            output.AppendLine(string.Empty);
        }

        public override void WriteLine(string value)
        {
            output.AppendLine(value);
        }

        public override void Write(string value)
        {
            output.Append(value);
        }
    }
}
