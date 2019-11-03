using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace SLStudio.Studio.Core.Framework.Themes
{
    [Export(typeof(ITheme))]
    public class BlueTheme : ITheme
    {
        public virtual string Name
        {
            get { return "Blue"; }
        }

        public virtual IEnumerable<Uri> ApplicationResources
        {
            get
            {
                yield return new Uri("pack://application:,,,/Xceed.Wpf.AvalonDock.Themes.VS2013;component/BlueTheme.xaml");
                yield return new Uri("pack://application:,,,/SLStudio.Studio.Core;component/Resources/Themes/VS2013/BlueTheme.xaml");
            }
        }

        public virtual IEnumerable<Uri> MainWindowResources
        {
            get { yield break; }
        }
    }
}
