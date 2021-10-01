using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SLStudio.Core
{
    public static class StringExtensions
    {
        private const char DEFAULT_INVALID_CHAR_REPLACEMENT = '_';

        private static readonly IEnumerable<string> invalidFileNames = new List<string>()
        {
            "AUX", "CON", "COM0", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
            "CLOCK$", "PRN", "LPT0", "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9", "NUL"
        };

        private static readonly IEnumerable<char> invalidFileNameChars = new List<char>(Path.GetInvalidFileNameChars());

        public static string ToFileName(this string source, char replacement = '_')
        {
            if (invalidFileNameChars.Contains(replacement))
                replacement = DEFAULT_INVALID_CHAR_REPLACEMENT;

            if (source.Count(s => s == '.') == source.Length)
                return string.Empty;

            foreach (var invalidName in invalidFileNames)
            {
                if (source.Equals(invalidName, StringComparison.OrdinalIgnoreCase))
                    return string.Empty;
            }

            foreach (var invalidChar in Path.GetInvalidFileNameChars())
                source = source.Replace(invalidChar, '_');

            return source;
        }
    }
}