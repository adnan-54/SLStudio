using System;

namespace SLStudio.Core
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            return source != null && value != null && source.IndexOf(value, comparisonType) >= 0;
        }
    }
}