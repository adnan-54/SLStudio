using System.ComponentModel;

namespace SLStudio
{
    public interface IClosable
    {
        void OnClosing(CancelEventArgs args);

        void OnClosed();
    }
}