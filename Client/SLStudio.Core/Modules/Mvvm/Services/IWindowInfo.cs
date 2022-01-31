namespace SLStudio;

public interface IWindowInfo
{
    IWindowView Window { get; }

    IWindowViewModel ViewModel { get; }

    bool IsModal { get; }

    bool IsDialog { get; }
}