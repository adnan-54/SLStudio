using System;

namespace SLStudio.NotificationCenter
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class NotificationModuleAttribute : Attribute
    {
        public NotificationModuleAttribute()
        {
        }
    }
}