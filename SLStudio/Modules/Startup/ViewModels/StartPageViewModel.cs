using Gemini.Framework;
using SLStudio.Logging;
using SLStudio.Modules.Startup.Resources;

namespace SLStudio.Modules.Startup.ViewModels
{
    class StartPageViewModel: Document
    {
        static readonly ILog logger = LogManager.GetLogger(nameof(StartPageViewModel));

        public StartPageViewModel()
        {
            DisplayName = StartPageResources.displayName;
            logger.Debug("fon");
        }
    }
}
