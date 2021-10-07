using SLStudio.Logging;
using System;

namespace SLStudio
{
	public class MenuToggle : MenuButton, IMenuToggle
	{
		private static readonly ILogger logger = LogManager.GetLogger<MenuToggle>();

		private readonly IMenuToggleHandler handler;

		public MenuToggle(IMenuToggleHandler handler)
		{
			this.handler = handler;
		}

		public bool IsChecked
		{
			get => GetValue<bool>();
			set => SetValue(value, OnToggled);
		}

		public void Check()
		{
			IsChecked = true;
		}

		public void Uncheck()
		{
			IsChecked = false;
		}

		public void Toggle()
		{
			IsChecked = !IsChecked;
		}

		private async void OnToggled()
		{
			try
			{
				await handler.OnToggle();
			}
			catch (Exception ex)
			{
				logger.Warn(ex);
			}
		}
	}
}
