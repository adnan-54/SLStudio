using SLStudio.FileTypes.Attributes;
using SLStudio.FileTypes.SoundFile;
using System;

namespace SLStudio.FileTypes.RpkFile
{
    public class SoundDefinition : ResourceDefinition
    {
        internal override ResourceType TypeOfEntry => ResourceType.Sound;

        public Sound Sound { get; set; }

        [ResourceAttribute("sourcefile", 0)]
        public string SourceFile => Sound.Path;

        [ResourceAttribute("volume", 1)]
        public double Volume { get; set; }

        [ResourceAttribute("mindist", 2)]
        public double MinDist { get; set; }

        [ResourceAttribute("maxdist", 3)]
        public double MaxDist { get; set; }

        [ResourceAttribute("instances", 4)]
        public int Instances { get; set; }

        [ResourceAttribute("flags", 5)]
        public SoundFlags Flags { get; set; }
    }

    [Flags]
    public enum SoundFlags
    {
        Unknown = 1
    }
}