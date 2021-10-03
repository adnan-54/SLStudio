using SLStudio.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SLStudio
{
    public abstract class MenuConfiguration : IMenuConfiguration
    {
        protected MenuConfiguration()
        {
        }

        protected void Build()
        {
        }

        protected void SetResourceContext()
        {
        }

        protected void Separator(string path, int? index, bool isVisible = true)
        {
        }

        protected void Button<THandler>() where THandler : MenuButtonHandler
        {
        }

        protected void Toggle<THandler>() where THandler : MenuToggleHandler
        {
        }
    }

    public interface IMenuConfiguration
    {
        IEnumerable<IMenuItem> Items { get; }
    }

    public interface IMenuItemFactory : IService
    {
        IMenuItem CreateSeparator();
    }
}