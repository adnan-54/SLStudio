using DevExpress.Mvvm;
using Newtonsoft.Json;
using SLStudio.Core;

namespace SLStudio.RpkEditor.Data
{
    [JsonObject(MemberSerialization.OptOut)]
    internal class ExternalReferenceMetadata : BindableBase
    {
        public ExternalReferenceMetadata()
        {
            Metadatas = new BindableCollection<ExternalResourceMetadata>();
        }

        public string Path
        {
            get => GetProperty(() => Path);
            set => SetProperty(() => Path, value);
        }

        public string Alias
        {
            get => GetProperty(() => Alias);
            set => SetProperty(() => Alias, value);
        }

        public string TargetVersion
        {
            get => GetProperty(() => TargetVersion);
            set => SetProperty(() => TargetVersion, value);
        }

        public BindableCollection<ExternalResourceMetadata> Metadatas { get; }
    }
}