using DevExpress.Mvvm;
using Newtonsoft.Json;

namespace SLStudio.RpkEditor.Data
{
    [JsonObject(MemberSerialization.OptOut)]
    internal class ExternalResourceMetadata : BindableBase
    {
        public int TypeId
        {
            get => GetProperty(() => TypeId);
            set => SetProperty(() => TypeId, value);
        }

        public int SuperId
        {
            get => GetProperty(() => SuperId);
            set => SetProperty(() => SuperId, value);
        }

        public int AdditionalType
        {
            get => GetProperty(() => AdditionalType);
            set => SetProperty(() => AdditionalType, value);
        }

        public string Alias
        {
            get => GetProperty(() => Alias);
            set => SetProperty(() => Alias, value);
        }

        public bool IsParentCompatible
        {
            get => GetProperty(() => IsParentCompatible);
            set => SetProperty(() => IsParentCompatible, value);
        }

        [JsonProperty]
        public int? TypeOfEntryId { get; private set; }

        [JsonIgnore]
        public ResourceType TypeOfEntry
        {
            get
            {
                var value = GetProperty(() => TypeOfEntry);
                if (value == null && TypeOfEntryId.HasValue)
                {
                    value = ResourceType.FromId(TypeOfEntryId.Value);
                    TypeOfEntry = value;
                }

                return value;
            }
            set
            {
                SetProperty(() => TypeOfEntry, value);
                TypeOfEntryId = value?.Id;
                RaisePropertyChanged(() => Icon);
            }
        }

        public string Icon => TypeOfEntry?.Icon;

        public bool IsSelected
        {
            get => GetProperty(() => IsSelected);
            set => SetProperty(() => IsSelected, value);
        }
    }
}