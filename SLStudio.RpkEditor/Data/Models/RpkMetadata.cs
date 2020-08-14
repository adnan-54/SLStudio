using DevExpress.Mvvm;
using SLStudio.Core;

namespace SLStudio.RpkEditor.Rpk
{
    internal class RpkMetadata : BindableBase
    {
        public RpkMetadata()
        {
            ExternalRefs = new BindableCollection<RpkMetadata>();
            Resources = new BindableCollection<ResourceMetadata>();
        }

        public string Path
        {
            get => GetProperty(() => Path);
            set => SetProperty(() => Path, value);
        }

        public BindableCollection<RpkMetadata> ExternalRefs { get; }

        public BindableCollection<ResourceMetadata> Resources { get; }
    }
}