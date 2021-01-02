namespace SLStudio.Core.Modules.NewFile.Models
{
    internal class NewFileModel
    {
        public NewFileModel(IFileDescription fileDescription)
        {
            FileDescription = fileDescription;
        }

        public IFileDescription FileDescription { get; }

        public string DisplayName => FileDescription.Name;

        public string Type => FileDescription.Name;

        public string Description => FileDescription.Description;

        public string Category => FileDescription.Category;

        public string IconSource => FileDescription.IconSource;
    }
}