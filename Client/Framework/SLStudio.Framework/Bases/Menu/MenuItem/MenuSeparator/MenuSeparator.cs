using System;

namespace SLStudio
{
    internal class MenuSeparator : MenuItem, IMenuSeparator
    {
        public override void AddChild(IMenuItem child)
        {
            throw new InvalidOperationException("A menu separator cannot have children");
        }
    }
}