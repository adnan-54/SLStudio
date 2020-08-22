using DevExpress.Mvvm;
using SLStudio.Core;
using SLStudio.Core.Behaviors;
using SLStudio.RpkEditor.Modules.Editors.ViewModels;
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

        public abstract string IconSource { get; }

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

        public RpkMetadata ExternalReference
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
            get => GetProperty(() => SuperId);
            set => SetProperty(() => SuperId, value);
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
    }
}