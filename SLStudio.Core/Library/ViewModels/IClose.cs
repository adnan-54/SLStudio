namespace SLStudio.Core
{
    public interface IClose
    {
        void TryClose();

        void TryClose(bool? dialogResult);
    }
}