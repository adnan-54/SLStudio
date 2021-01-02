using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Results;
using SLStudio.Core;
using SLStudio.RpkEditor.Data;

namespace SLStudio.RpkEditor.Modules.RdfEditor.ViewModels
{
    internal class ExternalResourceEditorViewModel : WindowViewModel, INotifyDataErrorInfo
    {
        private static IReadOnlyCollection<string> properties;

        private readonly bool isEditing;
        private bool realtimeValidation = false;

        public ExternalResourceEditorViewModel()
        {
            Validator = new ExternalResourceEditorValidator();

            if (!isEditing)
                IsParentCompatible = true;

            DisplayName = "External resource editor";
        }

        public ExternalResourceEditorViewModel(ExternalResourceMetadata item) : this()
        {
            isEditing = true;

            TypeId = item.TypeId.ToStringId();
            SuperId = item.SuperId.ToStringId();
            AdditionalType = item.AdditionalType;
            Alias = item.Alias;
            IsParentCompatible = item.IsParentCompatible;
            TypeOfEntry = item.TypeOfEntry;

            Validate();
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IValidator<ExternalResourceEditorViewModel> Validator { get; }

        public ValidationResult Validation => Validator.Validate(this);

        public bool IsValid => Validate();

        public bool HasErrors => realtimeValidation && !Validation.IsValid;

        public IEnumerable<ResourceType> AvaliableResourceTypes => ResourceType.AvaliableTypes;

        public string TypeId
        {
            get => GetProperty(() => TypeId);
            set => SetProperty(() => TypeId, value);
        }

        public string SuperId
        {
            get => GetProperty(() => SuperId);
            set => SetProperty(() => SuperId, value);
        }

        public int AdditionalType
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

        public ResourceType TypeOfEntry
        {
            get => GetProperty(() => TypeOfEntry);
            set => SetProperty(() => TypeOfEntry, value);
        }

        public ExternalResourceMetadata GetMetadata()
        {
            if (!IsValid)
                throw new Exception("Metadata has invalid parameters");

            return new ExternalResourceMetadata()
            {
                TypeId = TypeId.ToIntId(),
                SuperId = SuperId.ToIntId(),
                AdditionalType = AdditionalType,
                Alias = Alias,
                IsParentCompatible = IsParentCompatible,
                TypeOfEntry = TypeOfEntry
            };
        }

        public IEnumerable GetErrors(string propertyName)
        {
            return Validation.Errors.Where(e => e.PropertyName == propertyName);
        }

        private bool Validate()
        {
            realtimeValidation = true;

            if (properties == null)
                properties = new List<string>(GetType().GetProperties().Select(p => p.Name));

            properties.ToList().ForEach(p => Validate(p));

            return Validation.IsValid;
        }

        public void Validate(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public override void TryClose(bool? dialogResult)
        {
            if (dialogResult == true && !IsValid)
                return;

            base.TryClose(dialogResult);
        }
    }

    internal class ExternalResourceEditorValidator : AbstractValidator<ExternalResourceEditorViewModel>
    {
        public ExternalResourceEditorValidator()
        {
            //TypeId
            RuleFor(vm => vm.TypeId).NotEmpty();
            RuleFor(vm => vm.TypeId).Must(BeAnHexId);

            //SuperId
            RuleFor(vm => vm.SuperId).NotEmpty();
            RuleFor(vm => vm.SuperId).Must(BeAnHexId);

            //AdditionalType
            //RuleFor(vm => vm.AdditionalType).NotEmpty();

            //Alias
            RuleFor(vm => vm.Alias).NotEmpty();

            //TypeOf
            RuleFor(vm => vm.TypeOfEntry).NotEmpty();
        }

        private static bool BeAnHexId(string id)
        {
            if (string.IsNullOrEmpty(id))
                return false;

            if (id.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
            {
                if (!Regex.IsMatch(id, @"0[xX][0-9a-fA-F]+"))
                    return false;
                try
                {
                    var result = Convert.ToInt32(id, 16);
                    return result >= 0 && result <= 0x00FFFFFF;
                }
                catch
                {
                    return false;
                }
            }

            return int.TryParse(id, out int intId) && intId >= 0 && intId <= 0x00FFFFFF;
        }
    }
}