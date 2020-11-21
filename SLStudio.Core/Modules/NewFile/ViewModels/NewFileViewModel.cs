﻿using FluentValidation;
using FluentValidation.Results;
using SLStudio.Core.Modules.NewFile.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SLStudio.Core.Modules.NewFile.ViewModels
{
    internal partial class NewFileViewModel : WindowViewModel, INewFileDialog, INotifyDataErrorInfo
    {
        private readonly IFileService fileService;
        private readonly ICollectionView collectionView;
        private readonly IUiSynchronization uiSynchronization;
        private readonly IValidator<NewFileViewModel> validator;

        private ValidationResult validationResult;

        public NewFileViewModel(IFileService fileService, IUiSynchronization uiSynchronization)
        {
            this.fileService = fileService;
            this.uiSynchronization = uiSynchronization;
            validator = new NewFileValidator();

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
            set => SetProperty(() => ShowLargeIcons, value);
        }

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

        public async Task OpenFile()
        {
            if (!CanOpen)
                return;

            await fileService.New(SelectedType.Extension, FileName);
            TryClose(true);
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

            if (SelectedSortMode != null)
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
            validationResult = validator.Validate(this, options => options.IncludeProperties(propertyName));
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