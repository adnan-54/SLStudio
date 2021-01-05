using SLStudio.Core.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    internal class DefaultFileService : IFileService
    {
        private readonly IShell shell;
        private readonly IObjectFactory objectFactory;
        private readonly IRecentFilesRepository recentFilesRepository;
        private readonly Dictionary<string, IFileDescription> filesDescription;
        private int createdFiles = 0;

        public DefaultFileService(IShell shell, IObjectFactory objectFactory, IRecentFilesRepository recentFilesRepository)
        {
            this.shell = shell;
            this.objectFactory = objectFactory;
            this.recentFilesRepository = recentFilesRepository;

            filesDescription = new Dictionary<string, IFileDescription>();
            CreateFilesDescriptionDictionary();
        }

        bool IFileService.CanEdit(string extension) => filesDescription.ContainsKey(Path.GetExtension(extension));

        IEnumerable<IFileDescription> IFileService.GetFileDescriptions()
        {
            return filesDescription.Select(kvp => kvp.Value).Distinct();
        }

        async Task<IFileDocumentItem> IFileService.Empty(string extension)
        {
            var description = GetFileDescription(extension);
            var file = await OpenFileWorkspace(description.EditorType);
            file.Activate();

            return file;
        }

        async Task<IFileDocumentItem> IFileService.New(string extension, string displayName, string content)
        {
            var description = GetFileDescription(extension);
            return await CreateNew(displayName, content, description.EditorType);
        }

        async Task<T> IFileService.New<T>(string displayName, string content)
        {
            return (T)await CreateNew(displayName, content, typeof(T));
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

        private static FileEditorAttribute GetFileEditorAttribute(Type fileType)
        {
            return fileType.GetCustomAttributes(typeof(FileEditorAttribute), false).Cast<FileEditorAttribute>().FirstOrDefault();
        }

        private IFileDescription GetFileDescription(string extension)
        {
            if (!filesDescription.TryGetValue(extension, out IFileDescription description))
                throw new InvalidOperationException($"The type {extension} is not registred");
            return description;
        }

        private async Task<IFileDocumentItem> OpenFileWorkspace(Type editor)
        {
            var file = objectFactory.Create(editor) as IFileDocumentItem;
            await shell.OpenWorkspaces(file);
            return file;
        }

        private async Task<IFileDocumentItem> CreateNew(string name, string content, Type fileType)
        {
            var file = await OpenFileWorkspace(fileType);
            file.Activate();

            var displayName = string.IsNullOrEmpty(name) ? string.Format(StudioResources.Untitled, ++createdFiles) : name;
            var ext = Path.GetExtension(displayName);
            if (string.IsNullOrEmpty(ext))
            {
                ext = GetFileEditorAttribute(file.GetType())?.Extension;
                displayName = $"{displayName}{ext}";
            }
            await file.New(displayName, content);

            return file;
        }

        public string GetFilter(string extension)
        {
            if (!filesDescription.TryGetValue(extension, out var value))
                return string.Empty;

            return value.Filter;
        }
    }
}