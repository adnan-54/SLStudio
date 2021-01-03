using SLStudio.Language.Rdf.Attributes;
using SLStudio.Language.Rdf.Resources;

namespace SLStudio.Language.Rdf
{
    [RdfDefinition(null, "name_Comment", typeof(CommonResources))]
    public class CommentLine : RdfDefinitionBase
    {
        public string Comment { get; set; }

        public override string ToString()
        {
            return Comment;
        }

        protected override RdfDescription GetDescription()
        {
            return new RdfDescription();
        }
    }
}