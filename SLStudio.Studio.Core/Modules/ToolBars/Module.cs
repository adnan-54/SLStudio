using SLStudio.Studio.Core.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;

namespace SLStudio.Studio.Core.Modules.ToolBars
{
    [Export(typeof(IModule))]
    public class Module : ModuleBase
    {
        public override IEnumerable<ResourceDictionary> GlobalResourceDictionaries
        {
            get
            {
                yield return new ResourceDictionary
                {
                    Source = new Uri("/SLStudio.Studio.Core;component/Modules/ToolBars/Resources/Styles.xaml", UriKind.Relative)
                };
            }
        }
    }
}