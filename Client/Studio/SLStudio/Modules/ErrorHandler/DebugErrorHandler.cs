using System;
using System.Diagnostics;
using SLStudio.Logging;

namespace SLStudio
{
    internal class DebugErrorHandler : ErrorHandler
    {
        private static readonly ILogger logger = LogManager.GetLogger<DebugErrorHandler>();

        protected override void Handle(Exception exception)
        {
            Debug.WriteLine(exception);
            logger.Fatal(exception);
            LogManager.RequestDump();
        }
    }
}