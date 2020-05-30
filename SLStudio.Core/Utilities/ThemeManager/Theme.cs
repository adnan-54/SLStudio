using System.Collections.Generic;

namespace SLStudio.Core
{
    public class Theme
    {
        public Theme()
        {
            Palette = new Dictionary<string, string>();
        }

        public bool IsDefault { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string BaseColor { get; set; }
        public string ColorScheme { get; set; }
        public Dictionary<string, string> Palette { get; set; }
    }
}