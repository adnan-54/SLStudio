using System;

namespace SLStudio.Core
{
    internal class FileDescription : IFileDescription
    {
        public FileDescription(Type editorType, FileEditorAttribute description)
        {
            EditorType = editorType;

            DisplayName = ResourceHelpers.ResolveResouce(description.NameKey, description.Resource);
            Description = ResourceHelpers.ResolveResouce(description.DescriptionKey, description.Resource);
            IconSource = description.IconSource;
            Extension = description.Extension;
            Filter = $"{DisplayName}|*{Extension}";
        }

        public Type EditorType { get; }

        public string DisplayName { get; }

        public string Description { get; }

        public string IconSource { get; }

        public string Extension { get; }

        public string Filter { get; }
    }
}