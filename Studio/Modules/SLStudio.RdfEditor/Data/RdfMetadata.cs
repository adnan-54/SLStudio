using DevExpress.Mvvm;
using SLStudio.Language.Rdf;
using System;
using System.Collections.Generic;

namespace SLStudio.RdfEditor
{
    public class RdfMetadata : BindableBase
    {
        public RdfMetadata(RdfAttributes attributes, RdfDefinitionBase definition)
        {
            Attributes = attributes;
            Definition = definition;
        }

        public RdfAttributes Attributes { get; }

        public RdfDefinitionBase Definition { get; }

        public bool IsEmptyLine => Definition is EmptyLine;

        public bool IsComment => Definition is CommentLine;

        public bool IsUnknown => Definition is UnknownDefinition;

        public Uri Icon => Attributes.Icon;

        public string Name => Attributes.Name;

        public IEnumerable<DescriptionToken> Description => Definition.Description.GetDescription();
    }
}