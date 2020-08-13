using DevExpress.Mvvm;

namespace SLStudio.RpkEditor.Rpk
{
    internal abstract class ResourceBaseModel : BindableBase
    {
        public RpkModel Parent
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

        public abstract string Description { get; }

        public abstract string IconSource { get; }

        public abstract string Category { get; }
    }
}