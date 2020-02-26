using System.Collections.Generic;

namespace SLStudio.Core
{
    public interface ICommandLineArguments
    {
        IEnumerable<string> Args { get; }

        bool DebuggingMode { get; }

        void ParseArguments(IEnumerable<string> args);
    }
}
