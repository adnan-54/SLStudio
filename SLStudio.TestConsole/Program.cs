using SLStudio.Compilers;
using SLStudio.FileTypes.MeshFile;
using SLStudio.FileTypes.RpkFile;
using System.IO;

namespace SLStudio.TestConsole
{
    internal class Program
    {
        private static void Main()
        {
            var rpk = new Rpk();
            rpk.ExternalRefs.Add(new Rpk() { Path = "system.rpk" });
            rpk.ExternalRefs.Add(new Rpk() { Path = "cars.rpk" });
            rpk.ExternalRefs.Add(new Rpk() { Path = @"cars\racers.rpk" });
            rpk.ExternalRefs.Add(new Rpk() { Path = "parts.rpk" });
            rpk.ExternalRefs.Add(new Rpk() { Path = "sound.rpk" });
            rpk.ExternalRefs.Add(new Rpk() { Path = "particles.rpk" });

            rpk.Meshes.Add(new Resource<MeshDefinition>()
            {
                Parent = new ExternalResource(),
                TypeId = 56,
                AdditionalType = 0,
                Alias = "Test :D",
                IsParentCompatible = true,

                Definition = new MeshDefinition()
                {
                    Mesh = new Mesh()
                    {
                        Path = @"cars\racers\Giulia_GTA_data\meshes\F_bumper.scx"
                    }
                }
            });

            rpk.Meshes.Add(new Resource<MeshDefinition>()
            {
                Parent = new ExternalResource(),
                TypeId = 57,
                AdditionalType = 0,
                Alias = "Test 2 :D",
                IsParentCompatible = true,

                Definition = new MeshDefinition()
                {
                    Mesh = new Mesh()
                    {
                        Path = @"cars\racers\Giulia_GTA_data\meshes\F_bumper_2.scx"
                    }
                }
            });

            rpk.Meshes.Add(new Resource<MeshDefinition>()
            {
                Parent = new ExternalResource(),
                TypeId = 58,
                AdditionalType = 0,
                Alias = "Test 3 :D",
                IsParentCompatible = true,

                Definition = new MeshDefinition()
                {
                    Mesh = new Mesh() { Path = @"cars\racers\Giulia_GTA_data\meshes\F_bumper_3.scx" }
                }
            });

            var rpkCompiler = new RpkCompiler();
            var compiled = rpkCompiler.CompileToPlainText(rpk);

            File.WriteAllText(@"C:\Users\adnan\Desktop\output.rdb2", compiled);
        }
    }
}