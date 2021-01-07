using Microsoft.Win32;
using SLStudio.Core.Resources;
using SLStudio.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace SLStudio.Core
{
    internal class DefaultFileService : IFileService
    {
        private static readonly ILogger logger = LogManager.GetLoggerFor<DefaultFileService>();

        private readonly IShell shell;
        private readonly IAssemblyLookup assemblyLookup;
        private readonly IObjectFactory objectFactory;
        private readonly IRecentFilesRepository recentFilesRepository;
        private readonly IStatusBar statusBar;
        private readonly ConcurrentDictionary<string, IFileEditorDescription> extensionTofileDescription;
        private readonly ConcurrentDictionary<Type, IFileEditorDescription> typeToFileDescription;

        public DefaultFileService(IShell shell, IAssemblyLookup assemblyLookup, IObjectFactory objectFactory,
                                  IRecentFilesRepository recentFilesRepository, IStatusBar statusBar)
        {
            this.shell = shell;
            this.assemblyLookup = assemblyLookup;
            this.objectFactory = objectFactory;
            this.recentFilesRepository = recentFilesRepository;
            this.statusBar = statusBar;
            extensionTofileDescription = new ConcurrentDictionary<string, IFileEditorDescription>();
            typeToFileDescription = new ConcurrentDictionary<Type, IFileEditorDescription>();

            CreateFilesDescriptionDictionary();
        }

        public IEnumerable<IFileDocumentItem> OpenedFiles => shell.Workspaces.OfType<IFileDocumentItem>();

        public IEnumerable<IFileEditorDescription> GetDescriptions()
        {
            return typeToFileDescription.Select(kvp => kvp.Value).Distinct();
        }

        public IFileEditorDescription GetDescription(IFileDocumentItem file)
        {
            return GetDescription(file.GetType());
        }

        public IFileEditorDescription GetDescription(Type editorType)
        {
            if (!typeToFileDescription.TryGetValue(editorType, out IFileEditorDescription description))
                description = typeToFileDescription.FirstOrDefault(t => editorType.IsAssignableFrom(t.Key)).Value;

            return description;
        }

        public IFileEditorDescription GetDescription(string extension)
        {
            extensionTofileDescription.TryGetValue(extension, out IFileEditorDescription description);
            return description;
        }

        public bool CanHandle(string fileName)
        {
            try
            {
                return extensionTofileDescription.ContainsKey(Path.GetExtension(fileName));
            }
            catch
            {
                return false;
            }
        }

        Task<IFileDocumentItem> IFileService.Empty(string extension)
        {
            var description = GetDescription(extension);
            if (description != null)
                return AddWorkspace(description.EditorType);

            return null;
        }

        Task<IFileDocumentItem> IFileService.New(string extension, string displayName, string content)
        {
            var description = GetDescription(extension);
            if (description != null)
                return CreateNew(displayName, content, description);

            return null;
        }

        async Task<T> IFileService.New<T>(string displayName, string content)
        {
            return (await CreateNew(displayName, content, GetDescription(typeof(T)))) as T;
        }

        async Task<T> IFileService.Open<T>(string fileName)
        {
            return (await DoLoad(fileName, typeof(T))) as T;
        }

        async Task<IFileDocumentItem> IFileService.Open(string fileName)
        {
            var shellTarget = shell.Workspaces.OfType<IFileDocumentItem>().FirstOrDefault(i => !string.IsNullOrEmpty(i.FileName) && i.FileName.Equals(fileName));
            if (shellTarget != null)
            {
                await shell.OpenWorkspaces(shellTarget);
                return shellTarget;
            }
            var description = GetDescription(Path.GetExtension(fileName));
            if (description != null)
                return await DoLoad(fileName, description.EditorType);

            return null;
        }

        Task<bool> IFileService.Save(IFileDocumentItem file)
        {
            return DoSave(file, showSaveAs: false);
        }

        Task<bool> IFileService.SaveAs(IFileDocumentItem file)
        {
            return DoSave(file, showSaveAs: true);
        }

        private void CreateFilesDescriptionDictionary()
        {
            var editors = assemblyLookup.LoadedAssemblies
                .SelectMany(assembly => assembly.DefinedTypes)
                .SelectMany(typeInfo =>
                {
                    var attr = typeInfo.GetCustomAttribute<FileEditorAttribute>();
                    if (attr != null)
                        return attr.Extensions.Select(e => new KeyValuePair<string, IFileEditorDescription>(e, new FileEditorDescription(typeInfo, attr)));
                    return Enumerable.Empty<KeyValuePair<string, IFileEditorDescription>>();
                });

            foreach (var editor in editors)
            {
                if (!extensionTofileDescription.TryAdd(editor.Key, editor.Value))
                    throw new NotSupportedException($"The editor extension '{editor.Key}' has already been added");

                typeToFileDescription.TryAdd(editor.Value.EditorType, editor.Value);
            }
        }

        private async Task<IFileDocumentItem> AddWorkspace(Type editor)
        {
            var file = objectFactory.Create(editor) as IFileDocumentItem;
            await shell.AddWorkspaces(file);
            return file;
        }

        private async Task<IFileDocumentItem> CreateNew(string name, string content, IFileEditorDescription description)
        {
            if (string.IsNullOrEmpty(content))
                content = string.Empty;

            var file = await AddWorkspace(description.EditorType);

            if (string.IsNullOrEmpty(name))
            {
                var openedDocuments = shell.Workspaces.OfType<IDocumentItem>();
                var index = 1;
                name = string.Format(StudioResources.Untitled, index);
                while (openedDocuments.Any(i => i.DisplayName == $"{name}{description.Extensions.First()}"))
                    name = string.Format(StudioResources.Untitled, ++index);
            }

            var ext = Path.GetExtension(name);
            if (string.IsNullOrEmpty(ext))
            {
                ext = description.Extensions.FirstOrDefault();
                name = $"{name}{ext}";
            }

            await file.New(name, content);
            await shell.OpenWorkspaces(file);
            return file;
        }

        private async Task<IFileDocumentItem> DoLoad(string fileName, Type editorType)
        {
            Stream stream = null;
            IFileDocumentItem file = null;
            try
            {
                stream = await Task.FromResult<Stream>(new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read));
                if (stream == null)
                    return null;

                file = OpenedFiles.FirstOrDefault(d => d.FileName == fileName);
                if (file == null)
                    file = await AddWorkspace(editorType);

                file.FileName = fileName;
                await shell.OpenWorkspaces(file);
                await file.LoadFrom(stream);
                await recentFilesRepository.Add(fileName);

                return file;
            }
            catch (Exception ex)
            {
                file?.Close();
                MessageBox.Show($"Failed to open file {fileName}", SLStudioConstants.ProductName, MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Error(ex);

                return null;
            }
            finally
            {
                stream?.Dispose();
            }
        }

        private async Task<bool> DoSave(IFileDocumentItem file, bool showSaveAs)
        {
            Stream stream = new MemoryStream();
            try
            {
                var description = GetDescription(file);

                if (description.ReadOnly || !file.CanSave)
                    return false;

                if (string.IsNullOrEmpty(file.FileName) || file.IsNew || showSaveAs)
                {
                    var saveFileDialog = new SaveFileDialog()
                    {
                        FileName = file.DisplayName,
                        Filter = description.Filter,
                        AddExtension = true,
                        DefaultExt = description.Extensions.First(),
                        Title = SLStudioConstants.ProductName
                    };

                    if (!saveFileDialog.ShowDialog().GetValueOrDefault())
                        return false;

                    file.FileName = saveFileDialog.FileName;
                    file.DisplayName = Path.GetFileName(file.FileName);
                }

                await file.SaveTo(stream);
                await WriteStream(stream, file.FileName);

                await recentFilesRepository.Add(file.FileName);
                statusBar.Status = StudioResources.statusbar_status_fileSaved;
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return false;
            }
            finally
            {
                stream.Dispose();
            }
        }

        private static Task WriteStream(Stream content, string fileName)
        {
            var file = new FileInfo(fileName);
            var fileMode = file.Exists ? FileMode.Truncate : FileMode.Create;
            using (var fileStream = new FileStream(file.FullName, fileMode, FileAccess.Write, FileShare.Read))
            {
                content.Seek(0, SeekOrigin.Begin);
                content.CopyTo(fileStream);
                fileStream.Flush();
            }

            return Task.FromResult(true);
        }
    }

    public interface IFileService
    {
        IEnumerable<IFileDocumentItem> OpenedFiles { get; }

        IEnumerable<IFileEditorDescription> GetDescriptions();

        IFileEditorDescription GetDescription(IFileDocumentItem file);

        IFileEditorDescription GetDescription(Type editorType);

        IFileEditorDescription GetDescription(string ext);

        bool CanHandle(string fileName);

        Task<IFileDocumentItem> Empty(string extension);

        Task<IFileDocumentItem> New(string extension, string displayName = null, string content = null);

        Task<T> New<T>(string displayName = null, string content = null) where T : class, IFileDocumentItem;

        Task<IFileDocumentItem> Open(string fileName);

        Task<T> Open<T>(string fileName) where T : class, IFileDocumentItem;

        Task<bool> Save(IFileDocumentItem file);

        Task<bool> SaveAs(IFileDocumentItem file);
    }
}