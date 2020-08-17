using DevExpress.Mvvm;
using FluentValidation;
using FluentValidation.Results;
using SLStudio.RpkEditor.Editors;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;

namespace SLStudio.RpkEditor.Modules.Editors.ViewModels
{
    internal abstract class DefinitionEditorBase<T> : ViewModelBase, IDefinitionEditor, INotifyDataErrorInfo where T : class
    {
        private bool realtimeValidation = false;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected abstract IValidator<T> Validator { get; }

        public ValidationResult Validation => Validator.Validate(this as T);

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

        public abstract void ApplyChanges();

        public IEnumerable GetErrors(string propertyName)
        {
            return Validation.Errors.Where(e => e.PropertyName == propertyName);
        }

        public bool Validate()
        {
            realtimeValidation = true;

            var properties = GetType().GetProperties();

            foreach (var property in properties)
                Validate(property.Name);

            return Validation.IsValid;
        }

        private void Validate(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}