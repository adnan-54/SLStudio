using FluentValidation;
using SLStudio.Core;
using SLStudio.RpkEditor.Data;
using System.Linq;
using System.Threading.Tasks;

namespace SLStudio.RpkEditor.Modules.Editors.ViewModels
{
    internal class MeshDefinitionsViewModel : DefinitionsEditorBase<MeshDefinitionsViewModel>
    {
        public MeshDefinitionsViewModel(MeshDefinitionsMetadata definition)
        {
            Validator = new MeshDefinitionValidator();
            Definitions = definition;
            SourceFilesDirectories = new BindableCollection<string>();
        }

        protected override IValidator<MeshDefinitionsViewModel> Validator { get; }

        public MeshDefinitionsMetadata Definitions { get; }

        public BindableCollection<string> SourceFilesDirectories { get; }

        public string SourceFile
        {
            get => GetProperty(() => SourceFile);
            set => SetProperty(() => SourceFile, value);
        }

        public override void LoadValues()
        {
            SourceFile = Definitions.SourceFile;

            FetchSuggestions().FireAndForget();
        }

        public override void ApplyChanges()
        {
            Definitions.SourceFile = SourceFile;
        }

        private Task FetchSuggestions()
        {
            if (Definitions.Parent != null)
            {
                SourceFilesDirectories.Clear();

                var meshDefinitions = Definitions.Parent.Resources.Where(r => r is MeshDefinitionsMetadata).Select(r => r as MeshDefinitionsMetadata);
                var sourceFiles = meshDefinitions.Select(m => m.SourceFile.Substring(0, m.SourceFile.Replace('\\', '/').LastIndexOf('/') + 1)).Distinct().Where(s => !string.IsNullOrEmpty(s));
                SourceFilesDirectories.AddRange(sourceFiles);
            }
            return Task.CompletedTask;
        }
    }

    internal class MeshDefinitionValidator : AbstractValidator<MeshDefinitionsViewModel>
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