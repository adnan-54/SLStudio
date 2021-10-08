namespace SLStudio
{
    public interface IWindowManager
    {
        TViewModel Show<TViewModel>()
            where TViewModel : class, IWindowViewModel;

        TViewModel ShowDialog<TViewModel>()
            where TViewModel : class, IWindowViewModel;
    }
}