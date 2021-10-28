using System.Windows.Controls;

namespace SLStudio
{
    public partial class View : Control
    {
        public IViewModel ViewModel
        {
            get => (IViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
    }
}