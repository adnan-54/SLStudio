using System;

namespace SLStudio.Language.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ResourceAttributeAttribute : Attribute
    {
        public ResourceAttributeAttribute(string attributeName)
        {
            AttributeName = attributeName;
        }

        public string AttributeName { get; }
    }
}