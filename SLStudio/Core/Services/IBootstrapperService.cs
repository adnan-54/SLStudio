using Caliburn.Micro;

namespace SLStudio.Core
{
    class BootstrapperService : IBootstrapperService
    {
        private readonly SimpleContainer container;
        private readonly ISplashScreenService splashScreen;

        public BootstrapperService(SimpleContainer container, ISplashScreenService splashScreen)
        {
            this.container = container;
            this.splashScreen = splashScreen;
        }

        public void Initialize()
        {
            splashScreen.Show();
        }
    }

    public interface IBootstrapperService
    {
        void Initialize();
    }
}
