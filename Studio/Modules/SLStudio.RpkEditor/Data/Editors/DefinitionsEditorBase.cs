using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SLStudio.RpkEditor.Data
{
    internal interface IDefinitionsEditor
    {
        bool IsValid { get; }

        bool Validate();

        void LoadValues();

        void ApplyChanges();
    }

    internal abstract class DefinitionsEditorBase<T> : ViewModelBase, IDefinitionsEditor, INotifyDataErrorInfo where T : class
    {
        private static IReadOnlyCollection<string> properties;

        private bool realtimeValidation = false;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected abstract IValidator<T> Validator { get; }

        public ValidationResult Validation => Validator.Validate(this as T);

        public bool IsValid => Validate();

        public bool HasErrors => realtimeValidation && !Validation.IsValid;

        public abstract void LoadValues();

        public abstract void ApplyChanges();

        public IEnumerable GetErrors(string propertyName)
        {
            return Validation.Errors.Where(e => e.PropertyName == propertyName);
        }

        public bool Validate()
        {
            realtimeValidation = true;

            if (properties == null)
                properties = new List<string>(GetType().GetProperties().Select(p => p.Name));

            properties.ForEach(p => Validate(p));

            return Validation.IsValid;
        }

        private void Validate(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }
}