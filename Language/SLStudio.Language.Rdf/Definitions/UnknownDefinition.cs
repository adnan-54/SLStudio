using SLStudio.Language.Rdf.Attributes;
using SLStudio.Language.Rdf.Resources;

namespace SLStudio.Language.Rdf
{
    [RdfDefinition(null, "name_UnknownDefinition", typeof(CommonResources))]
    public class UnknownDefinition : RdfDefinitionBase
    {
        public UnknownDefinition(string content)
        {
            Content = content;
        }

        public string Content { get; set; }

        public override string ToString()
        {
            return Content;
        }

        protected override RdfDescription GetDescription()
        {
            return new RdfDescription();
        }
    }
}