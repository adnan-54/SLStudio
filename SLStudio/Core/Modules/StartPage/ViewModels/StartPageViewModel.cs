using Caliburn.Micro;

namespace SLStudio.Core.Modules.StartPage.ViewModels
{
    internal class StartPageViewModel : Screen, IStartPage
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