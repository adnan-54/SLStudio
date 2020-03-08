using Caliburn.Micro;
using System;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    public static class TaskExtensions
    {
        public static async void FireAndForget(this Task task, IErrorHandler errorHandler = null)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                if (errorHandler == null)
                    errorHandler = IoC.Get<IErrorHandler>();

                errorHandler.HandleError(ex);
            }
        }
    }
}