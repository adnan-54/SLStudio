using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.Core
{
    internal class ConfirmDocumentsClosingViewModel : WindowViewModel
    {
        public ConfirmDocumentsClosingViewModel(params IFileDocumentItem[] dirtyDocuments)
        {
            DirtyDocuments = dirtyDocuments.Select(d => new DirtyDocumentModel(d)).ToList();

            DisplayName = StudioConstants.ProductName;
        }

        public IReadOnlyCollection<DirtyDocumentModel> DirtyDocuments { get; }

        public string DirtyDocumentsString => string.Join(Environment.NewLine, DirtyDocuments.Select(d => d.DisplayName));

        public IEnumerable<IFileDocumentItem> FilesToSave { get; private set; }

        public bool CanShowAdvancedOptions => DirtyDocuments.Count > 1;

        public ConfirmationResult Result { get; private set; }

        public bool ShowAdvancedOptions
        {
            get => GetProperty(() => ShowAdvancedOptions);
            set => SetProperty(() => ShowAdvancedOptions, value);
        }

        public void ToggleShowAdvancedOptions()
        {
            ShowAdvancedOptions = !ShowAdvancedOptions;
        }

        public void Save()
        {
            SaveInternal();
            TryClose();
        }

        public void DontSave()
        {
            FilesToSave = Enumerable.Empty<IFileDocumentItem>();
            Result = ConfirmationResult.DontSave;
            TryClose();
        }

        public void Cancel()
        {
            FilesToSave = Enumerable.Empty<IFileDocumentItem>();
            Result = ConfirmationResult.Cancel;
            TryClose();
        }

        public override void TryClose()
        {
            if (Result == ConfirmationResult.None)
                SaveInternal();

            base.TryClose();
        }

        private void SaveInternal()
        {
            if (ShowAdvancedOptions)
            {
                FilesToSave = DirtyDocuments.Where(d => d.Save).Select(d => d.DocumentItem);
                Result = ConfirmationResult.Ok;
            }
            else
            {
                FilesToSave = DirtyDocuments.Select(d => d.DocumentItem);
                Result = ConfirmationResult.Save;
            }
        }

        public enum ConfirmationResult
        {
            None,
            Save,
            DontSave,
            Ok,
            Cancel
        }
    }

    internal class DirtyDocumentModel
    {
        public DirtyDocumentModel(IFileDocumentItem documentItem)
        {
            DocumentItem = documentItem;
            Save = true;
        }

        public IFileDocumentItem DocumentItem { get; }

        public string DisplayName => DocumentItem.DisplayName;

        public bool Save { get; set; }
    }
}