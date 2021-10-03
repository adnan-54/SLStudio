using SLStudio.Core.Modules.Options.ViewModels;
using System.Threading.Tasks;
using System.Windows;

namespace SLStudio.Core.Menus.Handlers
{
    internal class ShowOptionsHandler : MenuCommandHandler
    {
        private readonly IWindowManager windowManager;
        private readonly IThemeManager themeManager;
        private readonly ILanguageManager languageManager;

        public ShowOptionsHandler(IWindowManager windowManager, IThemeManager themeManager, ILanguageManager languageManager)
        {
            this.windowManager = windowManager;
            this.themeManager = themeManager;
            this.languageManager = languageManager;
        }

        public override Task Execute(IMenuItem menu, object parameter)
        {
            var options = new OptionsViewModel(themeManager, languageManager);
            windowManager.ShowDialog(options);
            return Task.CompletedTask;
        }
    }
}