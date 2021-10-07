using System.ComponentModel;

namespace SLStudio
{
    public interface INotifyPropertyChangedEx : INotifyPropertyChanged
    {
        bool IsNotifying { get; set; }

        void NotifyOfPropertyChange(string propertyName);

        void Refresh();
    }
}