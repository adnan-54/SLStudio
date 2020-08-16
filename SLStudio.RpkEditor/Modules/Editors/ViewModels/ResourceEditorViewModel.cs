using SLStudio.Core;
using SLStudio.RpkEditor.Data;
using SLStudio.RpkEditor.Editors;

namespace SLStudio.RpkEditor.Modules.Editors.ViewModels
{
    internal class ResourceEditorViewModel : WindowViewModel
    {
        public ResourceEditorViewModel(ResourceMetadata metadata, bool isEditing = false)
        {
            Metadata = metadata;
            DefinitionsEditor = metadata.Editor;

            if (isEditing)
            {
                Alias = metadata.Alias;
                SuperId = metadata.SuperId.ToStringId();
                TypeId = metadata.TypeId.ToStringId();
                IsParentCompatible = metadata.IsParentCompatible;
            }

            DisplayName = $"Resource Editor - {metadata.DisplayName}";
        }

        public ResourceMetadata Metadata { get; }

        public IResourceEditor DefinitionsEditor { get; }

        public string Alias
        {
            get => GetProperty(() => Alias);
            set
            {
                SetProperty(() => Alias, value);
            }
        }

        public string SuperId
        {
            get => GetProperty(() => SuperId);
            set
            {
                SetProperty(() => SuperId, value);
            }
        }

        public string TypeId
        {
            get => GetProperty(() => TypeId);
            set
            {
                SetProperty(() => TypeId, value);
            }
        }

        public bool IsParentCompatible
        {
            get => GetProperty(() => IsParentCompatible);
            set
            {
                SetProperty(() => IsParentCompatible, value);
            }
        }

        public bool IsValid => true;

        private void ApplyChanges()
        {
            Metadata.Alias = Alias;
            Metadata.SuperId = SuperId.ToIntId();
            Metadata.TypeId = TypeId.ToIntId();
            Metadata.IsParentCompatible = true;

            DefinitionsEditor.ApplyChanges();

            Metadata.UpdateDescription();
        }

        public override void TryClose(bool? dialogResult)
        {
            if (dialogResult == true)
            {
                if (!DefinitionsEditor.IsValid || !IsValid)
                    return;
                else
                    ApplyChanges();
            }

            base.TryClose(dialogResult);
        }
    }
}