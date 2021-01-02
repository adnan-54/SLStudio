namespace SLStudio.FileTypes.CfgFile
{
    public class Cfg : GameFile
    {
        public ConfigurationType ConfigurationType { get; set; }
    }

    public enum ConfigurationType
    {
        Car,
        Part
    }
}