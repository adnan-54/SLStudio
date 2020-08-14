using DevExpress.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.RpkEditor.Rpk
{
    internal abstract class ResourceMetadata : BindableBase
    {
        public RpkMetadata Parent
        {
            get => GetProperty(() => Parent);
            set => SetProperty(() => Parent, value);
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

        public virtual int AdditionalType
        {
            get => GetProperty(() => AdditionalType);
            set => SetProperty(() => AdditionalType, value);
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

        public abstract ResourceType TypeOfEntry { get; }

        public abstract string DisplayName { get; }

        public IReadOnlyCollection<ResourceDescription> Description => BuildDescription().ToList();

        public abstract string IconSource { get; }

        public abstract string Category { get; }

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