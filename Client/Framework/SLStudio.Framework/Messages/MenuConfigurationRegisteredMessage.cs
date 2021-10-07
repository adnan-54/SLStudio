using System;

namespace SLStudio
{
    internal class MenuConfigurationRegisteredMessage
    {
        public MenuConfigurationRegisteredMessage(Type menuConfigurationType)
        {
            MenuConfigurationType = menuConfigurationType;
        }

        public Type MenuConfigurationType { get; }
    }
}
