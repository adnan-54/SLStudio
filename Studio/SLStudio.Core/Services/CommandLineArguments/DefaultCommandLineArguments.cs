using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.Core
{
    internal class DefaultCommandLineArguments : ICommandLineArguments
    {
        internal DefaultCommandLineArguments(IEnumerable<string> args)
        {
            Args = args;
        }

        public IEnumerable<string> Args { get; }

        public bool DebugMode => Args.Any(arg => arg.Equals("--debug", StringComparison.InvariantCultureIgnoreCase));
    }
}