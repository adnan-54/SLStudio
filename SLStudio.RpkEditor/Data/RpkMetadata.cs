using DevExpress.Mvvm;
using SLStudio.Core;

namespace SLStudio.RpkEditor.Data
{
    internal class RpkMetadata : BindableBase
    {
        public RpkMetadata()
        {
            ExternalRefs = new BindableCollection<RpkMetadata>();
            ResourceMetadatas = new BindableCollection<ResourceMetadata>();
        }

        public string Path
        {
            get => GetProperty(() => Path);
            set => SetProperty(() => Path, value);
        }

        public BindableCollection<RpkMetadata> ExternalRefs { get; }

        public BindableCollection<ResourceMetadata> ResourceMetadatas { get; }
    }
}