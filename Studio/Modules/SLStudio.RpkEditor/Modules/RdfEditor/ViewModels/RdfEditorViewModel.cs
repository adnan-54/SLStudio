using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using MvvmDialogs;
using MvvmDialogs.FrameworkDialogs.SaveFile;
using Newtonsoft.Json;
using SLStudio.Core;
using SLStudio.RpkEditor.Data;
using SLStudio.RpkEditor.Modules.RdfEditor.Resources;

namespace SLStudio.RpkEditor.Modules.RdfEditor.ViewModels
{
    //[FileEditor(".rdf", "rdfEditorName", "rdfEditorDescription", "categoryPath", typeof(RdfEditorResources), "pack://application:,,,/SLStudio.RpkEditor;component/Resources/Icons/rdfFileIcon.png")]
    internal class RdfEditorViewModel : FileDocumentPanelBase, IRdfEditor
    {
        private readonly IWindowManager windowManager;
        private readonly IDialogService dialogService;
        private readonly IFileService fileService;

        private readonly ICollectionView collectionView;

        public RdfEditorViewModel(IWindowManager windowManager, IDialogService dialogService, IFileService fileService)
        {
            this.windowManager = windowManager;
            this.dialogService = dialogService;
            this.fileService = fileService;
            Metadatas = new BindableCollection<ExternalResourceMetadata>();
            collectionView = CollectionViewSource.GetDefaultView(Metadatas);
            collectionView.Filter += Filter;
        }

        public BindableCollection<ExternalResourceMetadata> Metadatas { get; }

        public IEnumerable<ExternalResourceMetadata> SelectedItems => Metadatas.Where(m => m.IsSelected).ToList();

        public bool IsBusy
        {
            get => GetProperty(() => IsBusy);
            set => SetProperty(() => IsBusy, value);
        }

        public string SearchTerm
        {
            get => GetProperty(() => SearchTerm);
            set
            {
                SetProperty(() => SearchTerm, value);
                collectionView.Refresh();
            }
        }

        public string Path
        {
            get => GetProperty(() => Path);
            set => SetProperty(() => Path, value);
        }

        public string Alias
        {
            get => GetProperty(() => Alias);
            set => SetProperty(() => Alias, value);
        }

        public string TargetVersion
        {
            get => GetProperty(() => TargetVersion);
            set => SetProperty(() => TargetVersion, value);
        }

        public void OnCopy()
        {
        }

        public void OnPaste()
        {
        }

        public void OnCut()
        {
        }

        public void AddNew()
        {
            var model = new ExternalResourceEditorViewModel();
            var result = windowManager.ShowDialog(model);
            if (result == true)
            {
                var metadata = model.GetMetadata();
                Metadatas.Add(metadata);
            }
        }

        public void MoveUp()
        {
        }

        public void MoveDown()
        {
        }

        public void RemoveSelected()
        {
            Metadatas.RemoveRange(SelectedItems);
        }

        public void RemoveItem(ExternalResourceMetadata item)
        {
            Metadatas.Remove(item);
        }

        public void EditSelected()
        {
            EditItem(SelectedItems.FirstOrDefault());
        }

        public void EditItem(ExternalResourceMetadata item)
        {
            if (item == null)
                return;

            var model = new ExternalResourceEditorViewModel(item);
            var result = windowManager.ShowDialog(model);
            if (result == true)
            {
                var metadata = model.GetMetadata();

                item.TypeId = metadata.TypeId;
                item.SuperId = metadata.SuperId;
                item.AdditionalType = metadata.AdditionalType;
                item.Alias = metadata.Alias;
                item.IsParentCompatible = metadata.IsParentCompatible;
                item.TypeOfEntry = metadata.TypeOfEntry;
            }
        }

        public void Save()
        {
            DoSave().FireAndForget();
        }

        protected override Task DoLoad()
        {
            return Task.CompletedTask;
        }

        protected override Task DoNew(string content)
        {
            return Task.CompletedTask;
        }

        protected override async Task DoSave()
        {
            if (string.IsNullOrEmpty(Path) || string.IsNullOrEmpty(Alias) || string.IsNullOrEmpty(TargetVersion))
            {
                MessageBox.Show("Path, Alias and Target version are required");
                return;
            }

            var settings = new SaveFileDialogSettings
            {
                Title = "Save",
                Filter = fileService.GetFilter(".rdf"),
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                ValidateNames = true,
                FileName = $"{Path}_{TargetVersion}_{Alias}",
                AddExtension = true,
                OverwritePrompt = true
            };

            var success = dialogService.ShowSaveFileDialog(this, settings);

            if (success != true)
                return;

            var externalRef = new ExternalReferenceMetadata
            {
                Path = Path,
                Alias = Alias,
                TargetVersion = TargetVersion
            };

            externalRef.Metadatas.AddRange(Metadatas);

            IsBusy = true;

            var jsonString = JsonConvert.SerializeObject(externalRef);

            await File.WriteAllTextAsync(settings.FileName, jsonString);

            IsBusy = false;
        }

        private bool Filter(object obj)
        {
            if (obj is not ExternalResourceMetadata item)
                return false;

            return string.IsNullOrEmpty(SearchTerm)
                || item.Alias.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase)
                || item.TypeOfEntry.Name.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase);
        }
    }

    public interface IRdfEditor : IFileDocumentPanel
    {
    }
}