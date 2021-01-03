using System;
using System.Resources;

namespace SLStudio.Language.Rdf.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class RdfDefinitionAttribute : Attribute
    {
        public RdfDefinitionAttribute(string verb, string name, Type resourceType)
        {
            Verb = verb;
            Name = new ResourceManager(resourceType).GetString(name);
        }

        public string Verb { get; }

        public string Name { get; }
    }
}