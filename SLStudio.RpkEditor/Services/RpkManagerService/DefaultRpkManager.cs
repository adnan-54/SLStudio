using SLStudio.Compilers;
using SLStudio.Core;
using SLStudio.FileTypes.RpkFile;
using SLStudio.RpkEditor.Data;
using SLStudio.RpkEditor.Modules.Editors.ViewModels;
using System;
using System.Threading.Tasks;

namespace SLStudio.RpkEditor.Services
{
    internal class DefaultRpkManager : IRpkManager
    {
        private readonly RpkMetadata rpkMetadata;
        private readonly IObjectFactory objectFactory;
        private readonly IWindowManager windowManager;

        public DefaultRpkManager(RpkMetadata rpkMetadata, IObjectFactory objectFactory, IWindowManager windowManager)
        {
            this.rpkMetadata = rpkMetadata;
            this.objectFactory = objectFactory;
            this.windowManager = windowManager;
        }

        public string ParseContent()
        {
            var rpk = ConvertToRpkData();
            return RpkCompiler.CompileToPlainText(rpk);
        }

        public Task<string> ParseContentAsync()
        {
            return Task.Run(() => ParseContent());
        }

        public ResourceMetadata AddResource(Type resourceType, int index = -1)
        {
            var metadata = objectFactory.Create(resourceType) as ResourceMetadata;
            metadata.Parent = rpkMetadata;

            var editor = new ResourceEditorViewModel(metadata);
            var result = windowManager.ShowDialog(editor);
            if (result.GetValueOrDefault())
            {
                if (index == -1)
                    rpkMetadata.ResourceMetadatas.Add(metadata);
                else
                    rpkMetadata.ResourceMetadatas.Insert(index, metadata);

                return metadata;
            }

            return null;
        }

        private Rpk ConvertToRpkData()
        {
            var rpk = new Rpk();

            foreach (var metadata in rpkMetadata.ResourceMetadatas)
            {
                if (metadata is MeshDefinitionMetadata meshDefinition)
                {
                    var mesh = new MeshDefinition
                    {
                        AdditionalType = meshDefinition.AdditionalType,
                        Alias = meshDefinition.Alias,
                        IsParentCompatible = meshDefinition.IsParentCompatible,
                        Mesh = new FileTypes.MeshFile.Mesh() { Path = meshDefinition.SourceFile },
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

    internal interface IRpkManager
    {
        ResourceMetadata AddResource(Type resourceType, int index = -1);

        string ParseContent();

        Task<string> ParseContentAsync();
    }
}