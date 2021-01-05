using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SLStudio.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FileEditorAttribute : Attribute
    {
        public FileEditorAttribute(string nameKey, string descriptionKey, string categoryKey, Type resource, string iconSource = null, bool readOnly = false, params string[] extensions)
        {
            NameKey = nameKey;
            DescriptionKey = descriptionKey;
            CategoryKey = categoryKey;
            Resource = resource;
            IconSource = iconSource;
            ReadOnly = readOnly;
            Extensions = extensions.Select(s => $".{Regex.Replace(s, @".*\.", "")}").ToArray();
        }

        public string NameKey { get; }

        public string DescriptionKey { get; }

        public string CategoryKey { get; }

        public Type Resource { get; }

        public string IconSource { get; }

        public bool ReadOnly { get; }

        public string[] Extensions { get; }
    }
}