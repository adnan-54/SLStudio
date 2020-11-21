using System;
using System.Text.RegularExpressions;

namespace SLStudio.RpkEditor
{
    public static class StringExtensions
    {
        public static int ToIntId(this string id)
        {
            if (string.IsNullOrEmpty(id) || id.StartsWith('-'))
                return -1;

            if (id.StartsWith("0x", StringComparison.InvariantCultureIgnoreCase))
            {
                if (Regex.IsMatch(id, @"0[xX][0-9a-fA-F]+"))
                    return Convert.ToInt32(id, 16);
                return -1;
            }

            try
            {
                return Convert.ToInt32(id);
            }
            catch
            {
                return -1;
            }
        }
    }
}