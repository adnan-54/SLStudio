using DevExpress.Mvvm.Native;
using FluentValidation;
using FluentValidation.Results;
using SLStudio.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SLStudio.RpkEditor.Data
{
    internal interface IResourceEditor : IWindow
    {
        IDefinitionsEditor DefinitionsEditor { get; }

        bool IsValid { get; }

        void LoadValues();

        void ApplyChanges();

        bool Validate();
    }

    internal abstract class ResourceEditorBase<T> : WindowViewModel, IResourceEditor, INotifyDataErrorInfo where T : class
    {
        private static IReadOnlyCollection<string> properties;

        private bool realtimeValidation = false;

        protected ResourceEditorBase(ResourceMetadata metadata)
        {
            Metadata = metadata;
            DisplayName = $"Resource Editor - {metadata.DisplayName}";
            IconSource = metadata.IconSource;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public string IconSource
        {
            get => GetProperty(() => IconSource);
            set => SetProperty(() => IconSource, value);
        }

        protected abstract IValidator<T> Validator { get; }

        public ValidationResult Validation => Validator.Validate(this as T);

        public bool IsValid => Validate();

        public bool HasErrors => realtimeValidation && !Validation.IsValid;

        public ResourceMetadata Metadata { get; }

        public bool HasDefinitionsEditor => DefinitionsEditor != null && !(DefinitionsEditor is NullDefinitionsEditor);

        public IDefinitionsEditor DefinitionsEditor => Metadata.DefinitionsEditor ?? NullDefinitionsEditor.Default;

        public void LoadValues()
        {
            OnLoadValues();
            DefinitionsEditor.LoadValues();
        }

        protected abstract void OnLoadValues();

        public void ApplyChanges()
        {
            OnApplyChanges();
            DefinitionsEditor.ApplyChanges();
            Metadata.UpdateProperties();
        }

        protected abstract void OnApplyChanges();

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

            return DefinitionsEditor.IsValid && Validation.IsValid;
        }

        private void Validate(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public override void OnLoaded()
        {
            LoadValues();
        }

        public override void OnClosed()
        {
            LoadValues();
            Validate();
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
}