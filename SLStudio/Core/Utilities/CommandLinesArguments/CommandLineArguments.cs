using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.Core.Utilities.CommandLinesArguments
{
    internal class CommandLineArguments : ICommandLineArguments
    {
        private bool alreadyParsed = false;

        public CommandLineArguments()
        {
            DebuggingMode = false;
        }

        public IEnumerable<string> Args { get; private set; }

        public bool DebuggingMode { get; private set; }

        public void ParseArguments(IEnumerable<string> args)
        {
            if (alreadyParsed)
                throw new InvalidOperationException();

            if (!args.Any())
                return;

            if (args.FirstOrDefault(arg => arg.Equals("--debugging", StringComparison.InvariantCultureIgnoreCase)).Any())
                DebuggingMode = true;

            Args = args;
            alreadyParsed = true;
        }
    }
}
