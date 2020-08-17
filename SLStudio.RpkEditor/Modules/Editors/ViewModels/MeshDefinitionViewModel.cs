using FluentValidation;
using SLStudio.RpkEditor.Data;

namespace SLStudio.RpkEditor.Modules.Editors.ViewModels
{
    internal class MeshDefinitionViewModel : DefinitionEditorBase<MeshDefinitionViewModel>
    {
        public MeshDefinitionViewModel(MeshDefinitionMetadata definition)
        {
            Validator = new MeshDefinitionValidator();

            Definition = definition;
            SourceFile = definition.SourceFile;
        }

        protected override IValidator<MeshDefinitionViewModel> Validator { get; }

        public MeshDefinitionMetadata Definition { get; }

        public string SourceFile
        {
            get => GetProperty(() => SourceFile);
            set
            {
                SetProperty(() => SourceFile, value);
            }
        }

        public override void ApplyChanges()
        {
            Definition.SourceFile = SourceFile;
        }
    }

    internal class MeshDefinitionValidator : AbstractValidator<MeshDefinitionViewModel>
    {
        public MeshDefinitionValidator()
        {
            RuleFor(vm => vm.SourceFile).Cascade(CascadeMode.Continue)
                .Must(BeAValidScxPath)
                .NotEmpty();
        }

        private static bool BeAValidScxPath(string path)
        {
            return !string.IsNullOrEmpty(path) && path.EndsWith(".scx");
        }
    }
}