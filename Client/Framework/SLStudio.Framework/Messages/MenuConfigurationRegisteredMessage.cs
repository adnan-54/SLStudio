namespace SLStudio
{
	internal class MenuConfigurationRegisteredMessage
	{
		public MenuConfigurationRegisteredMessage(IMenuConfiguration menuConfiguration)
		{
			MenuConfiguration = menuConfiguration;
		}

		public IMenuConfiguration MenuConfiguration { get; }
	}
}
