using SimpleInjector;

namespace SLStudio
{
    internal static class LifeStyleExtensions
    {
        public static Lifestyle ToLifestyle(this LifeStyle lifeStyle)
        {
            return lifeStyle switch
            {
                LifeStyle.Singleton => Lifestyle.Singleton,
                LifeStyle.Transient => Lifestyle.Transient,
                _ => Lifestyle.Transient
            };
        }
    }
}