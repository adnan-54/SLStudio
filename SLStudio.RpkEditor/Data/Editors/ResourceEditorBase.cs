using DevExpress.Mvvm;
using DevExpress.Mvvm.Native;
using FluentValidation;
using FluentValidation.Results;
using SLStudio.Core;
using System;
using System.Collections;
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

    //internal abstract class ResourceEditorBase<T> : WindowViewModel, IResourceEditor, INotifyDataErrorInfo where T : class
    //{
    //    private bool realtimeValidation = false;

    //    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    //    protected abstract IValidator<T> Validator { get; }

    //    public ValidationResult Validation => Validator.Validate(this as T);

    //    public bool IsValid => Validate();

    //    public bool HasErrors => realtimeValidation && !Validation.IsValid;

    //    public abstract void ApplyChanges();

    //    public IEnumerable GetErrors(string propertyName)
    //    {
    //        return Validation.Errors.Where(e => e.PropertyName == propertyName);
    //    }

    //    public bool Validate()
    //    {
    //        realtimeValidation = true;

    //        GetType().GetProperties().ForEach(p => Validate(p.Name));

    //        return Validation.IsValid;
    //    }

    //    private void Validate(string propertyName)
    //    {
    //        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    //    }
    //}
}