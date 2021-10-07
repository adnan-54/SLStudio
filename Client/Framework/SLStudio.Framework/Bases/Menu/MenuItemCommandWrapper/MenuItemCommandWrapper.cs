using SLStudio.Logging;
using System;
using System.Windows.Input;

namespace SLStudio
{
	internal class MenuItemCommandWrapper<THandler, TMenuItem> : ICommand
		where THandler : class, IMenuItemHandler<TMenuItem>
		where TMenuItem : class, IMenuItem
	{
		private static readonly ILogger logger = LogManager.GetLogger(typeof(MenuItemCommandWrapper<,>));

		private readonly THandler handler;

		private bool isExecuting;

		public MenuItemCommandWrapper(THandler handler)
		{
			this.handler = handler;
		}

		public event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
			remove => CommandManager.RequerySuggested -= value;
		}

		public bool CanExecute(object parameter)
		{
			return !isExecuting && handler.CanExecute(parameter);
		}

		public async void Execute(object parameter)
		{
			if (isExecuting)
				return;

			try
			{
				isExecuting = true;

				await handler.Execute(parameter);
			}
			catch (Exception ex)
			{
				logger.Warn(ex);
			}
			finally
			{
				isExecuting = false;
			}
		}
	}
}
