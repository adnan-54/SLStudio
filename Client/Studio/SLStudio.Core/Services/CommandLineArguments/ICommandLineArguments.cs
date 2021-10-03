using CommandLine;
using SLStudio.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.Core
{
    internal class DefaultCommandLineArguments : ICommandLineArguments
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<DefaultCommandLineArguments>();

        private readonly Parser parser;
        private readonly IEnumerable<string> args;
        private readonly StartupArguments startupArguments;

        internal DefaultCommandLineArguments()
        {
            args = Environment.GetCommandLineArgs().Skip(1);
            parser = new Parser(SetupParser);
            startupArguments = ParseArgs();
        }

        public IEnumerable<string> Files => startupArguments.Files;

        public bool Debug => startupArguments.Debug;

        public string Language => startupArguments.Language;

        private void SetupParser(ParserSettings settings)
        {
            settings.IgnoreUnknownArguments = true;
        }

        private StartupArguments ParseArgs()
        {
            return parser.ParseArguments<StartupArguments>(args).MapResult(options => options, errors =>
            {
                logger.Error(string.Join('|', args), "Failed to parse startup arguments");
                return new StartupArguments();
            });
        }
    }

    public interface ICommandLineArguments
    {
        public IEnumerable<string> Files { get; }

        bool Debug { get; }

        string Language { get; }
    }
}