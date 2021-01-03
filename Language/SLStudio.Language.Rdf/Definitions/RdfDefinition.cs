using SLStudio.Language.Rdf.Attributes;
using SLStudio.Language.Rdf.Resources;
using System.Collections.Generic;
using System.Text;

namespace SLStudio.Language.Rdf
{
    [RdfDefinition(Verb, "name_RdfDefinition", typeof(CommonResources))]
    public class RdfDefinition : RdfDefinitionBase
    {
        private const string Verb = "rdfDefinition";

        public uint TypeId { get; set; }

        public uint SuperId { get; set; }

        public uint AdditionalType { get; set; }

        public string Alias { get; set; }

        public bool IsParentCompatible { get; set; }

        public uint TypeOfEntry { get; set; }

        public override string ToString()
        {
            var parts = new List<string>();

            if (TypeId > 0)
                parts.Add($"{nameof(TypeId)}={TypeId}");

            if (SuperId > 0)
                parts.Add($"{nameof(SuperId)}={SuperId}");

            if (AdditionalType > 0)
                parts.Add($"{nameof(SuperId)}={SuperId}");

            if (!string.IsNullOrEmpty(Alias))
                parts.Add($"{nameof(Alias)}={Alias}");

            if (IsParentCompatible)
                parts.Add($"{nameof(IsParentCompatible)}");

            if (TypeOfEntry > 0)
                parts.Add($"{nameof(TypeOfEntry)}={TypeOfEntry}");

            return $"{Verb} {string.Join(", ", parts)};";
        }

        protected override RdfDescription GetDescription()
        {
            return new RdfDescription("description_RdfDefinition", typeof(CommonResources), this);
        }
    }
}