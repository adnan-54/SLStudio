using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SLStudio.Core
{
    internal class DefaultLanguageDefinitionManager : ILanguageDefinitionManager
    {
        private readonly IAssemblyLookup assemblyLookup;
        private readonly IObjectFactory objectFactory;

        private IReadOnlyCollection<ILanguageDefinition> languageDefinitionsCache;

        public DefaultLanguageDefinitionManager(IAssemblyLookup assemblyLookup, IObjectFactory objectFactory)
        {
            this.assemblyLookup = assemblyLookup;
            this.objectFactory = objectFactory;
        }

        public IEnumerable<ILanguageDefinition> LanguageDefinitions => Lookup();

        public ILanguageDefinition GetByExtension(string extension)
        {
            return LanguageDefinitions.FirstOrDefault(d => d.Extension.Equals(extension, StringComparison.OrdinalIgnoreCase));
        }

        public ILanguageDefinition GetByName(string name)
        {
            return LanguageDefinitions.FirstOrDefault(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        private IEnumerable<ILanguageDefinition> Lookup()
        {
            if (languageDefinitionsCache == null)
                CreateCache();

            return languageDefinitionsCache;
        }

        private void CreateCache()
        {
            var definitions = assemblyLookup.LoadedAssemblies
                                            .SelectMany(assembly => assembly.DefinedTypes)
                                            .Where(type => type.IsSubclassOf(typeof(LanguageDefinition)))
                                            .Where(type => type.GetCustomAttribute<LanguageDefinitionAttribute>() != null)
                                            .Select(type => objectFactory.Create(type) as ILanguageDefinition);

            languageDefinitionsCache = new List<ILanguageDefinition>(definitions);
        }
    }

    public interface ILanguageDefinitionManager
    {
        IEnumerable<ILanguageDefinition> LanguageDefinitions { get; }

        ILanguageDefinition GetByExtension(string extension);

        ILanguageDefinition GetByName(string name);
    }
}