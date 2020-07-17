using DevExpress.Mvvm;

namespace SLStudio.Core
{
    public interface IWindow : IHaveDisplayName, IClose, ISupportServices
    {
        void Activate();

        void Minimize();

        void Restore();

        void Maximize();
    }
}