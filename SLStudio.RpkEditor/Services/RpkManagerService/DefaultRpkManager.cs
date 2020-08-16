using SLStudio.Compilers;
using SLStudio.FileTypes.RpkFile;
using SLStudio.RpkEditor.Data;
using System.Threading.Tasks;

namespace SLStudio.RpkEditor.Services
{
    internal class DefaultRpkManager : IRpkManager
    {
        private readonly RpkMetadata rpkMetadata;

        public DefaultRpkManager(RpkMetadata rpkMetadata)
        {
            this.rpkMetadata = rpkMetadata;
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
        string ParseContent();

        Task<string> ParseContentAsync();
    }
}