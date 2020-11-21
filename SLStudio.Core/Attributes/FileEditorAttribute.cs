using System;

namespace SLStudio.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FileEditorAttribute : Attribute
    {
        public FileEditorAttribute(string extension, string nameKey, string descriptionKey, Type resource, string iconSource = null, FileEditorCategory fileEditorCategory = FileEditorCategory.GameFile)
        {
            Extension = extension;
            NameKey = nameKey;
            DescriptionKey = descriptionKey;
            Resource = resource;
            IconSource = iconSource;
            FileEditorCategory = fileEditorCategory;
        }

        public string Extension { get; }
        public string NameKey { get; }
        public string DescriptionKey { get; }
        public Type Resource { get; }
        public string IconSource { get; }
        public FileEditorCategory FileEditorCategory { get; }
    }

    public enum FileEditorCategory
    {
        GameFile,
        Advanced
    }
}