namespace SLStudio;

public interface IView
{
    event DependencyPropertyChangedEventHandler DataContextChanged;

    event EventHandler Initialized;

    event RoutedEventHandler Loaded;

    event RoutedEventHandler Unloaded;

    object DataContext { get; }

    bool IsInitialized { get; }

    bool IsLoaded { get; }

    bool IsEnabled { get; }

    bool IsFocused { get; }

    object Tag { get; }

    Visibility Visibility { get; }
}