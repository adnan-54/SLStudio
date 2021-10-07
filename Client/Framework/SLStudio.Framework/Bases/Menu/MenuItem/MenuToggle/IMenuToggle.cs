using System.Windows.Input;

namespace SLStudio
{
    public interface IMenuToggle : IMenuItem
    {
        KeyGesture Shortcut { get; }

        bool IsChecked { get; }

        bool IsToggling { get; }

        bool CanToggle { get; }

        void Check();

        void Uncheck();

        void Toggle();
    }
}
