using Caliburn.Micro;
using SLStudio.Studio.Core.Modules.StatusBar.ViewModels;
using System.Windows;

namespace SLStudio.Studio.Core.Modules.StatusBar
{
    public interface IStatusBar
    {
        IObservableCollection<StatusBarItemViewModel> Items { get; }

        void AddItem(string message, GridLength width);
    }
}