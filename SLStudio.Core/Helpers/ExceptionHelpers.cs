using System;

namespace SLStudio.Core.Helpers
{
    public static class ExceptionHelpers
    {
        public static string FindOriginalSource(Exception exception)
        {
            while (exception.InnerException != null)
                exception = exception.InnerException;

            return exception.TargetSite.DeclaringType.Name;
        }
    }
}