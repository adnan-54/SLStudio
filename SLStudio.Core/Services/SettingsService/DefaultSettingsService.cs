using Caliburn.Micro;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;

namespace SLStudio.Core.Services.SettingsService
{
    internal class DefaultSettingsService : ISettingsService
    {
        private readonly Dictionary<ViewModel, ApplicationSettingsBase> registredViewModels;

        public DefaultSettingsService()
        {
            registredViewModels = new Dictionary<ViewModel, ApplicationSettingsBase>();
        }

        public void RegisterSettingFor(ViewModel viewModel, ApplicationSettingsBase settings)
        {
            viewModel.Activated += OnViewModelActivated;
            viewModel.Deactivated += OnViewModelDeactivated;
            viewModel.PropertyChanged += OnViewModelPropertyChanged;
            settings.PropertyChanged += OnSettingsPropertyChanged;
            registredViewModels.Add(viewModel, settings);
        }

        private void OnViewModelActivated(object sender, ActivationEventArgs e)
        {
            if (sender is ViewModel viewModel)
            {
                registredViewModels.TryGetValue(viewModel, out ApplicationSettingsBase settings);
                var settingsProperties = settings.Properties;
                foreach (SettingsProperty property in settingsProperties)
                    viewModel.GetType().GetProperty(property.Name).SetValue(viewModel, settings[property.Name]);
            }
        }

        private void OnViewModelDeactivated(object sender, DeactivationEventArgs e)
        {
            if (sender is ViewModel viewModel)
            {
                registredViewModels.TryGetValue(viewModel, out ApplicationSettingsBase settings);
                viewModel.Activated -= OnViewModelActivated;
                viewModel.Deactivated -= OnViewModelDeactivated;
                viewModel.PropertyChanged -= OnViewModelPropertyChanged;
                settings.PropertyChanged -= OnSettingsPropertyChanged;
                registredViewModels.Remove(viewModel);
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is ViewModel viewModel)
            {
                registredViewModels.TryGetValue(viewModel, out ApplicationSettingsBase settings);
                settings.PropertyChanged -= OnSettingsPropertyChanged;
                try
                {
                    settings[e.PropertyName] = viewModel.GetType().GetProperty(e.PropertyName).GetValue(viewModel);
                    settings.Save();
                }
                catch (SettingsPropertyNotFoundException)
                {
                }
                finally
                {
                    settings.PropertyChanged += OnSettingsPropertyChanged;
                }
            }
        }

        private void OnSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is ApplicationSettingsBase settings)
            {
                var viewModel = registredViewModels.FirstOrDefault(k => k.Value == settings).Key;
                if (viewModel != null)
                {
                    viewModel.PropertyChanged -= OnViewModelPropertyChanged;
                    viewModel.GetType().GetProperty(e.PropertyName).SetValue(viewModel, settings[e.PropertyName]);
                    settings.Save();
                    viewModel.PropertyChanged += OnViewModelPropertyChanged;
                }
            }
        }
    }
}