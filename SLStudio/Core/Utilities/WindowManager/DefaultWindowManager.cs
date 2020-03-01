using Caliburn.Micro;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SLStudio.Core.Utilities.WindowManager
{
    public class DefaultWindowManager : Caliburn.Micro.WindowManager, IWindowManager
    {
        public Task CloseWindow(object rootModel)
        {
            if (rootModel is IClose closeObject)
                return closeObject.TryCloseAsync();

            return Task.FromResult(true);
        }

        public Task CloseWindow<TType>()
        {
            var rootModel = IoC.Get<TType>();
            return CloseWindow(rootModel);
        }

        public Task<bool?> ShowDialog(object rootModel, object context = null, IDictionary<string, object> settings = null)
        {
            return base.ShowDialogAsync(rootModel, context, settings);
        }

        public Task<bool?> ShowDialog<TType>(object context = null, IDictionary<string, object> settings = null)
        {
            var rootModel = IoC.Get<TType>();
            return ShowDialog(rootModel, context, settings);
        }

        public Task ShowPopup(object rootModel, object context = null, IDictionary<string, object> settings = null)
        {
            return base.ShowPopupAsync(rootModel, context, settings);
        }

        public Task ShowPopup<TType>(object context = null, IDictionary<string, object> settings = null)
        {
            var rootModel = IoC.Get<TType>();
            return ShowPopup(rootModel, context, settings);
        }

        public Task ShowWindow(object rootModel, object context = null, IDictionary<string, object> settings = null)
        {
            return base.ShowWindowAsync(rootModel, context, settings);
        }

        public Task ShowWindow<TType>(object context = null, IDictionary<string, object> settings = null)
        {
            var rootModel = IoC.Get<TType>();
            return ShowWindow(rootModel, context, settings);
        }
    }
}