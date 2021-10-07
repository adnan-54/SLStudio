using System;

namespace SLStudio
{
	public class MenuSeparator : MenuItem, IMenuSeparator
	{
		public override void AddChild(IMenuItem child)
		{
			throw new InvalidOperationException("A menu separator cannot have children");
		}
	}
}
