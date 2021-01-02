using SLStudio.FileTypes.Attributes;
using SLStudio.FileTypes.CfgFile;
using SLStudio.FileTypes.ClassFile;

namespace SLStudio.FileTypes.RpkFile
{
    public class GameObjectDefinition : ResourceBase
    {
        public override ResourceType TypeOfEntry => ResourceType.GameObject;

        public Class Class { get; set; }

        public Cfg Configuration { get; set; }

        [ResourceAttribute("script", 0)]
        public string Script => Class.Path;

        [ResourceAttribute("native", 1)]
        public string Native => $"{Configuration.ConfigurationType} {Configuration.Path}";
    }
}