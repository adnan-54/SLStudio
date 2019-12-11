using Caliburn.Micro;
using System.Reflection;

namespace SLStudio.Modules.SplashScreen.ViewModels
{
    class SplashScreenViewModel : Screen
    {
        public SplashScreenViewModel()
        {
            LoadingObject = Assembly.GetExecutingAssembly().GetName().Name;
        }

        private string loadingObject;
        public string LoadingObject
        {
            get => loadingObject;
            set
            {
                loadingObject = value;
                NotifyOfPropertyChange(() => LoadingObject);
            }
        }
    }
}
