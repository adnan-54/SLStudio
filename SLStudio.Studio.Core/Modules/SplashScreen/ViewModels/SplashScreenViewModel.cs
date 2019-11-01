using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Studio.Core.Modules.SplashScreen.ViewModels
{
    [Export(typeof(SplashScreenViewModel))]
    class SplashScreenViewModel : Screen, ISplashScreen
    {
        public SplashScreenViewModel()
        {
            DisplayName = "SLStudio";
        }
    }
}
