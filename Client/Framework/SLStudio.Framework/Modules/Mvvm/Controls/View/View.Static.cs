using System.Windows;

namespace SLStudio
{
    public partial class View
    {
        static View()
        {
            ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(IViewModel), typeof(View));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(View), new FrameworkPropertyMetadata(typeof(View)));
        }

        public static readonly DependencyProperty ViewModelProperty;
    }
}
