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
            DebuggingMode = false;
        }

        public IEnumerable<string> Args { get; private set; }

        public bool DebuggingMode { get; private set; }

        public void ParseArguments(IEnumerable<string> args)
        {
            if (alreadyParsed || !args.Any())
                return;

            alreadyParsed = true;
            Args = args;

            if (args.FirstOrDefault(arg => arg.Equals("--debugging", StringComparison.InvariantCultureIgnoreCase)).Any())
                DebuggingMode = true;
        }
    }
}