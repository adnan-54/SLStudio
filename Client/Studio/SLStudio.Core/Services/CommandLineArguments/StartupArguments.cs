using CommandLine;
using System.Collections.Generic;

namespace SLStudio.Core
{
    internal class StartupArguments
    {
        public StartupArguments()
        {
            Files = new List<string>();
        }

        [Value(0, Required = false)]
        public IEnumerable<string> Files { get; set; }

        [Option("debug", Required = false)]
        public bool Debug { get; set; }

        [Option("language", Required = false)]
        public string Language { get; set; }
    }
}