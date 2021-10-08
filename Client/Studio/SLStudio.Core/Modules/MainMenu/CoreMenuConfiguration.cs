namespace SLStudio.Core
{
    internal class CoreMenuConfiguration : MenuConfiguration
    {
        protected override void Build()
        {
            Item("file|");
            Item("file|save|");
            Separator("file|separator|");
            Item("file|open|");
            Item("edit|");
            Item("view|");
        }
    }
}