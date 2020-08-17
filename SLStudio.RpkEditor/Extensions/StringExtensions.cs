using System;

namespace SLStudio.RpkEditor
{
    public static class StringExtensions
    {
        public static int ToIntId(this string id)
        {
            if (string.IsNullOrEmpty(id))
                return -1;

            var sanitized = id.Replace("0x", "");
            return Convert.ToInt32(sanitized, 16);
        }
    }
}