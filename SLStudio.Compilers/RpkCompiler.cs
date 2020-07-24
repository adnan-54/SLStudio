using SLStudio.FileTypes.RpkFile;
using System;
using System.Text;

namespace SLStudio.Compilers
{
    public class RpkCompiler
    {
        public string CompileToPlainText(Rpk rpk)
        {
            var sb = new StringBuilder();

            sb.AppendLine("<RPK>");
            sb.AppendLine("\t<EXTERNAL_REFS>");
            foreach (var externalRef in rpk.ExternalRefs)
                sb.AppendLine($"\t\t{externalRef.Path}");
            sb.AppendLine("\t</EXTERNAL_REFS>");
            foreach (var mesh in rpk.Meshes)
            {
                sb.AppendLine("\t<RES>");
                sb.AppendLine($"\t\tTypeID=0x{mesh.TypeId:X8}");
                sb.AppendLine($"\t\tSuperID=0x{mesh.SuperId:X8}");
                sb.AppendLine($"\t\tAdditionalType={mesh.AdditionalType}");
                sb.AppendLine($"\t\tAlias={mesh.Alias}");
                sb.AppendLine($"\t\tIsParentCompatible={Convert.ToInt32(mesh.IsParentCompatible)}");
                sb.AppendLine($"\t\tTypeOfEntry={Convert.ToInt32(mesh.TypeOfEntry)}");
                sb.AppendLine($"\t\t<RSD>");
                sb.AppendLine($"\t\t\t<STR>");
                sb.AppendLine($"\t\t\t\tsourcefile {mesh.Definition.SourceFile}");
                sb.AppendLine($"\t\t\t</STR>");
                sb.AppendLine($"\t\t/<RSD>");
                sb.AppendLine("\t</RES>");
            }
            sb.AppendLine("</RPK>");

            return sb.ToString();
        }
    }
}