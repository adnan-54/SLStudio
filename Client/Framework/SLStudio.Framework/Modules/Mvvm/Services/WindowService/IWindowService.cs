namespace SLStudio
{
    public interface IWindowService
    {
        void Activate();

        void Maximize();

        void Restore();

        void Minimize();

        void Close();
    }
}