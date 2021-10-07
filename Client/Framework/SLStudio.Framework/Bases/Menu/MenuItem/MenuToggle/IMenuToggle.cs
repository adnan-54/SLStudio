namespace SLStudio
{
    public interface IMenuToggle : IMenuButton
    {
        bool IsChecked { get; }

        void Check();

        void Uncheck();

        void Toggle();
    }
}
