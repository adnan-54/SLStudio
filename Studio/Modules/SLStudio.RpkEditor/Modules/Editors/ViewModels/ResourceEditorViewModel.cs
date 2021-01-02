using FluentValidation;
using SLStudio.RpkEditor.Data;

namespace SLStudio.RpkEditor.Modules.Editors.ViewModels
{
    internal class ResourceEditorViewModel : ResourceEditorBase<ResourceEditorViewModel>
    {
        public ResourceEditorViewModel(ResourceMetadata metadata) : base(metadata)
        {
            Validator = new ResourceEditorValidator();
        }

        protected override IValidator<ResourceEditorViewModel> Validator { get; }

        public string Alias
        {
            get => GetProperty(() => Alias);
            set => SetProperty(() => Alias, value);
        }

        public string SuperId
        {
            get => GetProperty(() => SuperId);
            set => SetProperty(() => SuperId, value);
        }

        public string TypeId
        {
            get => GetProperty(() => TypeId);
            set => SetProperty(() => TypeId, value);
        }

        public bool IsParentCompatible
        {
            get => GetProperty(() => IsParentCompatible);
            set => SetProperty(() => IsParentCompatible, value);
        }

        protected override void OnLoadValues()
        {
            Alias = Metadata.Alias;
            SuperId = Metadata.SuperId.ToStringId();
            TypeId = Metadata.TypeId.ToStringId();
            IsParentCompatible = Metadata.IsParentCompatible;
        }

        protected override void OnApplyChanges()
        {
            Metadata.Alias = Alias;
            Metadata.SuperId = SuperId.ToIntId();
            Metadata.TypeId = TypeId.ToIntId();
            Metadata.IsParentCompatible = IsParentCompatible;
        }
    }

    internal class ResourceEditorValidator : AbstractValidator<ResourceEditorViewModel>
    {
        public ResourceEditorValidator()
        {
            RuleFor(vm => vm.SuperId).NotEmpty();
            RuleFor(vm => vm.TypeId).NotEmpty();
        }
    }
}