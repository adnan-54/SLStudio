namespace SLStudio.Core
{
	internal class CoreMenuConfiguration : MenuConfiguration
	{
		protected override void Build()
		{
			Item("file|");
			Item("edit|");
			Item("view|");
		}
	}
}
