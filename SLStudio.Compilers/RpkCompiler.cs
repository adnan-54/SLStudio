using SLStudio.FileTypes.Attributes;
using SLStudio.FileTypes.RpkFile;
using System;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SLStudio.Compilers
{
    public static class RpkCompiler
    {
        public static string CompileToPlainText(Rpk rpk)
        {
            var sb = new StringBuilder();

            //header
            sb.AppendLine("<RPK>");

            //externalRefs
            if (rpk.ExternalRefs.Any())
            {
                sb.AppendLine("\t<EXTERNAL_REFS>");
                foreach (var externalRef in rpk.ExternalRefs)
                    sb.AppendLine($"\t\t{externalRef.Path}");
                sb.AppendLine("\t</EXTERNAL_REFS>");
            }

            //resources
            foreach (var resource in rpk.Resources)
            {
                //parameters
                sb.AppendLine("\t<RES>");
                sb.AppendLine($"\t\tTypeID=0x{resource.TypeId:X8}");
                sb.AppendLine($"\t\tSuperID=0x{resource.SuperId:X8}");
                sb.AppendLine($"\t\tAdditionalType={resource.AdditionalType}");
                sb.AppendLine($"\t\tAlias={resource.Alias}");
                sb.AppendLine($"\t\tIsParentCompatible={Convert.ToInt32(resource.IsParentCompatible)}");
                sb.AppendLine($"\t\tTypeOfEntry={Convert.ToInt32(resource.TypeOfEntry)}");

                //definitions
                sb.AppendLine($"\t\t<RSD>");

                var attributes = resource.GetType()
                                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                .Select(p => new
                                {
                                    Property = p,
                                    Value = p.GetValue(resource),
                                    Attribute = (ResourceAttributeAttribute)Attribute.GetCustomAttribute(p, typeof(ResourceAttributeAttribute), true)
                                })
                                .OrderBy(x => x.Attribute != null ? x.Attribute.Index : -1)
                                .Where(x => x.Attribute != null && !(string.IsNullOrEmpty(x.Value as string)))
                                .ToList();

                if (attributes.Any())
                {
                    sb.AppendLine($"\t\t\t<STR>");

                    foreach (var attribute in attributes)
                        sb.AppendLine($"\t\t\t\t{attribute.Attribute.AttributeName} {attribute.Value}");

                    sb.AppendLine($"\t\t\t</STR>");
                }
                sb.AppendLine($"\t\t</RSD>");

                sb.AppendLine("\t</RES>");
            }

            //end
            sb.AppendLine("</RPK>");

            return sb.ToString();
        }
    }
}