using DevExpress.Mvvm;
using SLStudio.Core;
using SLStudio.Core.Behaviors;
using SLStudio.RpkEditor.Editors;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.RpkEditor.Data
{
    internal abstract class ResourceMetadata : BindableBase, ISelectable
    {
        protected ResourceMetadata()
        {
            IsParentCompatible = true;

            Errors = new BindableCollection<string>();
            Warnings = new BindableCollection<string>();
            Infos = new BindableCollection<string>();
        }

        public BindableCollection<string> Errors { get; }

        public BindableCollection<string> Warnings { get; }

        public BindableCollection<string> Infos { get; }

        public abstract int AdditionalType { get; }

        public abstract ResourceType TypeOfEntry { get; }

        public abstract string DisplayName { get; }

        public IReadOnlyCollection<ResourceDescription> Description => BuildDescription().ToList();

        public abstract string IconSource { get; }

        public abstract string Category { get; }

        public abstract IDefinitionEditor Editor { get; }

        public bool HasErros => Errors.Any();

        public bool HasWarnings => Warnings.Any();

        public bool HasInfos => Warnings.Any();

        public bool HasExternalDependency => ExternalReference != null;

        public RpkMetadata Parent
        {
            get => GetProperty(() => Parent);
            set => SetProperty(() => Parent, value);
        }

        public RpkMetadata ExternalReference
        {
            get => GetProperty(() => ExternalReference);
            set => SetProperty(() => ExternalReference, value);
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

        public bool IsSelected
        {
            get => GetProperty(() => IsSelected);
            set => SetProperty(() => IsSelected, value);
        }

        public void UpdateDescription()
        {
            RaisePropertyChanged(() => Description);
        }

        protected virtual IEnumerable<ResourceDescription> BuildDescription()
        {
            yield return new ResourceDescription("DescriptionNotDefined", true);
        }
    }
}