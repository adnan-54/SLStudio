using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    public interface IWindowManager
    {
        Task<bool?> ShowDialog(object rootModel, object context = null, IDictionary<string, object> settings = null);

        Task<bool?> ShowDialog<TType>(object context = null, IDictionary<string, object> settings = null);

        Task ShowWindow(object rootModel, object context = null, IDictionary<string, object> settings = null);

        Task ShowWindow<TType>(object context = null, IDictionary<string, object> settings = null);

        Task ShowPopup(object rootModel, object context = null, IDictionary<string, object> settings = null);

        Task ShowPopup<TType>(object context = null, IDictionary<string, object> settings = null);

        Task CloseWindow(object rootModel);

        Task CloseWindow<TType>();
    }
}