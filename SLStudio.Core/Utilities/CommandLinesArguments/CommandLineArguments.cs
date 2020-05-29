using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.Core.Utilities.CommandLinesArguments
{
    internal class DefaultCommandLineArguments : ICommandLineArguments
    {
        private bool alreadyParsed = false;

        public DefaultCommandLineArguments()
        {
            DebugMode = false;
        }

        public IEnumerable<string> Args { get; private set; }

        public bool DebugMode { get; private set; }

        public void ParseArguments(IEnumerable<string> args)
        {
            if (alreadyParsed || !args.Any())
                return;

            alreadyParsed = true;
            Args = args;

            if (args.Any(arg => arg.Equals("--debug", StringComparison.InvariantCultureIgnoreCase)))
                DebugMode = true;
        }
    }
}