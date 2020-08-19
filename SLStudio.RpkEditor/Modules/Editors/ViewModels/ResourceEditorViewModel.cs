using FluentValidation;
using FluentValidation.Results;
using SLStudio.Core;
using SLStudio.RpkEditor.Data;
using SLStudio.RpkEditor.Editors;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;

namespace SLStudio.RpkEditor.Modules.Editors.ViewModels
{
    internal class ResourceEditorViewModel : WindowViewModel, INotifyDataErrorInfo
    {
        private bool realtimeValidation = false;

        public ResourceEditorViewModel(ResourceMetadata metadata, bool isEditing = false)
        {
            Metadata = metadata;
            DefinitionEditor = metadata.Editor;
            Alias = metadata.Alias;
            IsParentCompatible = metadata.IsParentCompatible;
            Validator = new ResourceEditorValidator();

            if (isEditing)
            {
                Validate();
                SuperId = metadata.SuperId.ToStringId();
                TypeId = metadata.TypeId.ToStringId();
            }

            DisplayName = $"Resource Editor - {metadata.DisplayName}";
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public ResourceMetadata Metadata { get; }

        public IDefinitionEditor DefinitionEditor { get; }

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

        public IValidator<ResourceEditorViewModel> Validator { get; }

        public ValidationResult Validation => Validator.Validate(this);

        public bool IsValid => Validate();

        public bool HasErrors
        {
            get
            {
                if (realtimeValidation)
                    return !Validation.IsValid;

                return false;
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return Validation.Errors.Where(e => e.PropertyName == propertyName);
        }

        public bool Validate()
        {
            realtimeValidation = true;

            Validate(nameof(Alias));
            Validate(nameof(SuperId));
            Validate(nameof(TypeId));
            Validate(nameof(IsParentCompatible));

            DefinitionEditor.Validate();

            return Validation.IsValid && DefinitionEditor.IsValid;
        }

        private void Validate(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public override void TryClose(bool? dialogResult)
        {
            if (dialogResult == true)
            {
                if (!IsValid)
                    return;
                else
                    ApplyChanges();
            }

            base.TryClose(dialogResult);
        }

        private void ApplyChanges()
        {
            Metadata.Alias = Alias;
            Metadata.SuperId = SuperId.ToIntId();
            Metadata.TypeId = TypeId.ToIntId();
            Metadata.IsParentCompatible = IsParentCompatible;
            DefinitionEditor.ApplyChanges();
            Metadata.UpdateProperties();
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