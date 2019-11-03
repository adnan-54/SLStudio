using Caliburn.Micro;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SLStudio.Studio.Core.Framework.Services
{
    public static class ExtensionMethods
    {
        public static string GetExecutingAssemblyName()
        {
            return Assembly.GetExecutingAssembly().GetAssemblyName();
        }

        public static string GetPropertyName<TProperty>(Expression<Func<TProperty>> property)
        {
            return property.GetMemberInfo().Name;
        }

        public static string GetPropertyName<TTarget, TProperty>(Expression<Func<TTarget, TProperty>> property)
        {
            return property.GetMemberInfo().Name;
        }
    }
}
