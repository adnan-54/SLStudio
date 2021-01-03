using SLStudio.Language.Rdf.Attributes;
using SLStudio.Language.Rdf.Resources;
using System;

namespace SLStudio.Language.Rdf
{
    [RdfDefinition(null, "name_EmptyLine", typeof(CommonResources))]
    public class EmptyLine : RdfDefinitionBase
    {
        public override string ToString()
        {
            return Environment.NewLine;
        }

        protected override RdfDescription GetDescription()
        {
            return new RdfDescription();
        }
    }
}