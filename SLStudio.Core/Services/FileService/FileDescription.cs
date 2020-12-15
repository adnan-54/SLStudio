using System;
using System.Linq;

namespace SLStudio.Core
{
    internal class FileDescription : IFileDescription
    {
        public FileDescription(Type editorType, FileEditorAttribute description)
        {
            EditorType = editorType;
            Name = ResourceHelpers.ResolveResouce(description.NameKey, description.Resource);
            Description = ResourceHelpers.ResolveResouce(description.DescriptionKey, description.Resource);
            Category = ResourceHelpers.ResolveResouce(description.CategoryKey, description.Resource);
            IconSource = description.IconSource;
            Extension = description.Extension;
            Filter = $"{Name}|*{Extension}";
            ReadOnly = description.ReadOnly;

            if (!Category.Contains('/'))
                throw new ArgumentException($"{nameof(Category)} is not a valid path");
        }

        public Type EditorType { get; }

        public string Name { get; }

        public string Description { get; }

        public string Category { get; }

        public string IconSource { get; }

        public string Extension { get; }

        public string Filter { get; }

        public bool ReadOnly { get; }
    }
}