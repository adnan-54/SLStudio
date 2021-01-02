using DevExpress.Mvvm;
using SLStudio.Core;

namespace SLStudio.RpkEditor.Data
{
    internal class RpkMetadata : BindableBase
    {
        public RpkMetadata()
        {
            ExternalReferences = new BindableCollection<ExternalReferenceMetadata>();
            Resources = new BindableCollection<ResourceMetadata>();
        }

        public string Path
        {
            get => GetProperty(() => Path);
            set => SetProperty(() => Path, value);
        }

        public BindableCollection<ExternalReferenceMetadata> ExternalReferences { get; }

        public BindableCollection<ResourceMetadata> Resources { get; }
    }
}