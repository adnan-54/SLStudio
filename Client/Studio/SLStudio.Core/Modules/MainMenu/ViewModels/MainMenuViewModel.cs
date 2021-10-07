using System;
using System.Collections.Generic;
using System.Linq;

namespace SLStudio.Core
{
    internal class MainMenuViewModel : ViewModel, IMainMenu
    {
        private readonly IMenuBuilder menuBuilder;
        private readonly BindableCollection<IMenuItem> menus;

        public MainMenuViewModel(IMenuBuilder menuBuilder)
        {
            this.menuBuilder = menuBuilder;
            menus = new BindableCollection<IMenuItem>();

            Loaded += OnLoaded;
        }

        public IReadOnlyCollection<IMenuItem> Menus => menus;

        private void OnLoaded(object sender, EventArgs e)
        {
            if (menus.Any())
                return;

            menus.AddRange(menuBuilder.BuildMenus());
        }

        public IMenuItem GetFromPath(string path)
        {
            return menuBuilder.Menus[path];
        }
    }

    public interface IMainMenu : IViewModel
    {
        IReadOnlyCollection<IMenuItem> Menus { get; }

        IMenuItem GetFromPath(string path);
    }
}