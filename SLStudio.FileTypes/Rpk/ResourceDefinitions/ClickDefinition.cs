﻿using SLStudio.FileTypes.Attributes;
using SLStudio.FileTypes.MeshFile;

namespace SLStudio.FileTypes.RpkFile
{
    public class ClickDefinition : ResourceDefinition
    {
        internal override ResourceType TypeOfEntry => ResourceType.Click;

        public Mesh Mesh { get; set; }

        [ResourceAttribute("shape", 0)]
        public string Shape => Mesh.Path;
    }
}