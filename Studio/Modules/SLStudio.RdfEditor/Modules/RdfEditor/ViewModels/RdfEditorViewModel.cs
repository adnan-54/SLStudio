using SLStudio.Core;
using SLStudio.Language.Rdf;
using SLStudio.RdfEditor.Modules.RdfEditor.Resources;
using SLStudio.RdfEditor.Services;
using System.Threading.Tasks;

namespace SLStudio.RdfEditor.Modules.RdfEditor.ViewModels
{
    [FileEditor(".rdf", "rdfEditorName", "rdfEditorDescription", "categoryPath", typeof(RdfEditorResources), "pack://application:,,,/SLStudio.RdfEditor;component/Resources/Icons/rdfFileIcon.png")]
    internal class RdfEditorViewModel : FileDocumentPanelBase, IRdfEditor
    {
        private readonly RdfCodeViewModel code;
        private readonly RdfDesignerViewModel designer;

        public RdfEditorViewModel(IRdfDefinitionLookup lookup)
        {
            Editors = new BindableCollection<RdfEditorViewModelBase>();
            code = new RdfCodeViewModel();
            Editors.Add(code);
            designer = new RdfDesignerViewModel();
            SelectedEditor = designer;
            Editors.Add(designer);

            ToolContentProvider = new DefaultToolContentProvider(lookup);
        }

        public BindableCollection<RdfEditorViewModelBase> Editors { get; }

        public bool IsBusy
        {
            get => GetProperty(() => IsBusy);
            set => SetProperty(() => IsBusy, value);
        }

        public RdfEditorViewModelBase SelectedEditor
        {
            get => GetProperty(() => SelectedEditor);
            set => SetProperty(() => SelectedEditor, value);
        }

        protected override Task DoLoad()
        {
            return Task.CompletedTask;
        }

        protected override Task DoNew(string content)
        {
            return Task.CompletedTask;
        }

        protected override Task DoSave()
        {
            return Task.CompletedTask;
        }
    }
}