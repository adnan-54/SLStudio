using FluentValidation;
using FluentValidation.Results;
using SLStudio.Core;
using SLStudio.RpkEditor.Data;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;

namespace SLStudio.RpkEditor.Modules.Editors.ViewModels
{
    internal class ResourceEditorViewModel : WindowViewModel, IResourceEditor, INotifyDataErrorInfo
    {
        private readonly ResourceMetadata metadata;

        private bool realtimeValidation = false;

        public ResourceEditorViewModel(ResourceMetadata metadata)
        {
            this.metadata = metadata;

            Validator = new ResourceEditorValidator();
            DisplayName = $"Resource Editor - {metadata.DisplayName}";
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IValidator<ResourceEditorViewModel> Validator { get; }

        public ValidationResult Validation => Validator.Validate(this);

        public bool IsValid => Validate();

        public bool HasErrors => realtimeValidation && !Validation.IsValid;

        public bool HasDefinitionsEditor => DefinitionsEditor != null && !(DefinitionsEditor is NullDefinitionsEditor);

        public IDefinitionsEditor DefinitionsEditor => Metadata.DefinitionsEditor ?? NullDefinitionsEditor.Default;

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

        internal ResourceMetadata Metadata => metadata;

        public IEnumerable GetErrors(string propertyName)
        {
            return Validation.Errors.Where(e => e.PropertyName == propertyName).Select(r => r.ErrorMessage);
        }

        public void LoadValues()
        {
            Alias = Metadata.Alias;
            SuperId = Metadata.SuperId.ToStringId();
            TypeId = Metadata.TypeId.ToStringId();
            IsParentCompatible = Metadata.IsParentCompatible;
            DefinitionsEditor.LoadValues();
        }

        public void ApplyChanges()
        {
            Metadata.Alias = Alias;
            Metadata.SuperId = SuperId.ToIntId();
            Metadata.TypeId = TypeId.ToIntId();
            Metadata.IsParentCompatible = IsParentCompatible;
            DefinitionsEditor.ApplyChanges();
            Metadata.UpdateProperties();
        }

        public bool Validate()
        {
            realtimeValidation = true;

            Validate(nameof(Alias));
            Validate(nameof(SuperId));
            Validate(nameof(TypeId));
            Validate(nameof(IsParentCompatible));

            return DefinitionsEditor.IsValid && Validation.IsValid;
        }

        private void Validate(string propertyName = null)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public override void OnLoaded()
        {
            LoadValues();
        }

        public override void TryClose(bool? dialogResult)
        {
            if (dialogResult == true)
            {
                if (IsValid)
                    ApplyChanges();
                else
                    return;
            }

            base.TryClose(dialogResult);
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