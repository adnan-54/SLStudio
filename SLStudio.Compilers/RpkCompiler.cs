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
            foreach (var resource in rpk.Resources)
            {
                sb.AppendLine("\t<RES>");
                sb.AppendLine($"\t\tTypeID=0x{resource.TypeId:X8}");
                sb.AppendLine($"\t\tSuperID=0x{resource.SuperId:X8}");
                sb.AppendLine($"\t\tAdditionalType={resource.AdditionalType}");
                sb.AppendLine($"\t\tAlias={resource.Alias}");
                sb.AppendLine($"\t\tIsParentCompatible={Convert.ToInt32(resource.IsParentCompatible)}");
                sb.AppendLine($"\t\tTypeOfEntry={Convert.ToInt32(resource.TypeOfEntry)}");
                sb.AppendLine("\t</RES>");
            }
            sb.AppendLine("</RPK>");

            return sb.ToString();
        }
    }
}