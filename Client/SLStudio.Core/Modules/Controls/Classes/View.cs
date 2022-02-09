using System.Windows.Controls;

namespace SLStudio;

public partial class View : ContentControl
{
    public IViewModel ViewModel
    {
        get => (IViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
}
