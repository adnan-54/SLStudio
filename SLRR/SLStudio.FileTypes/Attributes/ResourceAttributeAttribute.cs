using System;

namespace SLStudio.FileTypes.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ResourceAttributeAttribute : Attribute
    {
        public ResourceAttributeAttribute(string attributeName, int index)
        {
            AttributeName = attributeName;
            Index = index;
        }

        public string AttributeName { get; }
        public int Index { get; }
    }
}