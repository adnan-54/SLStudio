using System;
using System.Collections.Generic;
using System.Reflection;
using System.Resources;
using System.Text;

namespace SLStudio.Language.Rdf
{
    public class RdfDescription
    {
        private readonly string key;
        private readonly ResourceManager resourceManager;
        private readonly IRdfDefinition definition;

        private IEnumerable<DescriptionToken> tokens;

        public RdfDescription()
        {
            tokens = new List<DescriptionToken>() {
                new DescriptionToken(null)
            };
        }

        public RdfDescription(string key, Type resourceType, IRdfDefinition definition)
        {
            this.key = key;
            resourceManager = new ResourceManager(resourceType);
            this.definition = definition;
        }

        public IEnumerable<DescriptionToken> GetDescription()
        {
            if (tokens == null)
                tokens = BuildTokens();

            return tokens;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var token in GetDescription())
                sb.Append(token);

            return sb.ToString();
        }

        private IEnumerable<DescriptionToken> BuildTokens()
        {
            var list = new List<DescriptionToken>();
            var description = resourceManager.GetString(key);
            var parts = description.Split(' ', StringSplitOptions.None);

            var count = 0;
            foreach (var part in parts)
            {
                var separator = count++ < parts.Length - 1 ? " " : "";

                if (part.StartsWith('{') && part.EndsWith('}'))
                {
                    var key = part.TrimStart('{').TrimEnd('}');
                    if (TryGetValue(key, out var value))
                    {
                        list.Add(new DescriptionToken($"{value}{separator}", true));
                        continue;
                    }

                    list.Add(new DescriptionToken($"{part}{separator}", true));
                    continue;
                }

                list.Add(new DescriptionToken($"{part}{separator}"));
            }

            return list;
        }

        private bool TryGetValue(string propertyName, out string value)
        {
            value = definition.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance)?.GetValue(definition).ToString();

            return value != null;
        }
    }
}