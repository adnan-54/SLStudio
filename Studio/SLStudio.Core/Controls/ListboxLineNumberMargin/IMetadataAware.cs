using System.Windows.Controls;

namespace SLStudio.Core.Controls
{
    public interface IMetadataAware
    {
        void Attach(ItemsControl control);

        void Detach();
    }
}