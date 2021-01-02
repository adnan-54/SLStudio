using System.Collections.Generic;

namespace SLStudio.Core
{
    public interface ICommandLineArguments
    {
        public IEnumerable<string> Args { get; }

        bool DebugMode { get; }
    }
}