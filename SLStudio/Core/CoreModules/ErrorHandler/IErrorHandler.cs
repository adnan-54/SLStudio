using System;

namespace SLStudio.Core
{
    public interface IErrorHandler
    {
        void HandleError(Exception exception);
    }
}
