using SLStudio.Core.Helpers;
using System;

namespace SLStudio.Core
{
    public static class ExceptionExtensions
    {
        public static string FindOriginalSource(this Exception exception)
        {
            return ExceptionHelpers.FindOriginalSource(exception);
        }
    }
}