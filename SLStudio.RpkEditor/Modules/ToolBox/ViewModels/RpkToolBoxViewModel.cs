using DevExpress.Mvvm;
using SLStudio.Core;

namespace SLStudio.RpkEditor.Modules.ToolBox.ViewModels
{
    internal class RpkToolBoxViewModel : ViewModelBase, IToolboxContent
    {
        public RpkToolBoxViewModel()
        {
            Teste = "aaaaaaaaaaa to com depressão em";
        }

        public string Teste
        {
            get => GetProperty(() => Teste);
            set => SetProperty(() => Teste, value);
        }
    }
}