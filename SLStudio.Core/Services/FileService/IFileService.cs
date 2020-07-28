using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SLStudio.Core
{
    internal class DefaultFileService : IFileService
    {
        private readonly IShell shell;
        private readonly IObjectFactory objectFactory;
        private readonly IRecentFilesRepository recentFilesRepository;
        private readonly Dictionary<string, IFileDescription> filesDescription;

        public DefaultFileService(IShell shell, IObjectFactory objectFactory, IRecentFilesRepository recentFilesRepository)
        {
            this.shell = shell;
            this.objectFactory = objectFactory;
            this.recentFilesRepository = recentFilesRepository;

            filesDescription = new Dictionary<string, IFileDescription>();
            CreateFilesDescriptionDictionary();
        }

        public bool CanEdit(string extension) => filesDescription.ContainsKey(Path.GetExtension(extension));

        public IEnumerable<IFileDescription> GetFileDescriptions()
        {
            return filesDescription.Select(kvp => kvp.Value).Distinct();
        }

        private void CreateFilesDescriptionDictionary()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.DefinedTypes).Where(a => a.FullName.Contains("SLStudio", StringComparison.OrdinalIgnoreCase));

            foreach (var type in types)
            {
                var attr = GetFileEditorAttribute(type);
                if (attr != null)
                {
                    var description = new FileDescription(type.AsType(), attr);
                    filesDescription.Add(attr.Extension, description);
                }
            }
        }

        private FileEditorAttribute GetFileEditorAttribute(Type fileType)
        {
            return fileType.GetCustomAttributes(typeof(FileEditorAttribute), false).Cast<FileEditorAttribute>().FirstOrDefault();
        }
    }

    public interface IFileService
    {
        bool CanEdit(string extension);

        IEnumerable<IFileDescription> GetFileDescriptions();
    }
}