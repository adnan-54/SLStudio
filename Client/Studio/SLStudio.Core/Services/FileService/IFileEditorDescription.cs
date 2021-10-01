using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLStudio.Core
{
    internal class FileEditorDescription : IFileEditorDescription
    {
        public FileEditorDescription(Type editorType, FileEditorAttribute description)
        {
            EditorType = editorType;
            Name = ResourceHelpers.ResolveResouce(description.NameKey, description.Resource);
            Description = ResourceHelpers.ResolveResouce(description.DescriptionKey, description.Resource);
            Category = ResourceHelpers.ResolveResouce(description.CategoryKey, description.Resource);
            IconSource = description.IconSource;
            ReadOnly = description.ReadOnly;
            Extensions = description.Extensions;
            Filter = BuildFilter();

            if (Category.Contains('\\'))
                throw new ArgumentException($"'\\' is not valid for category path");
            if (!Category.EndsWith('/'))
                throw new ArgumentException($"category path must end with '/'");
        }

        public Type EditorType { get; }

        public string Name { get; }

        public string Description { get; }

        public string Category { get; }

        public string IconSource { get; }

        public bool ReadOnly { get; }

        public IEnumerable<string> Extensions { get; }

        public string Filter { get; }

        private string BuildFilter()
        {
            var builder = new StringBuilder($"{Name} (*{string.Join(", *", Extensions)})|");
            foreach (var ext in Extensions)
                builder.AppendFormat("*{0};", ext);

            if (Extensions.Count() == 1)
                builder.Remove(builder.Length - 1, 1);

            return builder.ToString();
        }
    }

    public interface IFileEditorDescription
    {
        Type EditorType { get; }

        string Name { get; }

        string Description { get; }

        string Category { get; }

        string IconSource { get; }

        bool ReadOnly { get; }

        IEnumerable<string> Extensions { get; }

        string Filter { get; }
    }
}