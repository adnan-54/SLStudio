using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SLStudio.Core
{
    internal class DefaultLanguageDefinitionManager : ILanguageDefinitionManager
    {
        private readonly IAssemblyLookup assemblyLookup;
        private List<ILanguageDefinition> languageDefinitionsCache;

        public DefaultLanguageDefinitionManager(IAssemblyLookup assemblyLookup)
        {
            this.assemblyLookup = assemblyLookup;
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
            {
                var definitions = assemblyLookup.LoadedAssemblies
                                                .SelectMany(assembly => assembly.DefinedTypes)
                                                .Where(type => type.GetCustomAttribute<LanguageDefinitionAttribute>() != null)
                                                .Where(type => type.IsSubclassOf(typeof(LanguageDefinition)))
                                                .Select(type => (ILanguageDefinition)Activator.CreateInstance(type));

                languageDefinitionsCache = new List<ILanguageDefinition>(definitions);
            }

            return languageDefinitionsCache;
        }
    }

    public interface ILanguageDefinitionManager
    {
        IEnumerable<ILanguageDefinition> LanguageDefinitions { get; }

        ILanguageDefinition GetByExtension(string extension);

        ILanguageDefinition GetByName(string name);
    }
}