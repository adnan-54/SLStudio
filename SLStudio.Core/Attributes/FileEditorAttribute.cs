using System;

namespace SLStudio.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FileEditorAttribute : Attribute
    {
        public FileEditorAttribute(string extension, string nameKey, string descriptionKey, string categoryKey, Type resource, string iconSource = null, bool readOnly = false)
        {
            Extension = extension;
            NameKey = nameKey;
            DescriptionKey = descriptionKey;
            CategoryKey = categoryKey;
            Resource = resource;
            IconSource = iconSource;
            ReadOnly = readOnly;
        }

        public string Extension { get; }

        public string NameKey { get; }

        public string DescriptionKey { get; }

        public string CategoryKey { get; }

        public Type Resource { get; }

        public string IconSource { get; }

        public bool ReadOnly { get; }
    }
}