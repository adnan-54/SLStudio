using FluentValidation;
using SLStudio.Core.Modules.NewFile.Resources;
using SLStudio.Core.Modules.NewFile.ViewModels;
using System.Text.RegularExpressions;

namespace SLStudio.Core.Modules.NewFile.Validations
{
    internal class NewFileViewModelValidations : AbstractValidator<NewFileViewModel>
    {
        public NewFileViewModelValidations()
        {
            RuleFor(vm => vm.FileName).Cascade(CascadeMode.Continue)
                .Must(BeAValidFileName).WithMessage(NewFileResources.ValidationInvalidFileName)
                .NotEmpty().WithMessage(NewFileResources.ValidationNameCannotBeEmpty)
                .MaximumLength(32).WithMessage(NewFileResources.ValidationNameLenght);

            RuleFor(vm => vm.SelectedType).NotNull();
        }

        private static bool BeAValidFileName(string fileName)
        {
            return !Regex.IsMatch(fileName, "[~\"#%&*:<>?/\\{|}]+");
        }
    }
}