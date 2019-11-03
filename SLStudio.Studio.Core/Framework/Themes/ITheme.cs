using System;
using System.Collections.Generic;

namespace SLStudio.Studio.Core.Framework.Themes
{
    public interface ITheme
    {
        string Name { get; }
        
        IEnumerable<Uri> ApplicationResources { get; }
        
        IEnumerable<Uri> MainWindowResources { get; }
    }
}