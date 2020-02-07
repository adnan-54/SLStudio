using System;
using System.Diagnostics;

namespace SLStudio.Core.CoreModules.ErrorHandler
{
    internal class DefaultErrorHandler : IErrorHandler
    {
        public void HandleError(Exception exception)
        {
            Debug.WriteLine(exception);
            Console.WriteLine(exception);

            //Todo: Log the exception
            throw new NotImplementedException();
        }
    }
}
