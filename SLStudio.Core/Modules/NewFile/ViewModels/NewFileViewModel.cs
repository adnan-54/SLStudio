﻿using DevExpress.Mvvm.POCO;
using FluentValidation;
using FluentValidation.Results;
using SLStudio.Core.Modules.NewFile.Resources;
using SLStudio.Core.Modules.NewFile.Validations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SLStudio.Core.Modules.NewFile.ViewModels
{
    internal class NewFileViewModel : WindowViewModel, INotifyDataErrorInfo
    {
        private readonly IFileService fileService;
        private readonly ICollectionView collectionView;
        private readonly IUiSynchronization uiSynchronization;
        private readonly IValidator<NewFileViewModel> validator;

        private ValidationResult validationResult;

        public NewFileViewModel(IShell shell, IUiSynchronization uiSynchronization)
        {
            this.uiSynchronization = uiSynchronization;
            fileService = shell.GetService<IFileService>();
            validator = new NewFileViewModelValidations();

            AvaliableTypes = new BindableCollection<IFileDescription>();
            collectionView = CollectionViewSource.GetDefaultView(AvaliableTypes);
            collectionView.Filter += Filter;

            LoadAvaliableTypes().FireAndForget();

            SortModes = new List<SortModeModel>()
            {
                new SortModeModel(NewFileResources.NameAscending, new SortDescription("DisplayName", ListSortDirection.Ascending)),
                new SortModeModel(NewFileResources.NameDescending, new SortDescription("DisplayName", ListSortDirection.Descending))
            };

            SelectedSortMode = SortModes.FirstOrDefault();

            ShowLargeIcons = true;
            FileName = NewFileResources.Untitled;
            DisplayName = NewFileResources.NewFile;
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => !validationResult.IsValid;

        public IEnumerable<SortModeModel> SortModes { get; }

        public SortModeModel SelectedSortMode
        {
            get => GetProperty(() => SelectedSortMode);
            set
            {
                SetProperty(() => SelectedSortMode, value);
                SortAvaliableTypes();
            }
        }

        public bool ShowLargeIcons
        {
            get => GetProperty(() => ShowLargeIcons);
            set
            {
                SetProperty(() => ShowLargeIcons, value);
                RaisePropertyChanged(() => ShowSmallIcons);
            }
        }

        public bool ShowSmallIcons => !ShowLargeIcons;

        public string SearchTerm
        {
            get => GetProperty(() => SearchTerm);
            set
            {
                SetProperty(() => SearchTerm, value);
                collectionView.Refresh();
            }
        }

        public BindableCollection<IFileDescription> AvaliableTypes { get; }

        public IFileDescription SelectedType
        {
            get => GetProperty(() => SelectedType);
            set
            {
                SetProperty(() => SelectedType, value);
                Validate(nameof(SelectedType));
                RaisePropertyChanged(() => CanOpen);
            }
        }

        public string FileName
        {
            get => GetProperty(() => FileName);
            set
            {
                SetProperty(() => FileName, value);
                Validate(nameof(FileName));
                RaisePropertyChanged(() => CanOpen);
            }
        }

        public bool CanOpen => !HasErrors;

        public IEnumerable GetErrors(string propertyName)
        {
            return validationResult?.Errors.Where(e => e.PropertyName == propertyName);
        }

        private Task LoadAvaliableTypes()
        {
            return uiSynchronization.InvokeOnUiAsync(() =>
            {
                AvaliableTypes.AddRange(fileService.GetFileDescriptions());
                SelectedType = collectionView.Cast<IFileDescription>().FirstOrDefault();
            });
        }

        private void SortAvaliableTypes()
        {
            collectionView.SortDescriptions.Clear();

            if (SelectedSortMode != null && SelectedSortMode.Description != null)
                collectionView.SortDescriptions.Add(SelectedSortMode.Description);

            collectionView.Refresh();
        }

        private bool Filter(object obj)
        {
            if (!(obj is IFileDescription fileDescription))
                return false;

            if (string.IsNullOrWhiteSpace(SearchTerm))
                return true;

            return fileDescription.DisplayName.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase)
                || fileDescription.Description.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase)
                || fileDescription.Extension.Contains(SearchTerm, StringComparison.InvariantCultureIgnoreCase);
        }

        private void Validate(string propertyName)
        {
            validationResult = validator.Validate(this, propertyName);
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
    }

    internal class SortModeModel
    {
        public SortModeModel(string displayName, SortDescription description)
        {
            DisplayName = displayName;
            Description = description;
        }

        public string DisplayName { get; }
        public SortDescription Description { get; }
    }
}