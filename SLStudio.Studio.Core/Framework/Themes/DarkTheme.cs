using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace SLStudio.Studio.Core.Framework.Themes
{
    [Export(typeof(ITheme))]
    public class DarkTheme : ITheme
    {
        public virtual string Name
        {
            get { return Properties.Resources.ThemeDarkName; }
        }

        public virtual IEnumerable<Uri> ApplicationResources
        {
            get
            {
                yield return new Uri("pack://application:,,,/Xceed.Wpf.AvalonDock.Themes.VS2013;component/DarkTheme.xaml");
                yield return new Uri("pack://application:,,,/SLStudio.Studio.Core;component/Themes/VS2013/DarkTheme.xaml");
            }
        }

        public virtual IEnumerable<Uri> MainWindowResources
        {
            get { yield break; }
        }
    }
}