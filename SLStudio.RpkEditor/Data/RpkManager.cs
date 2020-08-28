using SLStudio.Compilers;
using SLStudio.Core;
using SLStudio.FileTypes.MeshFile;
using SLStudio.FileTypes.RpkFile;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.RpkEditor.Data
{
    internal class RpkManager
    {
        private readonly RpkMetadata rpkMetadata;
        private readonly IObjectFactory objectFactory;
        private readonly IWindowManager windowManager;

        public RpkManager(RpkMetadata rpkMetadata, IObjectFactory objectFactory, IWindowManager windowManager)
        {
            this.rpkMetadata = rpkMetadata;
            this.objectFactory = objectFactory;
            this.windowManager = windowManager;
        }

        public string Content { get; private set; }

        public string Parse()
        {
            var rpk = ConvertToRpk();
            Content = RpkCompiler.CompileToPlainText(rpk);
            return Content;
        }

        public ResourceMetadata AddResource(Type resourceType, int index = -1)
        {
            var metadata = CreateMetadata(resourceType);

            var editor = metadata.ResourceEditor;
            var result = windowManager.ShowDialog(editor);
            if (result == true)
            {
                if (index == -1)
                    rpkMetadata.Resources.Add(metadata);
                else
                    rpkMetadata.Resources.Insert(index, metadata);

                return metadata;
            }

            return null;
        }

        public bool EditResource(ResourceMetadata metadata)
        {
            var editor = metadata.ResourceEditor;
            editor.Validate();
            return windowManager.ShowDialog(editor).GetValueOrDefault();
        }

        public void RemoveResources(IEnumerable<ResourceMetadata> metadatas)
        {
            rpkMetadata.Resources.RemoveRange(metadatas);
        }

        private ResourceMetadata CreateMetadata(Type resourceType)
        {
            if (!resourceType.IsSubclassOf(typeof(ResourceMetadata)))
                throw new InvalidOperationException($"{resourceType.Name} is not a resource metadata");

            var metadata = objectFactory.Create(resourceType) as ResourceMetadata;
            metadata.Parent = rpkMetadata;
            return metadata;
        }

        private Rpk ConvertToRpk()
        {
            var rpk = new Rpk();

            foreach (var metadata in rpkMetadata.Resources)
            {
                if (metadata is MeshDefinitionsMetadata meshDefinition)
                {
                    var mesh = new MeshDefinition
                    {
                        AdditionalType = meshDefinition.AdditionalType,
                        Alias = meshDefinition.Alias,
                        IsParentCompatible = meshDefinition.IsParentCompatible,
                        Mesh = new Mesh() { Path = meshDefinition.SourceFile },
                        SuperId = meshDefinition.SuperId,
                        TypeId = meshDefinition.TypeId
                    };

                    rpk.Resources.Add(mesh);

                    continue;
                }
            }

            return rpk;
        }
    }
}