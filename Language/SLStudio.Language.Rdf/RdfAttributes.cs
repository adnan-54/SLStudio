using Jdenticon;
using SLStudio.Language.Rdf.Attributes;
using System;
using System.IO;
using System.Reflection;

namespace SLStudio.Language.Rdf
{
    public class RdfAttributes
    {
        public RdfAttributes(Type type)
        {
            Type = type;
            DefinitionAttr = type.GetCustomAttribute<RdfDefinitionAttribute>();
        }

        public Type Type { get; }

        public RdfDefinitionAttribute DefinitionAttr { get; }

        public bool IsComment => typeof(CommentLine).IsAssignableFrom(Type);

        public bool IsEmptyLine => typeof(EmptyLine).IsAssignableFrom(Type);

        public bool IsUnknown => typeof(UnknownDefinition).IsAssignableFrom(Type);

        public string Verb => DefinitionAttr.Verb;

        public string Name => DefinitionAttr.Name;

        public Uri Icon => GetIcon();

        private Uri GetIcon()
        {
            var fileName = string.Format("rdf_{0}.png", Type.Name);
            var filePath = Path.Combine(SLStudioConstants.IconsDirectory, fileName);
            if (!File.Exists(filePath))
            {
                if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                var identicon = Identicon.FromValue(Type.Name, 64);
                identicon.Style.BackColor = Jdenticon.Rendering.Color.Transparent;
                identicon.Style.Padding = 0;
                identicon.SaveAsPng(filePath);
            }

            return new Uri(filePath);
        }
    }
}