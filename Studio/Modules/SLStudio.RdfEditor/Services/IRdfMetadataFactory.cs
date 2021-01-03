using SLStudio.Language.Rdf;
using System;

namespace SLStudio.RdfEditor
{
    internal class DefaultRdfMetadataFactory : IRdfMetadataFactory
    {
        public RdfMetadata FromAttributes(RdfAttributes attributes)
        {
            var definition = Activator.CreateInstance(attributes.Type) as RdfDefinitionBase;
            return new RdfMetadata(attributes, definition);
        }
    }

    public interface IRdfMetadataFactory
    {
        RdfMetadata FromAttributes(RdfAttributes attributes);
    }
}