using Caliburn.Micro;

namespace SLStudio.Core.Modules.StartPage.ViewModels
{
    class StartPageViewModel : Screen, IStartPage
    {
        public StartPageViewModel()
        {
            DisplayName = "SLStudio";
        }
    }

    public interface IStartPage
    {

    }
}
