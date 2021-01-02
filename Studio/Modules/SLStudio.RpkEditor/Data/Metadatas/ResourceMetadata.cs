using DevExpress.Mvvm;
using SLStudio.Core;
using SLStudio.Core.Behaviors;
using SLStudio.RpkEditor.Modules.Editors.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.RpkEditor.Data
{
    internal abstract class ResourceMetadata : BindableBase, ISelectable
    {
        private readonly BindableCollection<string> errors;
        private readonly BindableCollection<string> warnings;
        private readonly BindableCollection<string> infos;

        protected ResourceMetadata()
        {
            errors = new BindableCollection<string>();
            warnings = new BindableCollection<string>();
            infos = new BindableCollection<string>();

            TypeId = -1;
            SuperId = -1;
            Alias = string.Empty;
            IsParentCompatible = true;

            ResourceEditor = new ResourceEditorViewModel(this);
        }

        public virtual string IconSource => TypeOfEntry?.Icon;

        public abstract string DisplayName { get; }

        public IReadOnlyCollection<ResourceDescription> Description => BuildDescription().ToList();

        public abstract string Category { get; }

        public virtual bool ShowInToolbox => true;

        public virtual IResourceEditor ResourceEditor { get; }

        public virtual IDefinitionsEditor DefinitionsEditor { get; }

        public bool HasErros => Errors.Any();

        public IReadOnlyCollection<string> Errors => errors;

        public bool HasWarnings => Warnings.Any();

        public IReadOnlyCollection<string> Warnings => warnings;

        public bool HasInfos => Infos.Any();

        public IReadOnlyCollection<string> Infos => infos;

        public RpkMetadata Parent
        {
            get => GetProperty(() => Parent);
            set => SetProperty(() => Parent, value);
        }

        public bool HasExternalReference => ExternalReference != null;

        public ExternalReferenceMetadata ExternalReference
        {
            get => GetProperty(() => ExternalReference);
            set
            {
                SetProperty(() => ExternalReference, value);
                RaisePropertyChanged(() => HasExternalReference);
            }
        }

        public int TypeId
        {
            get => GetProperty(() => TypeId);
            set => SetProperty(() => TypeId, value);
        }

        public int SuperId
        {
            get => GetSuperId();
            set => SetSuperId(value);
        }

        public abstract int AdditionalType { get; }

        public string Alias
        {
            get => GetProperty(() => Alias);
            set => SetProperty(() => Alias, value);
        }

        public bool IsParentCompatible
        {
            get => GetProperty(() => IsParentCompatible);
            set => SetProperty(() => IsParentCompatible, value);
        }

        public abstract ResourceType TypeOfEntry { get; }

        public bool IsSelected
        {
            get => GetProperty(() => IsSelected);
            set => SetProperty(() => IsSelected, value);
        }

        public void UpdateProperties()
        {
            RaisePropertyChanged(() => HasErros);
            RaisePropertyChanged(() => HasWarnings);
            RaisePropertyChanged(() => HasInfos);
            RaisePropertyChanged(() => HasExternalReference);
            RaisePropertyChanged(() => Description);
        }

        protected virtual IEnumerable<ResourceDescription> BuildDescription()
        {
            yield return new ResourceDescription("DescriptionNotDefined", true);
        }

        private int GetSuperId()
        {
            var externalReferenceIndex = 0;
            if (ExternalReference != null && Parent != null)
                externalReferenceIndex = Parent.ExternalReferences.IndexOf(ExternalReference) + 1;

            var superIdValue = GetProperty(() => SuperId);
            return Convert.ToInt32($"0x00{externalReferenceIndex:X2}{superIdValue:X4}", 16);
        }

        private void SetSuperId(int value)
        {
            if (value < -1 || value > 0x00FFFFFF)
                throw new ArgumentException($"{value} is not valid. Value must be between -1 and {0x00FFFFFF}");

            if (value == -1)
            {
                ExternalReference = null;
                SetProperty(() => SuperId, -1);
                return;
            }

            var stringValue = $"{value:X8}";

            var externalReferenceIndex = Convert.ToInt32($"0x{stringValue[^6..^4]}", 16);
            if (externalReferenceIndex > 0 && Parent != null)
            {
                if (externalReferenceIndex > Parent.ExternalReferences.Count - 1)
                    throw new ArgumentException($"The specified external reference index '{externalReferenceIndex}' is greater than the external references count '{Parent.ExternalReferences.Count - 1}'");
                ExternalReference = Parent.ExternalReferences[externalReferenceIndex];
            }
            else
                ExternalReference = null;

            var superIdValue = Convert.ToInt32($"0x{stringValue[^4..^0]}", 16);
            SetProperty(() => SuperId, superIdValue);
        }
    }
}