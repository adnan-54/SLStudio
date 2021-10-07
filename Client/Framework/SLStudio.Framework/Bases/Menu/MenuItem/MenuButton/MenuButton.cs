using System.Windows.Input;

namespace SLStudio
{
	public class MenuButton : MenuItem, IMenuButton
	{
		public KeyGesture Shortcut { get; init; }

		public ICommand Command { get; init; }
	}
}
