using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI;
using SLStudio.Core.Modules.Shell.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SLStudio.Core.Menus
{
    internal class MenuManagerService : ServiceBaseGeneric<Menu>
    {
        private readonly IMenuLookup menuLookup;
        private readonly IUiSynchronization uiSynchronization;

        private readonly BindableCollection<IMenuItem> menus;

        public MenuManagerService() : this(IoC.Get<IMenuLookup>(), IoC.Get<IUiSynchronization>())
        {
        }

        public MenuManagerService(IMenuLookup menuLookup, IUiSynchronization uiSynchronization)
        {
            this.menuLookup = menuLookup;
            this.uiSynchronization = uiSynchronization;

            menus = new BindableCollection<IMenuItem>();
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DataContextChanged += OnDataContextChanged;
        }

        private async void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            await Build();
        }

        private async Task Build()
        {
            if (AssociatedObject.DataContext == null || !(AssociatedObject.DataContext is ShellViewModel))
                return;
            AssociatedObject.DataContextChanged -= OnDataContextChanged;

            var items = Enumerable.Empty<IMenuItem>();
            await Task.Run(() =>
            {
                var configurations = menuLookup.Lookup();
                items = Merge(configurations.SelectMany(c => c.Items)).OrderBy(m => m.Index).ToArray();
                uiSynchronization.EnsureExecuteOnUi(() =>
                {
                    RegisterShellShortcuts(items);
                });
            });

            menus.Clear();
            menus.AddRange(items);
            AssociatedObject.ItemsSource = menus;
        }

        private IEnumerable<IMenuItem> Merge(IEnumerable<IMenuItem> items)
        {
            foreach (var group in items.GroupBy(i => i.Path))
            {
                var children = group.SelectMany(m => m.Children).ToArray();

                var main = group.LastOrDefault(m => !string.IsNullOrEmpty(m.DisplayName)) ?? group.LastOrDefault();
                main.Children.Clear();
                main.Children.AddRange(Merge(children).OrderBy(m => m.Index));

                yield return main;
            }
        }

        private void RegisterShellShortcuts(IEnumerable<IMenuItem> items)
        {
            var shellWindow = Window.GetWindow(AssociatedObject);

            var existingBindings = shellWindow.InputBindings.OfType<InputBinding>().ToArray();

            var inputBindings = items.Flatten(m => m.Command != null && m.Shortcut != null, m => m.Children)
                                     .Select(m => new InputBinding(m.Command, m.Shortcut) { CommandParameter = m.Parameter })
                                     .ToList();

            foreach (var item in inputBindings.ToArray())
                if (!existingBindings.Any(x => x.Gesture == item.Gesture))
                    shellWindow.InputBindings.Add(item);
        }
    }
}