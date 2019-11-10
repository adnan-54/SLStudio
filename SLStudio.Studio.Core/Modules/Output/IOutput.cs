using SLStudio.Studio.Core.Framework;
using System.IO;

namespace SLStudio.Studio.Core.Modules.Output
{
    public interface IOutput : ITool
    {
        TextWriter Writer { get; }
        void AppendLine(string text);
        void Append(string text);
        void Clear();
    }
}
