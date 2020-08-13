using System;

namespace SLStudio.RpkEditor.Rpk.Definitions
{
    internal class MeshDefinitionModel : ResourceBaseModel
    {
        public override ResourceType TypeOfEntry => new ResourceType();

        public override string DisplayName => "Mesh";

        public override string Description => $"Zinco para o loiro {TypeId} como dale {Alias}";

        public override string IconSource => "";

        public override string Category => "Mesh";
    }
}