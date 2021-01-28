namespace SLStudio.Core.Modules.NewFile.Models
{
    internal class NewFileModel
    {
        public NewFileModel(IFileEditorDescription fileDescription)
        {
            FileDescription = fileDescription;
        }

        public IFileEditorDescription FileDescription { get; }

        public string DisplayName => FileDescription.Name;

        public string Type => FileDescription.Category.Substring(0, FileDescription.Category.IndexOf("/"));

        public string Description => FileDescription.Description;

        public string Category => FileDescription.Category;

        public string IconSource => FileDescription.IconSource;
    }
}