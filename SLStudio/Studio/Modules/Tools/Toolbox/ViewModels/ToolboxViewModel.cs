using Gemini.Framework;
using Gemini.Framework.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Studio.Modules.Tools.Toolbox.ViewModels
{
    [Export(typeof(IToolbox))]
    class ToolboxViewModel : Tool, IToolbox
    {
        public ToolboxViewModel()
        {
            DisplayName = "Toolbox2";
        }

        public override PaneLocation PreferredLocation => PaneLocation.Left;
    }
}
