using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLStudio.Core
{
    internal class DefaultSettingsManager : ISettingsManager
    {
        private readonly Dictionary<Type, ApplicationSettingsBase> settings;

        public DefaultSettingsManager()
        {
            settings = new Dictionary<Type, ApplicationSettingsBase>();
        }

        public void Register(IHaveSettings host, ApplicationSettingsBase settings)
        {
            var hostType = host.GetType();
            if (!this.settings.TryGetValue(hostType, out var settting))
            {
                this.settings.Add(hostType, settings);
            }
        }

        public void Reset()
        {
            foreach (var setting in settings.Values)
            {
                setting.Reset();
                setting.Save();
            }
        }
    }

    internal interface ISettingsManager
    {
        void Register(IHaveSettings host, ApplicationSettingsBase settings);

        void Reset();
    }
}