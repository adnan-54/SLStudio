using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace SLStudio.Studio.Core.Framework.Themes
{
    [Export(typeof(ITheme))]
    public class LightTheme : ITheme
    {
        public virtual string Name
        {
            get { return "Light"; }
        }

        public virtual IEnumerable<Uri> ApplicationResources
        {
            get
            {
                yield return new Uri("pack://application:,,,/Xceed.Wpf.AvalonDock.Themes.VS2013;component/LightTheme.xaml");
                yield return new Uri("pack://application:,,,/SLStudio.Studio.Core;component/Resources/Themes/VS2013/LightTheme.xaml");
            }
        }

        public virtual IEnumerable<Uri> MainWindowResources
        {
            get { yield break; }
        }
    }
}