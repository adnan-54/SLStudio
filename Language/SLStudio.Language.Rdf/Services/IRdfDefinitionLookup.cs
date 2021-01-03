using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.Language.Rdf
{
    internal class DefaultRdfDefinitionLookup : IRdfDefinitionLookup
    {
        private IEnumerable<RdfAttributes> definitions;

        public IEnumerable<RdfAttributes> Lookup()
        {
            if (definitions == null)
            {
                definitions = new List<RdfAttributes>()
                {
                    new RdfAttributes(typeof(CommentLine)),
                    new RdfAttributes(typeof(EmptyLine)),
                    new RdfAttributes(typeof(RdfDefinition))
                };
            }

            return definitions;
        }

        public RdfAttributes Lookup(string verb)
        {
            if (definitions == null)
                Lookup();

            return definitions.FirstOrDefault(d => d.Verb.Equals(verb, StringComparison.OrdinalIgnoreCase));
        }
    }

    public interface IRdfDefinitionLookup
    {
        IEnumerable<RdfAttributes> Lookup();

        RdfAttributes Lookup(string verb);
    }
}