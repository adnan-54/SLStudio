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

        async Task<IFileDocumentPanel> IFileService.Empty(string extension)
        {
            var description = GetFileDescription(extension);
            var file = await OpenFileWorkspace(description.EditorType);
            file.Activate();

            return file;
        }

        async Task<IFileDocumentPanel> IFileService.New(string extension, string displayName, string content)
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

        private FileEditorAttribute GetFileEditorAttribute(Type fileType)
        {
            return fileType.GetCustomAttributes(typeof(FileEditorAttribute), false).Cast<FileEditorAttribute>().FirstOrDefault();
        }

        private IFileDescription GetFileDescription(string extension)
        {
            if (!filesDescription.TryGetValue(extension, out IFileDescription description))
                throw new InvalidOperationException($"the type {description.EditorType} is not registred");
            return description;
        }

        private async Task<IFileDocumentPanel> OpenFileWorkspace(Type editor)
        {
            var file = objectFactory.Create(editor) as IFileDocumentPanel;
            await shell.OpenPanel(file);
            return file;
        }

        private async Task<IFileDocumentPanel> CreateNew(string name, string content, Type fileType)
        {
            var file = await OpenFileWorkspace(fileType);
            file.Activate();

            var displayName = string.IsNullOrEmpty(name) ? $"{StudioResources.Untitled}{createdFiles++}" : name;
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

    public interface IFileService
    {
        bool CanEdit(string extension);

        IEnumerable<IFileDescription> GetFileDescriptions();

        Task<IFileDocumentPanel> Empty(string extension);

        Task<IFileDocumentPanel> New(string extension, string displayName = null, string content = null);

        Task<T> New<T>(string displayName = null, string content = null) where T : class, IFileDocumentPanel;

        string GetFilter(string extension);

        //Task<IFileDocumentPanel> Open(string fileName);

        //Task<T> Open<T>(string fileName) where T : class, IFileDocumentPanel;

        //Task<bool> Save(IFileDocumentPanel file);

        //Task<bool> SaveAs(IFileDocumentPanel file);
    }
}