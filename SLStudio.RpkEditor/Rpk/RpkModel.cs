using DevExpress.Mvvm;
using SLStudio.Core;

namespace SLStudio.RpkEditor.Rpk
{
    internal class RpkModel : BindableBase
    {
        public RpkModel()
        {
            ExternalRefs = new BindableCollection<RpkModel>();
            Resources = new BindableCollection<ResourceBaseModel>();
        }

        public string Path
        {
            get => GetProperty(() => Path);
            set => SetProperty(() => Path, value);
        }

        public BindableCollection<RpkModel> ExternalRefs { get; }

        public BindableCollection<ResourceBaseModel> Resources { get; }
    }
}